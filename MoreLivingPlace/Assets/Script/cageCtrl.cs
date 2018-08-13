using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cageCtrl : MonoBehaviour {
    private int curHumanCount = 0;
    public float curMaxVolume, curVolume = 0, stressLevel;
    public bool ableToSpawn = true;
    public bool readyToPush = false;
    public bool ableToAddThin = false, ableToAddFuelTank = false;
    private Vector3 targetPos;
    private Transform curShooter = null;

    public float preStress = 0, lastPushTime, releaseFireTime = 0.2f;
    private bool readyToFire = false, pushedThisFrame =false;
    private mouseCtrl mouseScript;

    private Transform ground, wallUp, wallDown, wallRight, wallLeft;
    private Rigidbody wallUpRb, wallDownRb, wallRightRb, wallLeftRb;
    public Vector3 wallUpPos, wallDownPos, wallRightPos, wallLeftPos;
    public Vector3 wallUpStartPos, wallDownStartPos, wallRightStartPos, wallLeftStartPos;
    public Vector3 smoothWallUpMove, smoothWallDownMove, smoothWallRightMove, smoothWallLeftMove;
    public float wallMovingSmooth = 0.05f, wallMoveingSmoothDamp = 0.1f;

    public float curGroundSize, curWallLength, curWallThick = 0.5f, pushPower = 1f;
    private float startStress = 2.1f, curStress = 2.1f;
    private float curGroundXSpace, curGroundZSpace;

    //human spawning
    public Transform spawnPt;
    private Vector3 spawnPos;

    public Material[] skin, clothing, shoe;
    private int skinCount, clothingCount, shoeCount;

    private Transform normalHuamn, thinHuman, fuelTankHuman;
    //private humanSpawning humanSpawnScript;
    public float humanSpawnInterval = 0.3f;

    private AudioSource audioS;

    public bool isWallRecovered = true;

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
        //SetUpTarget(new Vector3(0,0,0));
        mouseScript = GameObject.Find("MouseCtrl").GetComponent<mouseCtrl>();
        normalHuamn = Resources.Load<Transform>("Human/NormalHuman");
        thinHuman = Resources.Load<Transform>("Human/ThinHuman");
        fuelTankHuman = Resources.Load<Transform>("Human/FuelTankHuman");
        skinCount = skin.Length;
        clothingCount = clothing.Length;
        shoeCount = shoe.Length;
        spawnPos.y = wallUp.localPosition.y + 3f;
        //humanSpawnScript = GetComponent<humanSpawning>();
        StartCoroutine("humanSpawningLoop");
        BackgroundSetting.curStep = 0;
        BackgroundSetting.CheckStep();
        audioS = GetComponent<AudioSource>();
    }
	
	// Update is called once per frame
	void Update ()
    {
        if (readyToPush)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                audioS.Play();
                Pushing();
                pushedThisFrame = true;
                lastPushTime = Time.time;
            }
            else if (readyToFire)
            {
                if (Time.time > lastPushTime + releaseFireTime)
                {
                    mouseScript.Fired();
                    Debug.Log("fire power: " + Mathf.Pow(curVolume, preStress * 0.9f) + " c&p" + curVolume + ":" + preStress * 0.9f);
                    curShooter.GetComponent<shooterCtrl>().Fire(Mathf.Pow(curVolume, preStress*0.9f));
                    screenShake.shakecoefficient = 1f * preStress;
                    screenShake.StopScreen(0.07f);
                    Debug.Log("fire power: " + Mathf.Pow(curVolume, preStress * 0.9f) + " c&p" + curVolume + ":" + preStress * 0.9f);
                    //curShooter.GetComponent<shooterCtrl>().Fire(curVolume* preStress);
                    //Debug.Log("fire power: "+ curVolume * preStress +" c&p" +curVolume+":"+preStress);
                    readyToFire = false;
                    readyToPush = false;
                }
            }
        }
        PushingBack();
        WallMovement();

        StressCalculation();
        if (pushedThisFrame)
        {
            preStress = stressLevel;
            pushedThisFrame = false;
        }
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

    void StressCalculation()
    {
        curGroundXSpace = Mathf.Abs(wallUp.localPosition.z - wallDown.localPosition.z - curWallThick * 2);
        curGroundZSpace = Mathf.Abs(wallRight.localPosition.x - wallLeft.localPosition.x - curWallThick * 2);
        curMaxVolume = curGroundXSpace * curGroundZSpace;

        stressLevel = curVolume / curMaxVolume;
        stressLevel = Mathf.Min(stressLevel, 1.6f);
        //Debug.Log(stressLevel);
        curStress = (stressLevel >= 0.8f) ? startStress * (1 + Mathf.Min(stressLevel - 0.8f, 1f) * 8) : startStress;

        if (!readyToFire && (stressLevel > 1.2f))
            readyToFire = true;

        ableToSpawn = (curVolume < curMaxVolume * 1f);
    }

    public void Pushing()
    {
        wallUpPos = wallUp.localPosition;
        wallDownPos = wallDown.localPosition;
        wallRightPos = wallRight.localPosition;
        wallLeftPos = wallLeft.localPosition;

        if ((wallUpPos.x + curWallThick) > (targetPos.x + curWallThick * 3))
            wallUpPos.x = Mathf.Max(wallUpPos.x - pushPower, (targetPos.x + curWallThick * 4 - curWallThick));
        if (wallUpPos.z > (targetPos.z + curWallThick * 3))
            wallUpPos.z = Mathf.Max(wallUpPos.z - pushPower, (targetPos.z + curWallThick * 4));

        if ((wallDownPos.x - curWallThick) < (targetPos.x - curWallThick * 3))
            wallDownPos.x = Mathf.Min(wallDownPos.x + pushPower, (targetPos.x - curWallThick * 4 + curWallThick));
        if (wallDownPos.z < (targetPos.z - curWallThick * 4))
            wallDownPos.z = Mathf.Min(wallDownPos.z + pushPower, (targetPos.z - curWallThick * 4));

        if (wallRightPos.x > (targetPos.x + curWallThick * 4))
            wallRightPos.x = Mathf.Max(wallRightPos.x - pushPower, (targetPos.x + curWallThick * 4));
        if ((wallRightPos.z - curWallThick) < (targetPos.z - curWallThick * 4))
            wallRightPos.z = Mathf.Min(wallRightPos.z + pushPower, (targetPos.z - curWallThick * 4 + curWallThick));

        if (wallLeftPos.x < (targetPos.x - curWallThick * 4))
            wallLeftPos.x = Mathf.Min(wallLeftPos.x + pushPower, (targetPos.x - curWallThick * 4));
        if ((wallLeftPos.z + curWallThick) > (targetPos.z + curWallThick * 4))
            wallLeftPos.z = Mathf.Max(wallLeftPos.z - pushPower, (targetPos.z + curWallThick * 4 - curWallThick));
    }

    public void PushingBack()
    {
        int backCount = 0;
        if (wallUpPos.x < wallUpStartPos.x)
            wallUpPos.x = Mathf.Min(wallUpPos.x + (pushPower * curStress) * Time.deltaTime, wallUpStartPos.x);
        else
            backCount++;
        if (wallUpPos.z < wallUpStartPos.z)
            wallUpPos.z = Mathf.Min(wallUpPos.z + (pushPower * curStress) * Time.deltaTime, wallUpStartPos.z);
        else
            backCount++;

        if (wallDownPos.x > wallDownStartPos.x)
            wallDownPos.x = Mathf.Max(wallDownPos.x - (pushPower * curStress) * Time.deltaTime, wallDownStartPos.x);
        else
            backCount++;
        if (wallDownPos.z > wallDownStartPos.z)
            wallDownPos.z = Mathf.Max(wallDownPos.z - (pushPower * curStress) * Time.deltaTime, wallDownStartPos.z);
        else
            backCount++;

        if (wallRightPos.x < wallRightStartPos.x)
            wallRightPos.x = Mathf.Min(wallRightPos.x + (pushPower * curStress) * Time.deltaTime, wallRightStartPos.x);
        else
            backCount++;
        if (wallRightPos.z > wallRightStartPos.z)
            wallRightPos.z = Mathf.Max(wallRightPos.z - (pushPower * curStress) * Time.deltaTime, wallRightStartPos.z);
        else
            backCount++;

        if (wallLeftPos.x > wallLeftStartPos.x)
            wallLeftPos.x = Mathf.Max(wallLeftPos.x - (pushPower * curStress) * Time.deltaTime, wallLeftStartPos.x);
        else
            backCount++;
        if (wallLeftPos.z < wallLeftStartPos.z)
            wallLeftPos.z = Mathf.Min(wallLeftPos.z + (pushPower * curStress) * Time.deltaTime, wallLeftStartPos.z);
        else
            backCount++;

        isWallRecovered = (backCount == 8);
    }

    //public void UpdateGroundSize()
    //{
    //    //...

    //    wallUpPos = wallUp.localPosition;
    //    wallDownPos = wallDown.localPosition;
    //    wallRightPos = wallRight.localPosition;
    //    wallLeftPos = wallLeft.localPosition;
    //    wallUpStartPos = wallUp.localPosition;
    //    wallDownStartPos = wallDown.localPosition;
    //    wallRightStartPos = wallRight.localPosition;
    //    wallLeftStartPos = wallLeft.localPosition;
    //}

    public void ScaleGroundSize(float scaleValue)
    {
        Vector3 newGroundScale = ground.localScale;
        newGroundScale.x = ((newGroundScale.x - 0.01f) * scaleValue) + 0.01f;
        newGroundScale.z = ((newGroundScale.z - 0.01f) * scaleValue) + 0.01f;
        ground.localScale = newGroundScale;

        Vector3 newWallUpPos = wallUp.localPosition, newWallUpScale = wallUp.localScale;
        newWallUpPos.x *= scaleValue;
        newWallUpPos.z *= scaleValue;
        newWallUpScale.x *= scaleValue;
        newWallUpScale.y *= scaleValue;
        wallUp.localPosition = newWallUpPos;
        wallUp.localScale = newWallUpScale;

        Vector3 newWallDownPos = wallDown.localPosition, newWallDownScale = wallDown.localScale;
        newWallDownPos.x *= scaleValue;
        newWallDownPos.z *= scaleValue;
        newWallDownScale.x *= scaleValue;
        newWallDownScale.y *= scaleValue;
        wallDown.localPosition = newWallDownPos;
        wallDown.localScale = newWallDownScale;

        Vector3 newWallRightPos = wallRight.localPosition, newWallRightScale = wallRight.localScale;
        newWallRightPos.x *= scaleValue;
        newWallRightPos.z *= scaleValue;
        newWallRightScale.z *= scaleValue;
        newWallRightScale.y *= scaleValue;
        wallRight.localPosition = newWallRightPos;
        wallRight.localScale = newWallRightScale;

        Vector3 newWallLeftPos = wallLeft.localPosition, newWallLeftScale = wallLeft.localScale;
        newWallLeftPos.x *= scaleValue;
        newWallLeftPos.z *= scaleValue;
        newWallLeftScale.z *= scaleValue;
        newWallLeftScale.y *= scaleValue;
        wallLeft.localPosition = newWallLeftPos;
        wallLeft.localScale = newWallLeftScale;

        wallUpPos = wallUp.localPosition;
        wallDownPos = wallDown.localPosition;
        wallRightPos = wallRight.localPosition;
        wallLeftPos = wallLeft.localPosition;
        wallUpStartPos = wallUp.localPosition;
        wallDownStartPos = wallDown.localPosition;
        wallRightStartPos = wallRight.localPosition;
        wallLeftStartPos = wallLeft.localPosition;

        curGroundSize = (int)ground.localScale.x;
        curWallLength = curGroundSize / 2 - curWallThick;
    }

    public void EnableThinSpawn()
    {
        ableToAddThin = true;
    }

    public void EnableFuelTankSpawn()
    {
        ableToAddFuelTank = true;
    }

    public void SetUpTarget(Transform shooter)
    {
        targetPos = shooter.position;
        curShooter = shooter;
        readyToPush = true;
    }

    public void Register(float volume)
    {
        curVolume += volume;
        curHumanCount++;
    }

    public void Dismiss(float volume)
    {
        curVolume -= volume;
        curHumanCount--;
    }

    public void HumanSpawn()
    {
        spawnPos.x = wallRight.localPosition.x - 1f - Random.Range(0f, curGroundXSpace - 1f);
        spawnPos.z = wallUp.localPosition.z - 1f - Random.Range(0f, curGroundZSpace - 1f);
        spawnPt.localPosition = spawnPos;
        Transform human;

        float randomNum = Random.Range(0f, 1f);
        if (ableToAddThin && randomNum < 0.2f)
        {
            human = Instantiate(thinHuman, spawnPos, Quaternion.identity);
        }
        else if (ableToAddFuelTank && randomNum < 0.35f)
        {
            human = Instantiate(fuelTankHuman, spawnPos, Quaternion.identity);
        }
        else
        {
            human = Instantiate(normalHuamn, spawnPos, Quaternion.identity);
        }

        human.GetComponent<humanCtrl>().InitMat(skin[Random.Range(0, skinCount)],
                                                clothing[Random.Range(0, clothingCount)],
                                                clothing[Random.Range(0, clothingCount)],
                                                shoe[Random.Range(0, shoeCount)]);
    }

    IEnumerator humanSpawningLoop()
    {
        yield return new WaitForSeconds(1f);
        while (true)
        {
            if (ableToSpawn)
            {
                HumanSpawn();
            }
            yield return new WaitForSeconds(humanSpawnInterval);
        }
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

}
