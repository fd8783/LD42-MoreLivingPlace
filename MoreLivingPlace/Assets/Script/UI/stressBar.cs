using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class stressBar : MonoBehaviour {

    public float minIndicatorMoving = -90f, maxIndicatorMoving = 90f;
    private float indicatorMovingRange;
    private Image barImg, caseImg;
    private RectTransform indicator;
    private Vector3 curIndicatorPos;
    private float curShowingStressLevel;

	// Use this for initialization
	void Awake () {
        barImg = transform.Find("bar").GetComponent<Image>();
        caseImg = transform.Find("case").GetComponent<Image>();
        indicator = transform.Find("indicator").GetComponent<RectTransform>();
        indicatorMovingRange = maxIndicatorMoving - minIndicatorMoving;
    }
	
	// Update is called once per frame
	void Update () {
        curShowingStressLevel = Mathf.Min(cageCtrl.stressLevel / 1.45f, 1f);
        barImg.fillAmount = curShowingStressLevel;
        curIndicatorPos = indicator.anchoredPosition;
        curIndicatorPos.y = minIndicatorMoving + (curShowingStressLevel * indicatorMovingRange);
        indicator.anchoredPosition = curIndicatorPos;
    }
}
