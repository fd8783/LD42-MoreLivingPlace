using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cageCtrl : MonoBehaviour {
    private static int curHumanCount = 0;

    private Transform ground, wallUp, wallDown, wallRight, wallLeft;
    private Rigidbody wallUpRb, wallDownRb, wallRightRb, wallLeftRb;
    public Vector3 wallUpPos, wallDownPos, wallRightPos, wallLeftPos;
    public Vector3 wallUpStartPos, wallDownStartPos, wallRightStartPos, wallLeftStartPos;
    public Vector3 smoothWallUpMove, smoothWallDownMove, smoothWallRightMove, smoothWallLeftMove;
    public float wallMovingSmooth = 0.05f, wallMoveingSmoothDamp = 0.1f;
    private bool readyToPush = false;
    private Vector3 targetPos;

    public float curGroundSize, curWallLength, curWallThick = 0.5f, pushPower = 1f;
    private float curStress = 2f;

    private humanSpawning humanSpawnScript;
    public float humanSpawnInterval = 1f;

	// Use this for initialization
	void Awake () {
        ground = transform.Find("ground");
        wallUp = transform.Find("wallUp");
        wallDown = transform.Find("wallDown");
        wallRight = transform.Find("wallRight");
        wallLeft = transform.Find("wallLeft");
        wallUpRb = wallUp.GetComponent<Rigidbody>();
        wallDownRb = wallDown.GetComponent<Rigidbody>();
        wallRightRb = wallRight.GetComponent<Rigidbody>();
        wallLeftRb = wallLeft.GetComponent<Rigidbody>();
        wallUpPos = wallUp.localPosition;
        wallDownPos = wallDown.localPosition;
        wallRightPos = wallRight.localPosition;
        wallLeftPos = wallLeft.localPosition;
        wallUpStartPos = wallUp.localPosition;
        wallDownStartPos = wallDown.localPosition;
        wallRightStartPos = wallRight.localPosition;
        wallLeftStartPos = wallLeft.localPosition;
        curGroundSize = (int)ground.transform.localScale.x;
        curWallLength = curGroundSize/2 - curWallThick;
        SetUpTarget(new Vector3(0,0,0));
        humanSpawnScript = GetComponent<humanSpawning>();
        StartCoroutine("humanSpawningLoop");
    }
	
	// Update is called once per frame
	void Update ()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Pushing();
        }
        PushingBack();
        WallMovement();
    }

    void WallMovement()
    {
        if (Vector3.Distance(wallUp.localPosition,wallUpPos) > wallMovingSmooth)
            wallUp.localPosition = Vector3.SmoothDamp(wallUp.localPosition, wallUpPos, ref smoothWallUpMove, wallMoveingSmoothDamp);
        if (Vector3.Distance(wallDown.localPosition, wallDownPos) > wallMovingSmooth)
            wallDown.localPosition = Vector3.SmoothDamp(wallDown.localPosition, wallDownPos, ref smoothWallDownMove, wallMoveingSmoothDamp);
        if (Vector3.Distance(wallRight.localPosition, wallRightPos) > wallMovingSmooth)
            wallRight.localPosition = Vector3.SmoothDamp(wallRight.localPosition, wallRightPos, ref smoothWallRightMove, wallMoveingSmoothDamp);
        if (Vector3.Distance(wallLeft.localPosition, wallLeftPos) > wallMovingSmooth)
            wallLeft.localPosition = Vector3.SmoothDamp(wallLeft.localPosition, wallLeftPos, ref smoothWallLeftMove, wallMoveingSmoothDamp);
    }

    public void Pushing()
    {
        if (readyToPush)
        {
            wallUpPos = wallUp.localPosition;
            wallDownPos = wallDown.localPosition;
            wallRightPos = wallRight.localPosition;
            wallLeftPos = wallLeft.localPosition;
            
            if ((wallUpPos.x + curWallThick) > (targetPos.x + curWallThick*3))
                wallUpPos.x = Mathf.Max(wallUpPos.x - pushPower, (targetPos.x + curWallThick * 3 - curWallThick));
            if (wallUpPos.z > (targetPos.z + curWallThick*3))
                wallUpPos.z = Mathf.Max(wallUpPos.z - pushPower, (targetPos.z + curWallThick * 3));

            if ((wallDownPos.x - curWallThick) < (targetPos.x - curWallThick*3))
                wallDownPos.x = Mathf.Min(wallDownPos.x + pushPower, (targetPos.x - curWallThick * 3 + curWallThick));
            if (wallDownPos.z < (targetPos.z - curWallThick * 3))
                wallDownPos.z = Mathf.Min(wallDownPos.z + pushPower, (targetPos.z - curWallThick * 3));

            if (wallRightPos.x > (targetPos.x + curWallThick * 3))
                wallRightPos.x = Mathf.Max(wallRightPos.x - pushPower, (targetPos.x + curWallThick * 3));
            if ((wallRightPos.z - curWallThick) < (targetPos.z - curWallThick * 3))
                wallRightPos.z = Mathf.Min(wallRightPos.z + pushPower, (targetPos.z - curWallThick * 3 + curWallThick));

            if (wallLeftPos.x < (targetPos.x - curWallThick * 3))
                wallLeftPos.x = Mathf.Min(wallLeftPos.x + pushPower, (targetPos.x - curWallThick * 3));
            if ((wallLeftPos.z + curWallThick) > (targetPos.z + curWallThick * 3))
                wallLeftPos.z = Mathf.Max(wallLeftPos.z - pushPower, (targetPos.z + curWallThick * 3 - curWallThick));
        }
    }

    public void PushingBack()
    {
        if (wallUpPos.x < wallUpStartPos.x)
            wallUpPos.x = Mathf.Min(wallUpPos.x + (pushPower * curStress) * Time.deltaTime, wallUpStartPos.x);
        if (wallUpPos.z < wallUpStartPos.z)
            wallUpPos.z = Mathf.Min(wallUpPos.z + (pushPower * curStress) * Time.deltaTime, wallUpStartPos.z);

        if (wallDownPos.x > wallDownStartPos.x)
            wallDownPos.x = Mathf.Max(wallDownPos.x - (pushPower * curStress) * Time.deltaTime, wallDownStartPos.x);
        if (wallDownPos.z > wallDownStartPos.z)
            wallDownPos.z = Mathf.Max(wallDownPos.z - (pushPower * curStress) * Time.deltaTime, wallDownStartPos.z);

        if (wallRightPos.x < wallRightStartPos.x)
            wallRightPos.x = Mathf.Min(wallRightPos.x + (pushPower * curStress) * Time.deltaTime, wallRightStartPos.x);
        if (wallRightPos.z > wallRightStartPos.z)
            wallRightPos.z = Mathf.Max(wallRightPos.z - (pushPower * curStress) * Time.deltaTime, wallRightStartPos.z);

        if (wallLeftPos.x > wallLeftStartPos.x)
            wallLeftPos.x = Mathf.Max(wallLeftPos.x - (pushPower * curStress) * Time.deltaTime, wallLeftStartPos.x);
        if (wallLeftPos.z < wallLeftStartPos.z)
            wallLeftPos.z = Mathf.Min(wallLeftPos.z + (pushPower * curStress) * Time.deltaTime, wallLeftStartPos.z);
    }

    //public void Pushing()
    //{
    //    if (readyToPush)
    //    {
    //        wallUpPos = wallUp.localPosition;
    //        wallDownPos = wallDown.position;
    //        wallRightPos = wallRight.position;
    //        wallLeftPos = wallLeft.position;

    //        Vector3 pushUp = Vector3.zero;
    //        if ((wallUpPos.x+0.5f) > (targetPos.x + wallMovingSmooth))
    //            pushUp.x = -pushPower * (1 - curStress);
    //        if (wallUpPos.z > (targetPos.z + wallMovingSmooth))
    //            pushUp.z = -pushPower * (1 - curStress);
    //        if (pushUp != Vector3.zero)
    //            wallUpRb.velocity = pushUp;

    //        Vector3 pushDown = Vector3.zero;
    //        if ((wallDownPos.x + 0.5f) < (targetPos.x + wallMovingSmooth))
    //            pushDown.x = pushPower * (1 - curStress);
    //        if (wallDownPos.z < (targetPos.z + wallMovingSmooth))
    //            pushDown.z = pushPower * (1 - curStress);
    //        if (pushDown != Vector3.zero)
    //            wallDownRb.velocity = pushDown;

    //        Vector3 pushRight = Vector3.zero;
    //        if (wallRightPos.x > (targetPos.x + wallMovingSmooth))
    //            pushRight.x = -pushPower * (1 - curStress);
    //        if ((wallRightPos.z + 0.5f) < (targetPos.z + wallMovingSmooth))
    //            pushRight.z = pushPower * (1 - curStress);
    //        if (pushRight != Vector3.zero)
    //            wallRightRb.velocity = pushRight;

    //        Vector3 pushLeft = Vector3.zero;
    //        if (wallLeftPos.x < (targetPos.x + wallMovingSmooth))
    //            pushLeft.x = pushPower * (1 - curStress);
    //        if ((wallLeftPos.z + 0.5f) > (targetPos.z + wallMovingSmooth))
    //            pushLeft.z = -pushPower * (1 - curStress);
    //        if (pushLeft != Vector3.zero)
    //            wallLeftRb.velocity = pushLeft;
    //    }
    //}

    public void UpdateGroundSize()
    {
        //...

        wallUpPos = wallUp.localPosition;
        wallDownPos = wallDown.localPosition;
        wallRightPos = wallRight.localPosition;
        wallLeftPos = wallLeft.localPosition;
        wallUpStartPos = wallUp.localPosition;
        wallDownStartPos = wallDown.localPosition;
        wallRightStartPos = wallRight.localPosition;
        wallLeftStartPos = wallLeft.localPosition;
    }

    public void SetUpTarget(Vector3 targetPos)
    {
        this.targetPos = targetPos;
        readyToPush = true;
    }

    public static void Register()
    {
        curHumanCount++;
    }

    public static void Dismiss()
    {
        curHumanCount--;
    }

    IEnumerator humanSpawningLoop()
    {
        yield return new WaitForSeconds(1f);
        while (true)
        {
            humanSpawnScript.HumanSpawn();
            yield return new WaitForSeconds(humanSpawnInterval);
        }
    }
}
