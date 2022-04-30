using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class ImageInfoData
{
    public List<ImageInfo> imageInfos;

    public ImageInfoData(List<ImageInfo> imageInfo)
    {
        imageInfos = imageInfo;
    }
}
