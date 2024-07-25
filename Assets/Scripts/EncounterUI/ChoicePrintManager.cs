using EndOfWorld.EncounterSystem;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ChoicePrintManager : PullingManager
{
    public void PrintChoice(List<TextAndEncounterFile> choice)
    {
        for(int i = nextPullingIndex; i < choice.Count; i++) PullObject();
        SetChoiceContents(choice);
    }
    public void SetChoiceContents(List<TextAndEncounterFile> choice)
    {
        for(int i = 0; i < choice.Count; i++)
        {
            pulledObjectList[i].GetComponent<Button>().gameObject.transform.GetChild(0).GetComponent<TMP_Text>().text = choice[i].text;
            pulledObjectList[i].GetComponent<EncounterButtonStats>().myIndex = i;
        }
    }
}
