using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonCtrl : MonoBehaviour {

    private cageCtrl humanSpawnScript;
    private Button addHumanBut;

	// Use this for initialization
	void Awake () {
        humanSpawnScript = GameObject.Find("Cage").GetComponent<cageCtrl>();
        addHumanBut = transform.Find("mainCameraUI/addHuman").GetComponent<Button>();
	}
	
	// Update is called once per frame
	void Update () {
        addHumanBut.interactable = cageCtrl.ableToSpawn;
    }

    public void AddHuman()
    {
        humanSpawnScript.HumanSpawn();
    }
}
