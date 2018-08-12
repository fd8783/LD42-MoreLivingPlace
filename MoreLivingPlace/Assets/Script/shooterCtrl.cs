using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class shooterCtrl : MonoBehaviour {

    public LayerMask smashTargetLayer;
    public Transform destroyParticle;
    private Transform destroyParticlePos;
    public float smashRange = 1.2f, volume = 1f;

    private Vector3 targetPos, curPos;
    private bool reachTarget = true, loaded = false;
    private float fallingSpeed = 10f, curFallingSpeed = 10f;
    private Transform loadedHuman;

    private ParticleSystem landingParticle;

    // Use this for initialization
    void Awake () {
        fallingSpeed *= Time.deltaTime;
        curFallingSpeed *= Time.deltaTime;
        landingParticle = transform.Find("landingParticle").GetComponent<ParticleSystem>();
        destroyParticlePos = transform.Find("destroyParticlePos");
    }

    // Update is called once per frame
    private void Update()
    {
    }

    void FixedUpdate () {
		if (!reachTarget)
        {
            Falling();
        }
	}

    void Falling()
    {
        curPos = transform.position;
        curFallingSpeed += fallingSpeed;
        if (curPos.y - targetPos.y <= curFallingSpeed)
        {
            transform.position = targetPos;
            Reach();
        }
        else
        {
            curPos.y -= curFallingSpeed;
            transform.position = curPos;
        }
    }

    void Reach()
    {
        reachTarget = true;
        landingParticle.Play();
        Smash();
        cageCtrl.Register(volume*transform.localScale.x);
        Debug.Log("pushing");
        if (loaded)
        {
            cageCtrl.SetUpTarget(transform);
        }
        else
        {
            GameObject.Find("MouseCtrl").GetComponent<mouseCtrl>().RestartAimming(transform);
        }
    }

    void Smash()
    {
        Collider[] smashTargets = Physics.OverlapSphere(transform.position, transform.localScale.x * smashRange, smashTargetLayer);
        if (smashTargets.Length > 0)
        {
            float closestDis = float.MaxValue, curDis, closetIndex = -1;
            for (int i = 0; i < smashTargets.Length; i++)
            {
                curDis = Vector3.Distance(transform.position, smashTargets[i].transform.position);
                if (curDis < closestDis)
                {
                    closetIndex = i;
                    closestDis = curDis;
                }
            }
            for (int j = 0; j < smashTargets.Length; j++)
            {
                if (closetIndex == j)
                {
                    loadedHuman = smashTargets[j].transform;
                    loadedHuman.GetComponent<humanCtrl>().Loaded(transform);
                    loaded = true;
                }
                else
                {
                    smashTargets[j].GetComponent<humanCtrl>().Death();
                }
            }
        }
    }

    public void Fire(float power)
    {
        loadedHuman.GetComponent<humanCtrl>().Fire(power);
        loadedHuman.parent = null;
        SelfDestroy();
    }

    public void SetUpTargetPos(Vector3 targetPos)
    {
        this.targetPos = targetPos;
        reachTarget = false;
    }

    public void SelfDestroy()
    {
        cageCtrl.Dismiss(volume);
        Instantiate(destroyParticle, destroyParticlePos.position, destroyParticlePos.rotation);
        Collider[] smashTargets = Physics.OverlapSphere(transform.position, transform.localScale.x * smashRange, smashTargetLayer);
        foreach (Collider col in smashTargets)
        {
            col.transform.GetComponent<humanCtrl>().Death();
        }
        Destroy(gameObject);
    }
}
