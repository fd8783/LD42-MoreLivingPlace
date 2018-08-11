using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class mouseCtrl : MonoBehaviour {

    public LayerMask groundLayer;
    private Transform targetTracer;

    private cageCtrl cageScript;
    
	// Use this for initialization
	void Awake () {
        targetTracer = transform.Find("targetTracer");
        cageScript = GameObject.Find("Cage").GetComponent<cageCtrl>();
	}
	
	// Update is called once per frame
	void Update () {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, 100, groundLayer))
        {
            if (cageScript.curWallLength - Mathf.Max(Mathf.Abs(hit.point.x),Mathf.Abs(hit.point.z)) > cageScript.curWallThick)
            {
                Debug.Log(hit.point);
                Debug.DrawRay(hit.point, transform.up * 100, Color.green);
            }
        }
    }
}
