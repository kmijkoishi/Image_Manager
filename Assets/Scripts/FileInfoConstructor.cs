using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;

public class FileInfoConstructor : MonoBehaviour
{

    public int contentsCounter = 0;
    public GameObject tagsPrefab;
    public void showFileInfo(string name)    //Create prefab that will be used in ScrollView.
    {
        ImageInfoData data = SLSystem.loadFile();
        List<string> tags = SLSystem.loadTagList();

        foreach(Transform child in GameObject.Find("TagsViewerContents").transform)
        {
            GameObject.Destroy(child.gameObject);
        }
        
        Sprite sprite = this.transform.Find(name).GetChild(0).GetComponent<Image>().sprite;

        GameObject.Find("FilePreviewImage").transform.GetComponent<Image>().sprite = sprite;

        string file = this.transform.Find(name).GetChild(0).GetChild(0).GetComponent<Text>().text;

        GameObject.Find("FilePreviewImage").transform.GetChild(0).GetComponent<Text>().text = file;

        Dictionary<string, bool> tagState = new Dictionary<string, bool>();
        for(int i = 0; i < data.imageInfos.Count; i++)
        {
            if(data.imageInfos[i].name == file)
            {
                tagState = data.imageInfos[i].tags;
            }
        }

        contentsCounter = 0;
        while(tags.Count > contentsCounter)
        {
            GameObject indexObj = Instantiate(tagsPrefab, new Vector3(0, 0, 0), Quaternion.identity) as GameObject;
            indexObj.transform.SetParent(GameObject.Find("TagsViewerContents").transform);
            for(int i = 0; i<3; i++)
            {
                indexObj.transform.GetChild(i).GetChild(1).GetComponent<Text>().text = tags[contentsCounter];
                indexObj.transform.GetChild(i).GetComponent<Toggle>().isOn = tagState[tags[contentsCounter]];
                contentsCounter++;
                if(tags.Count <= contentsCounter)
                {
                    break;
                }
            }
        }
    }
}
