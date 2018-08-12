using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TipsCtrl : MonoBehaviour {

    private GameObject[] Tips = new GameObject[4];

	// Use this for initialization
	void Awake () {
        Tips[0] = transform.Find("Tips1").gameObject;
        Tips[1] = transform.Find("Tips2").gameObject;
        Tips[2] = transform.Find("Tips3").gameObject;
        Tips[3] = transform.Find("Tips4").gameObject;
    }

    public void HideTips(int num)
    {
        Tips[num].SetActive(false);
    }

    public void ShowTips(int num)
    {
        Tips[num].SetActive(true);
    }
}
