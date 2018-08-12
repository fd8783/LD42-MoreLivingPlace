using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class followAst : MonoBehaviour {

    private Camera cam;
    private float startSize;

    public float lowestY, curY;

    private Vector3 curPos, targetPos, smoothMove, startPos;
    private Quaternion startRot;

    private Transform astronaut; 

	// Use this for initialization
	void Awake () {
        lowestY = transform.position.y;
        startPos = transform.position;
        cam = transform.GetComponent<Camera>();
        startSize = cam.orthographicSize;
        startRot = transform.rotation;
    }
	
	// Update is called once per frame
	void Update () {
        //if (astronaut != null)
        //{
        //    targetPos = astronaut.position;
        //    if (targetPos.y < lowestY)
        //        targetPos.y = lowestY;
        //}
        //MoveCamera();
        cam.orthographicSize = 10f + Mathf.Log(BackgroundSetting.curHeight+1) * 2;
	}

    //void MoveCamera()
    //{
    //    curPos = transform.position;
    //    transform.position = Vector3.SmoothDamp(curPos, targetPos, ref smoothMove, 0.1f);
    //}

    public void Register(Transform astronaut)
    {
        //this.astronaut = astronaut;
        transform.parent = astronaut;
        curPos = transform.localPosition;
        curPos.x = 0;
        transform.localPosition = curPos;
    }

    public void Dismiss()
    {
        //if (astronaut != null)
        //{
        //    astronaut = null;
        //}
        transform.parent = null;
        transform.position = startPos;
        transform.rotation = startRot;
    }
}
