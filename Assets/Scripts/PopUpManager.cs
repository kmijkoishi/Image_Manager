using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;

public class PopUpManager : MonoBehaviour
{
    public GameObject settingPopUp;
    public GameObject failedToLoadPopUp;

    private void Awake() {
        settingPopUp.SetActive(false);
        failedToLoadPopUp.SetActive(false);

    }
}
