using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class testfileinfo : MonoBehaviour
{
    private void Awake() {
        FileInfo fileInfo = new FileInfo("d.dat");

        FileStream file = fileInfo.Create();

        file.Close();

        fileInfo.CopyTo("e.dat");

        fileInfo.MoveTo("f.dat");

        bool isExists = fileInfo.Exists;
        Debug.Log(isExists);

        FileAttributes attr = fileInfo.Attributes;
        Debug.Log(attr);

        fileInfo.Delete();
        
    }
}
