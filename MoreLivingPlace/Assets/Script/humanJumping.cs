using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class humanJumping : MonoBehaviour {

    public Material[] clothing;

    public float volume = 1f;
    public LayerMask groundLayer;
    public float height = 2.7f;
    public Vector3 multiMovementDir = new Vector3(150f, 500f, 150f);
    public float minMoveInterval = 1f, maxMoveInterval = 2f;

    private Rigidbody bodyRb;
    private Vector3 targetDir;
    private float nextMoveTime;
    private bool isGround;

    // Use this for initialization
    public void Awake()
    {
        transform.Find("model/body").GetComponent<Renderer>().material = clothing[Random.Range(0, clothing.Length)];
        Material pantMat = clothing[Random.Range(0, clothing.Length)];
        transform.Find("model/leftLeg").GetComponent<Renderer>().material = pantMat;
        transform.Find("model/rightLeg").GetComponent<Renderer>().material = pantMat;
        bodyRb = GetComponent<Rigidbody>();
        nextMoveTime = Time.time + Random.Range(minMoveInterval, maxMoveInterval);
    }

    void Update()
    {
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
        targetDir.y = 0;
        transform.rotation = Quaternion.LookRotation(targetDir);
    }

    void GroundCheck()
    {
        isGround = Physics.Raycast(transform.position, -transform.up, height * 1.1f, groundLayer);
    }
}
