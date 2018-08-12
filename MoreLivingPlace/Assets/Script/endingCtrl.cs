using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class endingCtrl : MonoBehaviour {

    private Transform arrivedHuman;

	// Use this for initialization
	void Awake () {
        arrivedHuman = GameObject.Find("arrivedHuman").transform;
        arrivedHuman.parent = transform;
        arrivedHuman.localPosition = Vector3.zero;
        arrivedHuman.GetComponent<humanCtrl>().bloodParticle = Resources.Load<Transform>("bloodParticleNoGravity");
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void Death()
    {
        arrivedHuman.GetComponent<humanCtrl>().DeathWithoutDismiss();
    }
}
