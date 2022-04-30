using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonManager : MonoBehaviour
{
    public GameObject settingPopUp;
    public GameObject failedToLoadErrorPopUp;
    public void toggleSetting()
    {
        if(settingPopUp.activeSelf)
        {
            settingPopUp.SetActive(false);
        } else
        {
            settingPopUp.SetActive(true);
        }
    }

    public void closeFailedToLoad()
    {
        failedToLoadErrorPopUp.SetActive(false);
    }
}
