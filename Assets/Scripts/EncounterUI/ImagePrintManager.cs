using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ImagePrintManager : PullingManager
{
    public void PrintImage(Sprite sprite)
    {
        PullObject();
        SetImageContents(sprite);
    }
    public void SetImageContents(Sprite sprite)
    {
        pulledObjectList[nextPullingIndex - 1].GetComponent<Image>().sprite = sprite;
    }
}
