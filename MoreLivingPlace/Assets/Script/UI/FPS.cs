using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class FPS : MonoBehaviour {

    private string FPSText;
    private TextMeshProUGUI textCtrl;

	// Use this for initialization
	void Awake () {
        textCtrl = GetComponent<TextMeshProUGUI>();
        FPSText = textCtrl.text;
	}
	
	// Update is called once per frame
	void Update () {
        textCtrl.text = FPSText + Mathf.Round(1 / Time.deltaTime);
	}
}
