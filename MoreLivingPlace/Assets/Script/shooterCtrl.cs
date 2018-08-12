using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class shooterCtrl : MonoBehaviour {

    public LayerMask smashTargetLayer;
    public Transform destroyParticle;
    private Transform destroyParticlePos;
    public float smashRange = 1.2f, volume = 1f;
    private int curFuelTankCount = 0;

    private Vector3 targetPos, curPos;
    private bool reachTarget = true, loaded = false;
    private float fallingSpeed = 10f, curFallingSpeed = 10f;
    private Transform loadedHuman;

    private ParticleSystem landingParticle;

    private cageCtrl cageScript;

    private AudioSource audio;

    private TextMeshProUGUI talk;

    private string[] talkText = {"I can do this all DAY !!",
                                "It's a TRAP !!",
                                "I am the chosen one !",
                                "WHY AM I HERE ARRRRRRR",
                                "I believe I can flyyyy",
                                "DREAM CUM TRUEEEE",
                                "Finally",
                                "I am saving this world!",
                                "it's crowded!",
                                "Am I going to fly?",
                                "WOOOO HOOOOOOO",
    };

    // Use this for initialization
    void Awake () {
        fallingSpeed *= Time.deltaTime;
        curFallingSpeed *= Time.deltaTime;
        landingParticle = transform.Find("landingParticle").GetComponent<ParticleSystem>();
        destroyParticlePos = transform.Find("destroyParticlePos");
        cageScript = GameObject.Find("Cage").GetComponent<cageCtrl>();
        audio = GetComponent<AudioSource>();
        talk = transform.Find("Talk/content").GetComponent<TextMeshProUGUI>();
    }

    // Update is called once per frame
    private void Update()
    {
    }

    void FixedUpdate () {
		if (!reachTarget)
        {
            Falling();
        }
	}

    void Falling()
    {
        curPos = transform.position;
        curFallingSpeed += fallingSpeed;
        if (curPos.y - targetPos.y <= curFallingSpeed)
        {
            transform.position = targetPos;
            Reach();
        }
        else
        {
            curPos.y -= curFallingSpeed;
            transform.position = curPos;
        }
    }

    void Reach()
    {
        screenShake.shakecoefficient = 0.5f;
        screenShake.StopScreen(0.05f);
        reachTarget = true;
        landingParticle.Play();
        Smash();
        cageScript.Register(volume*transform.localScale.x);
        //Debug.Log("pushing");
        audio.Play();
        if (loaded)
        {
            cageScript.SetUpTarget(transform);
            StartCoroutine("startTalking", Random.Range(2.5f,3f));
        }
        else
        {
            GameObject.Find("MouseCtrl").GetComponent<mouseCtrl>().RestartAimming(transform);
        }
    }

    void Smash()
    {
        int normalCount = 0;
        float curHeight = 0, curTargetHeight;
        Vector3 tempPos;
        List<int> fuelIndexs = new List<int>();
        Collider[] smashTargets = Physics.OverlapSphere(transform.position, transform.localScale.x * smashRange, smashTargetLayer);
        if (smashTargets.Length > 0)
        {
            float closestDis = float.MaxValue, curDis, closetIndex = -1;
            for (int i = 0; i < smashTargets.Length; i++)
            {
                if (smashTargets[i].CompareTag("FuelTankHuman"))
                {
                    fuelIndexs.Add(i);
                }
                else
                {
                    normalCount++;
                    Vector3 humanPos = smashTargets[i].transform.position;
                    if (smashTargets[i].CompareTag("ThinHuman"))
                    {
                        humanPos.y *= 0.2f;
                    }
                    curDis = Vector3.Distance(transform.position, humanPos);
                    //Debug.Log("tag: " + smashTargets[i].tag + " dis: " + curDis +"\n"+transform.position +"  "+smashTargets[i].transform.position);
                    if (curDis < closestDis)
                    {
                        closetIndex = i;
                        closestDis = curDis;
                    }
                }
            }
            curFuelTankCount = fuelIndexs.Count;
            if (normalCount > 0)
            {
                for (int j = 0; j < smashTargets.Length; j++)
                {
                    if (fuelIndexs.Contains(j)) continue;
                    if (closetIndex == j)
                    {
                        loadedHuman = smashTargets[j].transform;
                        loaded = true;
                    }
                    else
                    {
                        smashTargets[j].GetComponent<humanCtrl>().Death();
                    }
                }
            }
            else
            {
                loadedHuman = smashTargets[fuelIndexs[0]].transform;
                loaded = true;
                fuelIndexs.RemoveAt(0);
            }
            curTargetHeight = loadedHuman.GetComponent<humanCtrl>().height;
            curHeight += curTargetHeight;

            if (fuelIndexs.Count > 0)
            {
                foreach (int index in fuelIndexs)
                {
                    smashTargets[index].GetComponent<humanCtrl>().Loaded(loadedHuman);
                    smashTargets[index].GetComponent<humanCtrl>().enabled = false;
                    tempPos = smashTargets[index].transform.position;
                    curHeight += 2.25f * 0.6f;
                    tempPos.y -= curHeight;
                    smashTargets[index].transform.position = tempPos;
                    curHeight += 2.25f;
                }
            }
            loadedHuman.GetComponent<humanCtrl>().Loaded(transform);
            tempPos = loadedHuman.position;
            tempPos.y += curHeight;
            loadedHuman.position = tempPos;
        }
    }

    IEnumerator startTalking(float time)
    {
        talk.text = talkText[Random.Range(0, talkText.Length)];
        yield return new WaitForSeconds(time);
        talk.text = "";
    }

    public void Fire(float power)
    {
        screenShake.ShakeScreen(1f);
        Debug.Log("checkPower: " + power);
        loadedHuman.GetComponent<humanCtrl>().Fire(power* Mathf.Pow(BackgroundSetting.fuelTankPower, curFuelTankCount));
        Debug.Log("before fuelTank: " + power + " After fuelTank*" + curFuelTankCount + ": " + power * Mathf.Pow(BackgroundSetting.fuelTankPower, curFuelTankCount));
        loadedHuman.parent = null;
        SelfDestroy();
    }

    public void SetUpTargetPos(Vector3 targetPos)
    {
        this.targetPos = targetPos;
        reachTarget = false;
    }

    public void SelfDestroy()
    {
        audio.Play();
        cageScript.Dismiss(volume);
        Instantiate(destroyParticle, destroyParticlePos.position, destroyParticlePos.rotation);
        Collider[] smashTargets = Physics.OverlapSphere(transform.position, transform.localScale.x * smashRange, smashTargetLayer);
        foreach (Collider col in smashTargets)
        {
            col.transform.GetComponent<humanCtrl>().Death();
        }
        Destroy(gameObject);
    }
}
