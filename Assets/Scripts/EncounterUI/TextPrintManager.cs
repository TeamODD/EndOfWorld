using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Unity.VisualScripting;

public class TextPrintManager : PullingManager
{
    public void PrintText(string text)
    {
        PullObject();
        SetText(text);
    }
    public void SetText(string text)
    {
        list[nextPullingIndex - 1].GetComponent<TMP_Text>().text = text;
    }
}
