using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ChoicePrintManager : PullingManager
{
    public void PrintChoice(List<string> choice)
    {
        for(int i = 0; i < choice.Count; i++) PullObject();
        SetChoiceContents(choice);
    }
    public void SetChoiceContents(List<string> choice)
    {
        for(int i = 0; i < choice.Count; i++)
        {
            pulledObjectList[i].GetComponent<Button>().GetComponent<TMP_Text>().text = choice[i];
        }
    }
}
