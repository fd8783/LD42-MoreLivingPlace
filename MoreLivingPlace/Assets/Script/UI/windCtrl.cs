using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class windCtrl : MonoBehaviour {

    public Sprite[] windImg;

    public float MaxHeight = 5000f, intervel = 100f;
    public float MinScale = 4f, MaxScale = 10f;
    public float MinX = -8f, MaxX = 8f;

    private Vector3 curPos, tempScale, tempPos;

	// Use this for initialization
	void Awake () {
        curPos = transform.position;
		while(curPos.y < MaxHeight)
        {
            curPos.y += Random.Range(0.8f, 1.2f) * intervel;
            GameObject wind = new GameObject("wind");
            wind.transform.parent = transform;
            wind.AddComponent<SpriteRenderer>().sprite = windImg[Random.Range(0,windImg.Length)];
            tempPos = curPos;
            tempPos.x += Random.Range(MinX, MaxX);
            tempScale = Vector3.one * Random.Range(MinScale, MaxScale);
            wind.transform.position = tempPos;
            wind.transform.localScale = tempScale;
        }
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
