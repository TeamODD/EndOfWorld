using EndOfWorld.EncounterSystem;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PrintManager : MonoBehaviour
{
    TextPrintManager textManager;

    ImagePrintManager imageManager;

    ChoicePrintManager choiceManager;

    [HideInInspector]
    public Canvas encounterUICanvas;

    [HideInInspector]
    public bool isPrintDone = false;

    public GameObject scrollViewObject;


    void Awake()
    {
        init();
    }
    void init()
    {
        textManager = GetComponent<TextPrintManager>();
        imageManager = GetComponent<ImagePrintManager>();
        choiceManager = GetComponent<ChoicePrintManager>();
        encounterUICanvas = GameObject.Find("EncounterUICanvas").GetComponent<Canvas>();
    }

    public void PrintContent(string text)
    {
        textManager.PrintText(text);
    }
    public void PrintContent(Sprite sprite)
    {
        imageManager.PrintImage(sprite);
    }
    public void PrintContent(List<ChoiceContents> choiceList)
    {
        choiceManager.PrintChoice(choiceList);
    }

    public void ReturnObjects()
    {
        textManager.ReturnAllObject();
        imageManager.ReturnAllObject();
        choiceManager.ReturnAllObject();
    }

    public void ReturnChoiceObjects()
    {
        choiceManager.ReturnAllObject();
    }
}
