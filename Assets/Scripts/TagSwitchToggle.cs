using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class TagSwitchToggle : MonoBehaviour
{
    public void toggleFileTag()
    {
        var fileLib = GameObject.Find("SystemManager").GetComponent<FileManager>();
    }
}
