using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonCtrl : MonoBehaviour {

    private humanSpawning humanSpawnScript;

	// Use this for initialization
	void Awake () {
        humanSpawnScript = GameObject.Find("Cage").GetComponent<humanSpawning>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void AddHuman()
    {
        humanSpawnScript.HumanSpawn();
    }
}
