using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class mouseCtrl : MonoBehaviour {

    public LayerMask groundLayer;
    private GameObject targetTracer;
    //private Projector aimProjector;

    private cageCtrl cageScript;
    private Transform shooterPool;
    private Transform shooter, lastShooter;
    
    private bool readyToFire = true;
    
	// Use this for initialization
	void Awake () {
        targetTracer = transform.Find("targetTracer").gameObject;
        targetTracer.SetActive(false);
        //aimProjector = targetTracer.GetComponent<Projector>();
        //aimProjector.enabled = false;
        cageScript = GameObject.Find("Cage").GetComponent<cageCtrl>();
        shooter = Resources.Load<Transform>("shooter");
	}
	
	// Update is called once per frame
	void Update () {
        if (readyToFire && cageScript.isWallRecovered) 
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, 100, groundLayer))
            {
                transform.position = hit.point;
                if (cageScript.curWallLength - Mathf.Max(Mathf.Abs(hit.point.x), Mathf.Abs(hit.point.z)) > cageScript.curWallThick)
                {
                    //Debug.Log(hit.point);
                    //Debug.DrawRay(hit.point, transform.up * 100, Color.green);
                    //aimProjector.enabled = true;
                    targetTracer.SetActive(true);

                    if (Input.GetMouseButtonDown(0))
                    {
                        if (lastShooter != null)
                        {
                            lastShooter.GetComponent<shooterCtrl>().SelfDestroy();
                            lastShooter = null;
                        }
                        Transform shooterInstant = Instantiate(shooter, new Vector3(hit.point.x, hit.point.y + 100f, hit.point.z), Quaternion.identity);
                        lastShooter = shooterInstant;
                        shooterInstant.GetComponent<shooterCtrl>().SetUpTargetPos(hit.point);
                        readyToFire = false;
                    }

                }
                else
                {
                    targetTracer.SetActive(false);
                    //aimProjector.enabled = false;
                }
            }
        }
    }

    public void RestartAimming(Transform lastShooter)//missed
    {
        readyToFire = true;
    }

    public void Fired()
    {
        readyToFire = false;
    }

    public void RestartAimming()//not missed
    {
        readyToFire = true;
    }
}
