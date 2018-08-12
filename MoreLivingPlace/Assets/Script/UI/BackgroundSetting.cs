using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundSetting : MonoBehaviour
{
    public static BackgroundSetting Instance;

    public static int curHeight= 0, highestHeight = 0;
    public static float fuelTankPower = 1.2f;

    //StepStone
    //1. addin Thin Human 80m
    //2. scale up Ground
    //3. addin FuelTankHuman
    //4. scale up Ground
    public static int curStep = 0;
    public static int[] stepStoneTarget = { 80, 200, 10000, 11000, int.MaxValue};


    // Use this for initialization
    void Awake ()
    {
        if (Instance == null)
        {
            Debug.Log("instance=this");
            Instance = this;
        }
        else if (Instance != this)
        {
            Debug.Log("Destroy(gameObject)");
            Destroy(gameObject);
        }
        DontDestroyOnLoad(this);
	}
	
	// Update is called once per frame
	void Update ()
    {
    }

    public static void CheckStep()
    {
        while (highestHeight > stepStoneTarget[curStep])
        {
            switch (curStep)
            {
                case 0:
                    GameObject.Find("Cage").GetComponent<cageCtrl>().EnableThinSpawn();
                    break;
                case 1:
                    GameObject.Find("Cage").GetComponent<cageCtrl>().ScaleGroundSize(1.5f);
                    break;
                case 2:
                    GameObject.Find("Cage").GetComponent<cageCtrl>().EnableFuelTankSpawn();
                    break;
                case 3:
                    GameObject.Find("Cage").GetComponent<cageCtrl>().ScaleGroundSize(1.5f);
                    break;
                default:
                    break;
            }
            curStep++;
        }
    }

    public static void SetCurHeight(int height)
    {
        curHeight = height;
        if (curHeight > highestHeight)
        {
            highestHeight = curHeight;
        }
    }
}
