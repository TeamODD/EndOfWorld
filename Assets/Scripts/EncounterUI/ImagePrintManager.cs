using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ImagePrintManager : PullingManager
{
    Image _imageComponent;

    private PrintManager _printManager;

    public float FadeSpeed = 1.0f;

    private float _imageRectHeight;


    private void Start()
    {
        _printManager = this.gameObject.GetComponent<PrintManager>();

        _imageRectHeight = 800;
    }

    public void PrintImage(Sprite sprite)
    {
        PullObject();
        SetImageObject();
        SetImageContents(sprite);
        SetResolution(sprite);
        StartCoroutine(ImageFadeAnimation());
    }

    private void SetImageObject()
    {
        _imageComponent = pulledObjectList[nextPullingIndex - 1].GetComponent<Image>();
    }

    private void SetImageContents(Sprite sprite)
    {
        if (_imageComponent.color.a != 0)
            _imageComponent.color = new Color(1, 1, 1, 0);

        _imageComponent.sprite = sprite;
    }

    private void SetResolution(Sprite sprite)
    {
        float boundX = sprite.bounds.size.x;
        float boundY = sprite.bounds.size.y;

        boundY = boundY / boundX;
        boundY = boundY * _imageRectHeight;

        RectTransform rectTransform = _imageComponent.gameObject.GetComponent<RectTransform>();
        rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, boundY);
    }

    private void EndPrint()
    {
        _printManager.isPrintDone = true;
    }

    IEnumerator ImageFadeAnimation()
    {
        float alpha = 0;

        while(_imageComponent.color.a < 1)
        {
            _imageComponent.color = new Color(1, 1, 1, alpha);
            alpha += Time.deltaTime;
            yield return null;
        }

        _imageComponent.color = new Color(1, 1, 1, alpha);

        EndPrint();

        yield return null;
    }
}
