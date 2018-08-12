using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class fuelTankHuman : humanCtrl {

    public ParticleSystem[] fuels = new ParticleSystem[2];

    new void Awake()
    {
        base.Awake();
        fuels[0] = transform.Find("model/fuelTank/leftFuelTank/mouth/fuel").GetComponent<ParticleSystem>();
        fuels[1] = transform.Find("model/fuelTank/rightFuelTank/mouth/fuel").GetComponent<ParticleSystem>();
    }

    //// Use this for initialization
    //void Start () {

    //}

    //// Update is called once per frame
    //void Update () {

    //}

    public override void Loaded(Transform shooter)
    {
        base.Loaded(shooter);
        ReleaseFuel();
    }

    public void ReleaseFuel()
    {
        fuels[0].Play();
        fuels[1].Play();
    }

    override public void InitMat(Material skin, Material clothing, Material pant, Material shoe)
    {
        base.InitMat(skin, clothing, pant, shoe);
        transform.Find("model/head/hatBase").GetComponent<Renderer>().material = shoe;
        transform.Find("model/head/hatTop/hatTop").GetComponent<Renderer>().material = shoe;
    }

}
