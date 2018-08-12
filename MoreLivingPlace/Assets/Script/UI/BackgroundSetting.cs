using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundSetting : MonoBehaviour {

    public static int curHeight= 0, highestHeight = 0;

    // Use this for initialization
    void Start () {
        DontDestroyOnLoad(this);
	}
	
	// Update is called once per frame
	void Update ()
    {
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
