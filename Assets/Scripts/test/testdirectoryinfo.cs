using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class testdirectoryinfo : MonoBehaviour
{
    private void Awake()
    {

        DirectoryInfo currentDirectory;

        string path = Application.dataPath;

        currentDirectory = new DirectoryInfo(path);

        currentDirectory.CreateSubdirectory("TestFolder");

        Debug.Log(currentDirectory.FullName);
        Debug.Log(currentDirectory.Name);

        bool isExists = currentDirectory.Exists;

        FileAttributes attr = currentDirectory.Attributes;
        Debug.Log(attr);

        foreach (DirectoryInfo directory in currentDirectory.GetDirectories())
        {
            Debug.Log(directory.Name);
        }

        foreach (FileInfo file in currentDirectory.GetFiles())
        {
            Debug.Log(file.Name);
        }
    }
}
