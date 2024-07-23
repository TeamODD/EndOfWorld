using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PrintManager : MonoBehaviour
{
    TextPrintManager textManager;

    ImagePrintManager imageManager;

    ChoicePrintManager choiceManager;

    public Canvas canvas;

    void Awake()
    {
        init();
    }
    void init()
    {
        textManager = GetComponent<TextPrintManager>();
        imageManager = GetComponent<ImagePrintManager>();
        choiceManager = GetComponent<ChoicePrintManager>();
        //canvas = GameObject.FindWithTag("EncounterUI").GetComponent<Canvas>();
    }

}
