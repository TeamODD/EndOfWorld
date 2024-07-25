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

    public bool PrintContent(string text)
    {
        textManager.PrintText(text);
        return true;
    }
    public bool PrintContent(Sprite sprite)
    {
        imageManager.PrintImage(sprite);
        return true;
    }
    public bool PrintContent(List<TextAndEncounterFile> choiceList)
    {
        choiceManager.PrintChoice(choiceList);
        return true;
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
