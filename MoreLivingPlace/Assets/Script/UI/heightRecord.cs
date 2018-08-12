using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class heightRecord : MonoBehaviour {

    private TextMeshProUGUI curHeightText, highestHeightText;

	// Use this for initialization
	void Awake () {
        curHeightText = transform.Find("curHeightText").GetComponent<TextMeshProUGUI>();
        highestHeightText = transform.Find("highestHeightText").GetComponent<TextMeshProUGUI>();
    }
	
	// Update is called once per frame
	void Update () {
        curHeightText.text = "Height: " + BackgroundSetting.curHeight + "m";
        highestHeightText.text = "(Highest: " + BackgroundSetting.highestHeight + "m)";
    }
}
