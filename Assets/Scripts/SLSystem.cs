using UnityEngine;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public static class SLSystem
{
    public static void saveFile(ImageInfoData imageInfoData)
    {
        BinaryFormatter formatter = new BinaryFormatter();
        string path = Application.persistentDataPath + "/KoishImageData";
        FileStream stream = new FileStream(path, FileMode.Create);

        formatter.Serialize(stream, imageInfoData);
        stream.Close();
    }

    public static ImageInfoData loadFile()
    {
        string path = Application.persistentDataPath + "/KoishImageData";
        if(File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);

            ImageInfoData data = formatter.Deserialize(stream) as ImageInfoData;
            stream.Close();

            return data;
        } else
        {
            Debug.LogError("There's no data in " + path);
            return null;
        }
    }

    public static void savePath(string p)
    {
        BinaryFormatter formatter = new BinaryFormatter();
        string path = Application.persistentDataPath + "/path";
        FileStream stream = new FileStream(path, FileMode.Create);

        formatter.Serialize(stream, p);
        stream.Close();
    }

    public static string loadPath()
    {
        string path = Application.persistentDataPath + "/path";
        if(File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);

            string p = formatter.Deserialize(stream) as string;
            stream.Close();

            return p;
        } else
        {
            Debug.LogError("There's no data in " + path);
            return null;
        }
    }

    public static void saveTagList(List<string> tags)
    {
        BinaryFormatter formatter = new BinaryFormatter();
        string path = Application.persistentDataPath + "/tags";
        FileStream stream = new FileStream(path, FileMode.Create);

        formatter.Serialize(stream, tags);
        stream.Close();
    }

    public static List<string> loadTagList()
    {
        
        string path = Application.persistentDataPath + "/tags";
        if(File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);

            List<string> tags = formatter.Deserialize(stream) as List<string>;
            stream.Close();

            return tags;
        } else
        {
            Debug.LogError("There's no data in " + path);
            return null;
        }
        
    }

}
