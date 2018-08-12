using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class humanCtrl : MonoBehaviour {

    public float volume = 1f;
    public Transform bloodParticle;
    private Transform bloodParticlePos;

    public LayerMask smashTargetLayer;

    public LayerMask groundLayer;
    public float height = 2.7f;
    public Vector3 multiMovementDir = new Vector3(150f, 500f, 150f);
    public float minMoveInterval = 1f, maxMoveInterval = 2f;

    private Rigidbody bodyRb;
    private Vector3 targetDir;
    private float nextMoveTime;

    private bool isDeath = false, isGround = true, isLoaded = false, fired = false, panicing = false;
    public float curFallSpeed = 0f, fallSpeed = 0.05f;
    private Vector3 flySpeed;

    private Transform model, armCtrl;

    // Use this for initialization
    public void Awake () {
        model = transform.Find("model");
        armCtrl = model.Find("armCtrl");
        bodyRb = GetComponent<Rigidbody>();
        cageCtrl.Register(volume);
        nextMoveTime = Time.time + Random.Range(minMoveInterval, maxMoveInterval);
        bloodParticlePos = transform.Find("bloodParticlePos");
    }
	
	// Update is called once per frame
	void Update () {
        if (isLoaded)
        {
            if (fired)
            {
                Fly();
            }
        }
        else
        {
            Panicing((cageCtrl.stressLevel > 1.1f));
            if (Time.time > nextMoveTime)
            {
                GroundCheck();
                if (isGround)
                    RandomMove();
                nextMoveTime = Time.time + Random.Range(minMoveInterval, maxMoveInterval);
            }
        }
	}

    void RandomMove()
    {
        targetDir = Random.insideUnitCircle;
        targetDir.z = targetDir.y;
        targetDir.y = Random.Range(0.7f, 1f);
        bodyRb.AddForce(Vector3.Scale(targetDir, multiMovementDir));
        targetDir.y = 0;
        transform.rotation = Quaternion.LookRotation(targetDir);
    }

    void GroundCheck()
    {
        isGround = Physics.Raycast(transform.position, -transform.up, height * 1.1f, groundLayer);
    }

    void InitClothing()
    {

    }

    public void Panicing(bool isPanicing)
    {
        if (panicing == isPanicing) return;
        if (isPanicing)
        {
            Vector3 handsRot = armCtrl.localEulerAngles;
            handsRot.x = Random.Range(-20f, 60f);
            handsRot.z = 180f;
            armCtrl.localEulerAngles = handsRot;
        }
        else
        {
            armCtrl.localEulerAngles = Vector3.zero;
        }
        panicing = isPanicing;
    }

    virtual public void Loaded(Transform shooter)
    {
        gameObject.layer = LayerMask.NameToLayer("Astronaut");
        isLoaded = true;
        bodyRb.useGravity = false;
        bodyRb.isKinematic = true;
        Vector3 handsRot = armCtrl.localEulerAngles;
        handsRot.z = 180f;
        armCtrl.localEulerAngles = handsRot;
        Vector3 loadedPos = shooter.position;
        loadedPos.y += height;
        transform.position = loadedPos;
        transform.parent = shooter;
        cageCtrl.Dismiss(volume);
    }

    void Fly()
    {
        flySpeed.y -= curFallSpeed;
        curFallSpeed += fallSpeed;
        bodyRb.velocity = flySpeed;
        if (bodyRb.velocity.y < 0)
        {
            if (transform.position.y - curFallSpeed * Time.deltaTime <= 0.5f) //0.5 is hardcode ground height
            {
                Vector3 curPos = transform.position;
                curPos.y = 0.5f;
                transform.position = curPos;
                Collider[] smashTargets = Physics.OverlapSphere(transform.position, transform.localScale.x * 0.6f, smashTargetLayer);
                foreach (Collider col in smashTargets)
                {
                    col.transform.GetComponent<humanCtrl>().Death();
                }
                isDeath = true;
                Instantiate(bloodParticle, bloodParticlePos.position, bloodParticlePos.rotation);
                GameObject.Find("Second Camera").GetComponent<followAst>().Dismiss();
                GameObject.Find("MouseCtrl").GetComponent<mouseCtrl>().RestartAimming();
                BackgroundSetting.SetCurHeight(0);
                Destroy(gameObject);
                return;
            }
        }
        else
        {
            Debug.Log(Mathf.Max(Mathf.RoundToInt(transform.position.y - height), 0));
            BackgroundSetting.SetCurHeight(Mathf.Max(Mathf.RoundToInt(transform.position.y - height), 0));
        }
    }

    public void Fire(float power)
    {
        fired = true;
        flySpeed = Vector3.zero;
        flySpeed.y = power;
        bodyRb.isKinematic = false;
        transform.rotation = Quaternion.identity;
        bodyRb.constraints = RigidbodyConstraints.FreezeRotationY;
        bodyRb.velocity = Vector3.zero;
        GameObject.Find("Second Camera").GetComponent<followAst>().Register(transform);
    }

    public void Death()
    {
        isDeath = true;
        cageCtrl.Dismiss(volume);
        //instant blood particle
        Instantiate(bloodParticle, bloodParticlePos.position, bloodParticlePos.rotation);
        Destroy(gameObject);
    }

    virtual public void InitMat(Material skin, Material clothing, Material pant, Material shoe)
    {
        model.Find("head").GetComponent<Renderer>().material = skin;
        armCtrl.Find("leftArm/leftHand").GetComponent<Renderer>().material = skin;
        armCtrl.Find("rightArm/rightHand").GetComponent<Renderer>().material = skin;
        
        model.Find("body").GetComponent<Renderer>().material = clothing;
        armCtrl.Find("leftArm").GetComponent<Renderer>().material = clothing;
        armCtrl.Find("rightArm").GetComponent<Renderer>().material = clothing;

        model.Find("leftLeg").GetComponent<Renderer>().material = pant;
        model.Find("rightLeg").GetComponent<Renderer>().material = pant;

        model.Find("leftLeg/leftFoot").GetComponent<Renderer>().material = shoe;
        model.Find("rightLeg/rightFoot").GetComponent<Renderer>().material = shoe;
    }
}
