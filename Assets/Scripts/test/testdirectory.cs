using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class testdirectory : MonoBehaviour
{
    private void Awake() {
        
        Directory.CreateDirectory("Exercise");

        Directory.Move("Exercise", "Exercise2");

        bool isExists = Directory.Exists("Exercise2");

        string[] directories = Directory.GetDirectories("Exercise2");
        Debug.Log(directories.Length);

        string[] files = Directory.GetFiles("Exercise2");
        Debug.Log(files.Length);
    }
}
