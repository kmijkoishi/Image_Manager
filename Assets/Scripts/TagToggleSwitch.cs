using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TagToggleSwitch : MonoBehaviour
{

    ImageInfo currentImageInfo;
    private void Awake() {
        string file = GameObject.Find("FilePreviewImage").transform.GetChild(0).GetComponent<Text>().text;
        ImageInfoData indexIID = SLSystem.loadFile();
        foreach(ImageInfo imageInfo in indexIID.imageInfos)
        {
            if(imageInfo.name == file)
            {
                currentImageInfo = imageInfo;
            }
        }
    }
    public void toggleTagState(string name)
    {
        string file = GameObject.Find("FilePreviewImage").transform.GetChild(0).GetComponent<Text>().text;
        string tag = this.transform.Find(name).GetChild(1).GetComponent<Text>().text;
        bool bol = this.transform.Find(name).GetComponent<Toggle>().isOn;
        ImageInfoData indexIID = SLSystem.loadFile();
        List<ImageInfo> indexIIL = new List<ImageInfo>();
        foreach(ImageInfo imageInfo in indexIID.imageInfos)
        {
            if(imageInfo.name == file)
            {
                imageInfo.tags[tag] = bol;
            }
            indexIIL.Add(imageInfo);
        }
        indexIID.imageInfos = indexIIL;
        SLSystem.saveFile(indexIID);
    }
}
