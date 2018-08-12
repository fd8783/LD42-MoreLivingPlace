using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//*******not using now
public class humanSpawning : MonoBehaviour {

    public Transform spawnPt;
    private Vector3 spawnPos;

    public Material[] skin, clothing, shoe;
    private int skinCount, clothingCount, shoeCount;

    private Transform normalHuamn;
    private Transform ground, wallUp, wallDown, wallRight, wallLeft;

    // Use this for initialization
    void Awake () {
        normalHuamn = Resources.Load<Transform>("Human/NormalHuman");
        skinCount = skin.Length;
        clothingCount = clothing.Length;
        shoeCount = shoe.Length;

        ground = transform.Find("ground");
        wallUp = transform.Find("wallUp");
        wallDown = transform.Find("wallDown");
        wallRight = transform.Find("wallRight");
        wallLeft = transform.Find("wallLeft");
        spawnPos.y = wallUp.localPosition.y + 3f;
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public void HumanSpawn()
    {
        Transform human = Instantiate(normalHuamn, spawnPt.position, Quaternion.identity);
        human.GetComponent<humanCtrl>().InitMat(skin[Random.Range(0, skinCount)],
                                                clothing[Random.Range(0, clothingCount)],
                                                clothing[Random.Range(0, clothingCount)],
                                                shoe[Random.Range(0, shoeCount)]);
        Vector3 forceDir = human.forward;
        forceDir.y = Random.Range(0.1f, 0.3f);
        //human.GetComponent<Rigidbody>().AddForce(10000f * forceDir);
    }
}
