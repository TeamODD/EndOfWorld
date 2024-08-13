using EndOfWorld.EncounterSystem;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ChoicePrintManager : PullingManager
{
    private PrintManager _printManager;

    public float FadeSpeed = 1.0f;

    private void Start()
    {
        _printManager = this.gameObject.GetComponent<PrintManager>();
    }
    private void EndPrint()
    {
        _printManager.isPrintDone = true;
    }

    public void PrintChoice(List<TextAndEncounterFile> choice)
    {
        for(int i = nextPullingIndex; i < choice.Count; i++)
            PullObject();

        SetChoiceContents(choice);
        StartCoroutine(ChoiceFadeAnimation(choice.Count));
    }

    public void SetChoiceContents(List<TextAndEncounterFile> choice)
    {
        for (int i = 0; i < choice.Count; i++)
        {
            pulledObjectList[i].GetComponent<Button>().gameObject.transform.GetChild(0).GetComponent<TMP_Text>().text = choice[i].text;
            pulledObjectList[i].GetComponent<EncounterButtonStats>().myIndex = i;

            if (pulledObjectList[i].GetComponent<Image>().color.a != 0)
            {
                pulledObjectList[i].GetComponent<Image>().color = new Color(1, 1, 1, 0);
                pulledObjectList[i].GetComponent<Button>().gameObject.transform.GetChild(0).GetComponent<TMP_Text>().color = new Color(0, 0, 0, 0);
            }
        }
    }

    IEnumerator ChoiceFadeAnimation(int objectCount)
    {
        float alpha = 0;

        List<Image> imageComponents = new List<Image>();
        List<TMP_Text> textComponents = new List<TMP_Text>();

        for (int i = 0; i < objectCount; i++)
        {
            imageComponents.Add(pulledObjectList[i].GetComponent<Image>());
            textComponents.Add(pulledObjectList[i].GetComponent<Button>().gameObject.transform.GetChild(0).GetComponent<TMP_Text>());
        }

        while (alpha < 1)
        {

            for(int i = 0; i < objectCount; i++)
            {
                imageComponents[i].color = new Color(1, 1, 1, alpha);
                textComponents[i].color = new Color(0, 0, 0, alpha);

                alpha += Time.deltaTime;

                yield return null;
            }
        }

        for (int i = 0; i < objectCount; i++)
        {
            imageComponents[i].color = new Color(1, 1, 1, 1);
            textComponents[i].color = new Color(0, 0, 0, 1);
        }


        EndPrint();

        yield return null;
    }
}