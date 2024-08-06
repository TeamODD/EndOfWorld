using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ImagePrintManager : PullingManager
{
    Image imageComponent;

    private PrintManager printManager;

    public float fadeSpeed = 1.0f;


    private void Start()
    {
        printManager = this.gameObject.GetComponent<PrintManager>();
    }

    public void PrintImage(Sprite sprite)
    {
        PullObject();
        SetImageObject();
        SetImageContents(sprite);
        StartCoroutine(ImageFadeAnimation());
    }

    private void SetImageObject()
    {
        imageComponent = pulledObjectList[nextPullingIndex - 1].GetComponent<Image>();
    }

    private void SetImageContents(Sprite sprite)
    {
        if (imageComponent.color.a != 0)
            imageComponent.color = new Color(1, 1, 1, 0);

        imageComponent.sprite = sprite;
    }

    private void EndPrint()
    {
        printManager.isPrintDone = true;
    }

    IEnumerator ImageFadeAnimation()
    {
        float alpha = 0;

        while(imageComponent.color.a < 1)
        {
            imageComponent.color = new Color(1, 1, 1, alpha);
            alpha += Time.deltaTime;
            yield return null;
        }

        imageComponent.color = new Color(1, 1, 1, alpha);

        EndPrint();

        yield return null;
    }
}
