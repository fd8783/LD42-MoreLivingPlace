using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MenuCtrl : MonoBehaviour {

    private GameObject startBut, continueBut;

	// Use this for initialization
	void Start () {
        startBut = transform.Find("mainCameraUI/Start").gameObject;
        continueBut = transform.Find("mainCameraUI/Continue").gameObject;
        if (BackgroundSetting.gameEnded)
            Switch(true);
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public void Switch(bool gameEnded)
    {
        startBut.SetActive(!gameEnded);
        continueBut.SetActive(gameEnded);
    }

    public void StartBut()
    {
        SceneManager.LoadScene(1);
    }

    public void ExitBut()
    {
        Application.Quit();
    }
}
