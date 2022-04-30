using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;


public class FileProcessor : MonoBehaviour
{

    public GameObject settingUI;
    public GameObject imagePrefab;
    public GameObject pathInputField;
    public GameObject tagInputField;
    ImageInfoData data;
    SpriteImageInfoData fileData;


    public class SpriteImageInfoData
    {
        public List<ImageInfo> imageInfos;
        public List<Sprite> images;
        
        public SpriteImageInfoData(ImageInfoData imageInfoData, List<Sprite> sprites)
        {
            imageInfos = imageInfoData.imageInfos;
            images = sprites;
        }
    }

    string path;
    void Awake() {
        path = Application.persistentDataPath;
    }
    void Start()
    {
        if(File.Exists(path +"/KoishImageData"))
        {
            data = SLSystem.loadFile();
            fileData = new SpriteImageInfoData(data, GetSprites());
            createPrefab(fileData);
        } else
        {
            if(File.Exists(path + "/path"))
            {
                data = createImageInfoData(SLSystem.loadPath());
                SLSystem.saveFile(data);
                fileData = new SpriteImageInfoData(data, GetSprites());
                createPrefab(fileData);
            } else
            {
                settingUI.SetActive(true);
            }
        }
    }


    public void reloadFiles()
    {
        data = createImageInfoData(SLSystem.loadPath());
        SLSystem.saveFile(data);
        fileData = new SpriteImageInfoData(data, GetSprites());
        createPrefab(fileData);
    }
    public void saveData()
    {
        SLSystem.saveFile(data);
    }
    public void savePath()
    {
        path = pathInputField.GetComponent<Text>().text;
        SLSystem.savePath(path);
    }
    public void addTag()
    {
        string tag = tagInputField.GetComponent<Text>().text;
        List<string> indexTags = SLSystem.loadTagList();
        bool isDupe = false;
        for (int i = 0; i < indexTags.Count; i++)
        {
            if(indexTags[i] == tag)
            {
                isDupe = true;
            }
        }
        if(!isDupe)
        {
            indexTags.Add(tag);
            SLSystem.saveTagList(indexTags);
            data = createImageInfoData(SLSystem.loadPath());
            SLSystem.saveFile(data);
        } else
        {
            Debug.Log("this tag already exist");
        }
        
    }

    public void createPrefab(SpriteImageInfoData imageInfoData)
    {
        foreach(Transform child in GameObject.Find("ImageViewerContents").transform)
        {
            GameObject.Destroy(child.gameObject);
        }
        int i = 0;
        while(imageInfoData.imageInfos.Count > i)
        {
            GameObject prefab = Instantiate(imagePrefab, new Vector3(0, 0, 0), Quaternion.identity) as GameObject;
            prefab.transform.SetParent(GameObject.Find("ImageViewerContents").transform);
            for(int j = 0; j<3; j++)
            {
                prefab.transform.GetChild(j).GetChild(0).GetComponent<Image>().sprite = imageInfoData.images[i];
                prefab.transform.GetChild(j).GetChild(0).GetChild(0).GetComponent<Text>().text = imageInfoData.imageInfos[i].name;
                i++;
                if(imageInfoData.imageInfos.Count <= i)
                {
                    break;
                }
            }
        }
    }
    
    public List<Sprite> GetSprites()
    {
        DirectoryInfo directory = new DirectoryInfo(SLSystem.loadPath());
        List<Sprite> sprites = new List<Sprite>();
        foreach(FileInfo file in directory.GetFiles())
        {
            Sprite sprite = loadNewSprite(file.FullName);
            if(sprite == null)
            {
                continue;
            }
            sprites.Add(sprite);
        }
        return sprites;
    }
    public ImageInfoData createImageInfoData(string path)
    {
        DirectoryInfo directory = new DirectoryInfo(path);
        List<ImageInfo> imageInfos = new List<ImageInfo>();

        foreach(FileInfo file in directory.GetFiles())
        {
            ImageInfo imageInfo = createImageInfo(file);
            if(imageInfo != null)
            {
                imageInfos.Add(imageInfo);
            }
        }
        ImageInfoData imageInfoData = new ImageInfoData(imageInfos);
        return imageInfoData;
    }

    public ImageInfo createImageInfo(FileInfo file)
    {
        ImageInfo imageInfo = new ImageInfo();

        List<string> tags = SLSystem.loadTagList();
        if(tags == null)
        {
            List<string> defaultTags = new List<string>();
            defaultTags.Add("favorite");
            defaultTags.Add("important");
            SLSystem.saveTagList(defaultTags);
            tags = SLSystem.loadTagList();
        }
        Dictionary<string, bool> tagDic = new Dictionary<string, bool>();

        Sprite sprite = loadNewSprite(file.FullName);
        if(sprite == null)
        {
            return null;
        }
        string name = file.Name;
        for(int i = 0; i < tags.Count; i++)
        {
            tagDic.Add(tags[i], false);
        }
        
        imageInfo.name = name;
        imageInfo.tags = tagDic;

        return imageInfo;
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
}
