using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class humanCtrl : MonoBehaviour {

    public LayerMask groundLayer;
    public float height = 2.7f;
    public Vector3 multiMovementDir = new Vector3(150f, 500f, 150f);
    public float minMoveInterval = 1f, maxMoveInterval = 2f;

    private Rigidbody bodyRb;
    private Vector3 targetDir;
    private float nextMoveTime;

    private bool isDeath = false, isGround = true;

    private Transform model;

    // Use this for initialization
    void Awake () {
        model = transform.Find("model");
        bodyRb = GetComponent<Rigidbody>();
        cageCtrl.Register();
        nextMoveTime = Time.time + Random.Range(minMoveInterval, maxMoveInterval);
	}
	
	// Update is called once per frame
	void Update () {
		if (Time.time > nextMoveTime)
        {
            GroundCheck();
            if (isGround)
                RandomMove();
            nextMoveTime = Time.time + Random.Range(minMoveInterval, maxMoveInterval);
        }
	}

    void RandomMove()
    {
        targetDir = Random.insideUnitCircle;
        targetDir.z = targetDir.y;
        targetDir.y = Random.Range(0.7f, 1f);
        bodyRb.AddForce(Vector3.Scale(targetDir, multiMovementDir));
    }

    void GroundCheck()
    {
        isGround = Physics.Raycast(transform.position, -transform.up, height * 1.1f, groundLayer);
    }

    void initClothing()
    {

    }

    public void Death()
    {
        isDeath = true;
        cageCtrl.Dismiss();
        //instant blood particle
        Destroy(gameObject);
    }

    public void initMat(Material skin, Material clothing, Material pant, Material shoe)
    {
        model.Find("head").GetComponent<Renderer>().material = skin;
        model.Find("leftArm/leftHand").GetComponent<Renderer>().material = skin;
        model.Find("rightArm/rightHand").GetComponent<Renderer>().material = skin;
        
        model.Find("body").GetComponent<Renderer>().material = clothing;
        model.Find("leftArm").GetComponent<Renderer>().material = clothing;
        model.Find("rightArm").GetComponent<Renderer>().material = clothing;

        model.Find("leftLeg").GetComponent<Renderer>().material = pant;
        model.Find("rightLeg").GetComponent<Renderer>().material = pant;

        model.Find("leftLeg/leftFoot").GetComponent<Renderer>().material = shoe;
        model.Find("rightLeg/rightFoot").GetComponent<Renderer>().material = shoe;
    }
}
