using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BackgroundSetting : MonoBehaviour
{
    public static BackgroundSetting Instance;
    public static GameObject human;

    public static bool isFirstTime = true;
    public static bool[] firstTips = { true, true, true, true };
    public static bool gameEnded = false;

    public static int curHeight = 0, highestHeight = 0;
    public static float fuelTankPower = 1.2f;

    private AudioSource grab;

    //StepStone
    //1. addin Thin Human 80m
    //2. scale up Ground
    //3. addin FuelTankHuman
    //4. scale up Ground
    public static int curStep = 0;
    public static int[] stepStoneTarget = { 80, 160, 800, 3000, 11000, int.MaxValue };


    // Use this for initialization
    void Awake()
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
        grab = GetComponent<AudioSource>();
    }

    //private void Start()
    //{
    //    if (SceneManager.GetActiveScene().buildIndex == 0 && gameEnded)
    //        GameObject.Find("Main Camera").GetComponent<MenuCtrl>().Switch(gameEnded);
    //}

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
            grab.Play();
        if (Input.GetKeyDown(KeyCode.Escape) && SceneManager.GetActiveScene().buildIndex != 0)
            SceneManager.LoadScene(0);
    }

    public static void CheckStep()
    {
        while (highestHeight > stepStoneTarget[curStep])
        {
            switch (curStep)
            {
                case 0:
                    GameObject.Find("Cage").GetComponent<cageCtrl>().EnableThinSpawn();
                    if (firstTips[0])
                    {
                        GameObject.Find("ScreenUI").GetComponent<TipsCtrl>().ShowTips(0);
                        firstTips[0] = false;
                    }
                    break;
                case 1:
                    GameObject.Find("Cage").GetComponent<cageCtrl>().ScaleGroundSize(1.5f);
                    if (firstTips[1])
                    {
                        GameObject.Find("ScreenUI").GetComponent<TipsCtrl>().ShowTips(1);
                        firstTips[1] = false;
                    }
                    break;
                case 2:
                    GameObject.Find("Cage").GetComponent<cageCtrl>().EnableFuelTankSpawn();
                    if (firstTips[2])
                    {
                        GameObject.Find("ScreenUI").GetComponent<TipsCtrl>().ShowTips(2);
                        firstTips[2] = false;
                    }
                    break;
                case 3:
                    GameObject.Find("Cage").GetComponent<cageCtrl>().ScaleGroundSize(1.5f);
                    if (firstTips[3])
                    {
                        GameObject.Find("ScreenUI").GetComponent<TipsCtrl>().ShowTips(3);
                        firstTips[3] = false;
                    }
                    break;
                case 4:
                    //end
                    break;
                default:
                    break;
            }
            curStep++;
        }
    }

    public static void EndTheGame(GameObject arrievdHuman)
    {
        human = arrievdHuman;
        gameEnded = true;
        SceneManager.LoadScene(3);
    }

    public static void SetCurHeight(int height)
    {
        curHeight = height;
        if (curHeight > highestHeight)
        {
            highestHeight = curHeight;
        }
    }

    public static void SwitchScene(int sceneNum)
    {
        SceneManager.LoadScene(sceneNum);
    }


}
