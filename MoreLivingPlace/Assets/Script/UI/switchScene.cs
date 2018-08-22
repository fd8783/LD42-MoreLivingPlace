using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class switchScene : MonoBehaviour {

	public void SwitchScene(int num)
    {
        BackgroundSetting.SwitchScene(num);
    }

    public void dismissTUT(GameObject dismiss)
    {
        dismiss.SetActive(false);
    }

    public void showTUT(GameObject show)
    {
        show.SetActive(true);
    }
}
