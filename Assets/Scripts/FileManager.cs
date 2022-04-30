using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.UI;

public class FileManager : MonoBehaviour
{
    public GameObject settingUI;
    public GameObject pathInputField;
    public GameObject tagInputField;
    public GameObject failedToLoadError;
    public GameObject tagsPrefab;
    public GameObject contentsPrefab;
    public int contentsCounter = 0;
    public List<ImageSimpleInfo> imageSimpleInfoList = new List<ImageSimpleInfo>();
    public List<ImageInfo> imageInfoList = new List<ImageInfo>();
    ImageInfo indexImageInfo = new ImageInfo();

    DirectoryInfo pathDirectory;

    private string path;

    public class ImageSimpleInfo
    {
        public Sprite image;
        public string name;
    }


    public class ImageInfo
    {
        public ImageSimpleInfo imageSimpleInfo;
        public Dictionary<string, bool> imageTags;
    }


    private void Start() {
        if(!File.Exists("data.txt"))    //Checking if theres path data in the file.
        {
            settingUI.SetActive(true);
        } else
        {
            StreamReader sr = new StreamReader("data.txt");
            path = sr.ReadLine();
            Debug.Log("at start, the directory is : " + path);
            pathDirectory = new DirectoryInfo(path);
            Debug.Log(pathDirectory.Exists);
            if(pathDirectory.Exists)    //Checking if theres folder that data.txt's path is pointing.
            {
                Debug.Log("The file is present.");
                imageSimpleInfoList = roadFiles();
                if(File.Exists("FileTags.txt"))
                {
                    imageInfoList = createImageInfo(imageSimpleInfoList);
                    createContents(imageInfoList);
                } else
                {
                    tagInitialize();
                }
            } else
            {
                Debug.Log("The file is not present.");
                failedToLoadError.SetActive(true);
            }
        }
    }



    public List<ImageInfo> createImageInfo(List<ImageSimpleInfo> imageSimpleInfoList)
    {
        List<ImageInfo> indexImageInfoList = new List<ImageInfo>();
        List<string> tagList = getTagList();
        foreach(ImageSimpleInfo imageSimpleInfo in imageSimpleInfoList)
        {
            ImageInfo indexImageInfo = new ImageInfo();
            indexImageInfo.imageSimpleInfo = imageSimpleInfo;
            Dictionary<string, bool> indexTagList = new Dictionary<string, bool>();
            for(int i = 0; i < tagList.Count; i++)
            {
                Debug.Log(indexImageInfo.imageSimpleInfo.name);
                Debug.Log(tagList[i]);
                bool isTagTrue = getTagStatus(indexImageInfo.imageSimpleInfo.name, tagList[i]);
                Debug.Log(isTagTrue);
                indexTagList.Add(tagList[i], isTagTrue);
            }
            indexImageInfo.imageTags = indexTagList;
            indexImageInfoList.Add(indexImageInfo);
        }
        return indexImageInfoList;
    }

    public void changeFilePath()    //Change path to access.
    {
        StreamWriter sw = File.CreateText("data.txt");
        path = pathInputField.GetComponent<Text>().text;
        Debug.Log(path);
        sw.WriteLine(path);
        sw.Close();
    }

    public void addTag()
    {
        createTag(tagInputField.GetComponent<Text>().text);
    }



    public List<ImageSimpleInfo> roadFiles()     //Road all files from folder that data.txt is pointing.
    {
        List<ImageSimpleInfo> indexImageSimpleInfo = new List<ImageSimpleInfo>();
        if(pathDirectory.Exists)
        {
            foreach(FileInfo files in pathDirectory.GetFiles())
            {
                Sprite indexSprite = loadNewSprite(files.FullName);
                if(indexSprite != null)
                {
                    ImageSimpleInfo index = new ImageSimpleInfo();
                    index.image = indexSprite;
                    index.name = files.Name;
                    indexImageSimpleInfo.Add(index);
                }
            }
            return indexImageSimpleInfo;
        }
        return null;
    }



    //Converts external files to Sprite
    public Sprite loadNewSprite(string path)    //Converts Texture2D to Sprite.
    {
        Texture2D spriteTexture = loadTexture(path);
        if(spriteTexture != null)
        {
            Sprite newSprite = Sprite.Create(spriteTexture, new Rect(0,0, spriteTexture.width, spriteTexture.height), new Vector2(0,0), 100f);
            return newSprite;
        }
        return null;
    }

    public Texture2D loadTexture(string path)   //Converts external files to Texture2D.
    {
        Texture2D Tex2D;
        byte[] fileData;

        if(File.Exists(path))
        {
            fileData = File.ReadAllBytes(path);
            Tex2D = new Texture2D(2,2);
            if(Tex2D.LoadImage(fileData))
            {
                return Tex2D;
            }
        }
        return null;
    }



    public void createContents(List<ImageInfo> infoList)    //Create prefab that will be used in ScrollView.
    {
        contentsCounter = 0;
        while(infoList.Count > contentsCounter)
        {
            GameObject indexObj = Instantiate(contentsPrefab, new Vector3(0, 0, 0), Quaternion.identity) as GameObject;
            indexObj.transform.SetParent(GameObject.Find("ImageViewerContents").transform);
            for(int i = 0; i<3; i++)
            {
                indexObj.transform.GetChild(i).GetChild(0).GetComponent<Image>().sprite = infoList[contentsCounter].imageSimpleInfo.image;
                indexObj.transform.GetChild(i).GetChild(0).GetChild(0).GetComponent<Text>().text = infoList[contentsCounter].imageSimpleInfo.name;
                contentsCounter++;
                if(infoList.Count <= contentsCounter)
                {
                    break;
                }
            }
        }
    }



    public bool getTagStatus(string file, string tag)
    {
        foreach (string line in File.ReadLines("FileTags.txt"))
        {
            if(line.Contains(file))
            {
                if(line.Contains(tag + " : False"))
                {
                    return false;
                } else if(line.Contains(tag + " : True"))
                {
                    return true;
                }
            }
        }
        return false;
    }

    public void createTag(string tagName)   //add a new tag to tag list.
    {
        if(!File.Exists("TagList.txt"))
        {
            StreamWriter sw = File.CreateText("TagList.txt");
            sw.WriteLine(tagName);
            sw.Close();
        } else
        {
            bool isAlreadyExists = false;
            foreach(string line in File.ReadLines("TagList.txt"))
            {
                if(line == tagName)
                {
                    isAlreadyExists = true;
                }
            }
            if(!isAlreadyExists)
            {
                StreamWriter sw;
                sw = File.AppendText("TagList.txt");
                sw.WriteLine(tagName);
                sw.Close();
            }
        }
    }


    public List<string> getTagList() // get all tags from tag list.
    {
        List<string> tagNameList = new List<string>();
        foreach(string tagName in File.ReadLines("TagList.txt"))
        {
            tagNameList.Add(tagName);
        }
        return tagNameList;
    }

    public void tagInitialize() // set all file's tags to false.
    {
        StreamWriter sw = File.CreateText("FileTags.txt");
        foreach(ImageSimpleInfo imageInfo in imageSimpleInfoList)
        {
            ImageInfo indexData = new ImageInfo();
            indexData.imageSimpleInfo = imageInfo;
            Dictionary<string,bool> indexTagList = new Dictionary<string, bool>();
            foreach(string tagName in getTagList())
            {
                indexTagList.Add(tagName, false);
            }
            indexData.imageTags = indexTagList;
            sw.WriteLine(makeJson(indexData));
        }
        sw.Close();
        imageInfoList = createImageInfo(imageSimpleInfoList);
    }

    public string makeJson(ImageInfo data) // make a json of the file's tags and name.
    {
        string json = "name : " + data.imageSimpleInfo.name + ", ";
        foreach(KeyValuePair<string, bool> keyValue in data.imageTags)
        {
            json += "tags_" + keyValue.Key + " : " + keyValue.Value + ", ";
        }
        return json;
    }

    //deprecated
    public void toggleFileTag(string file, string tag)
    {
        StreamWriter indexSW = new StreamWriter("index.txt");
        foreach(string line in File.ReadLines("FileTags.txt"))
        {
            if(line.Contains(file))
            {
                if(line.Contains(tag + " : False"))
                {
                    string indexline;
                    indexline = line.Replace(tag + " : False", tag + " : True");
                    indexSW.WriteLine(indexline);
                } else if(line.Contains(tag + " : True"))
                {
                    string indexline;
                    indexline = line.Replace(tag + " : True", tag + " : False");
                    indexSW.WriteLine(indexline);
                }
            } else
            {
                indexSW.WriteLine(line);
            }
        }
        indexSW.Close();
        File.Delete("FileTags.txt");
        File.Move("index.txt", "FileTags.txt");
    }




}
