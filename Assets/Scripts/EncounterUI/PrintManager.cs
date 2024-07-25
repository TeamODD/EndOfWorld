using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PrintManager : MonoBehaviour
{
    TextPrintManager textManager;

    ImagePrintManager imageManager;

    ChoicePrintManager choiceManager;

    [HideInInspector]
    public Canvas canvas;

    public Sprite sampleSprite;
    void Awake()
    {
        init();
    }
    void init()
    {
        textManager = GetComponent<TextPrintManager>();
        imageManager = GetComponent<ImagePrintManager>();
        choiceManager = GetComponent<ChoicePrintManager>();
        canvas = GameObject.Find("EncounterUICanvas").GetComponent<Canvas>();
    }

    private void Start()
    {
        textManager.PrintText("12312412412");
        imageManager.PrintImage(sampleSprite);
        imageManager.PrintImage(sampleSprite);
        imageManager.PrintImage(sampleSprite);
    }
}
