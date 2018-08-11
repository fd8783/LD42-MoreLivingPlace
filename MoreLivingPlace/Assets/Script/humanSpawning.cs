using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class humanSpawning : MonoBehaviour {

    public Transform spawnPt;

    public Material[] skin, clothing, shoe;
    private int skinCount, clothingCount, shoeCount;

    private Transform normalHuamn;

	// Use this for initialization
	void Awake () {
        normalHuamn = Resources.Load<Transform>("Human/NormalHuman");
        skinCount = skin.Length;
        clothingCount = clothing.Length;
        shoeCount = shoe.Length;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void HumanSpawn()
    {
        Transform human = Instantiate(normalHuamn, spawnPt.position, Quaternion.identity);
        human.GetComponent<humanCtrl>().initMat(skin[Random.Range(0, skinCount)],
                                                clothing[Random.Range(0, clothingCount)],
                                                clothing[Random.Range(0, clothingCount)],
                                                shoe[Random.Range(0, shoeCount)]);
    }
}
