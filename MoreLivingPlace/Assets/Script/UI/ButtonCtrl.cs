using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonCtrl : MonoBehaviour {

    private cageCtrl humanSpawnScript;
    private Button addHumanBut;

    private cageCtrl cageScript;
    // Use this for initialization
    void Awake () {
        humanSpawnScript = GameObject.Find("Cage").GetComponent<cageCtrl>();
        addHumanBut = transform.Find("mainCameraUI/addHuman").GetComponent<Button>();
        cageScript = GameObject.Find("Cage").GetComponent<cageCtrl>();
    }
	
	// Update is called once per frame
	void Update () {
        addHumanBut.interactable = cageScript.ableToSpawn;
    }

    public void AddHuman()
    {
        humanSpawnScript.HumanSpawn();
    }
}
