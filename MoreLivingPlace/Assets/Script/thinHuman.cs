using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class thinHuman : humanCtrl {

    override public void InitMat(Material skin, Material clothing, Material pant, Material shoe)
    {
        base.InitMat(skin, clothing, pant, shoe);
        transform.Find("model/head/hairBase").GetComponent<Renderer>().material = shoe;
        transform.Find("model/head/hairTop").GetComponent<Renderer>().material = shoe;
    }
}
