using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class killEveryThing : MonoBehaviour {

    int playerLayerNum;

    private void Awake()
    {
        playerLayerNum = LayerMask.NameToLayer("Player");
    }

    private void OnTriggerEnter(Collider other)
    {
        //Debug.Log(other.gameObject.layer +" f "+ playerLayerNum);
        if (other.gameObject.layer == playerLayerNum)
        {
            other.GetComponent<humanCtrl>().Death();
        }
        else
        {
            Destroy(other);
        }
    }

}
