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
        SetImage(sprite);
    }
    public void SetImage(Sprite sprite)
    {
        list[nextPullingIndex - 1].GetComponent<Image>().sprite = sprite;
    }
}
