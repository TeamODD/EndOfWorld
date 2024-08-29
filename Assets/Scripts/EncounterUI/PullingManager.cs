#pragma warning disable 8321

using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public abstract class PullingManager : MonoBehaviour
{
    [SerializeField]
    protected GameObject initObject;

    private Canvas _canvas;

    protected List<GameObject> pulledObjectList = new List<GameObject>();

    protected int nextPullingIndex = 0;

    private static int _hierarchyIndex = 0;

    private int _objectInstantiateCount = 3;

    private GameObject _scrollViewObject;

    private void Awake()
    {
        init();
    }

    private void init()
    {
        //canvas = this.GetComponent<PrintManager>().encounterUICanvas;
        _scrollViewObject = this.GetComponent<PrintManager>().scrollViewObject;

        //미리 오브젝트 생성해놓기
        for (int i = 0; i < _objectInstantiateCount; i++)
        {
            pulledObjectList.Add(Instantiate(initObject, _scrollViewObject.transform.position, Quaternion.identity));
            pulledObjectList[i].transform.SetParent(_scrollViewObject.transform, false);
            pulledObjectList[i].SetActive(false);
        }
    }

    public void PullObject()
    {
        //풀링할 오브젝트가 있는지 없는지 체크하여 Instantiate 혹은 활성화
        CheckIndex();

        if (nextPullingIndex > pulledObjectList.Count - 1)
        {
            pulledObjectList.Add(Instantiate(initObject, _canvas.transform.position, Quaternion.identity));
            pulledObjectList[nextPullingIndex].transform.SetParent(_canvas.transform);
        }
        else
            pulledObjectList[nextPullingIndex].SetActive(true);

        pulledObjectList[nextPullingIndex].transform.SetSiblingIndex(_hierarchyIndex++);
        nextPullingIndex++;
    }

    private void CheckIndex()
    {
        int i;
        for (i = 0; i < pulledObjectList.Count; i++)
        {
            if (pulledObjectList[i].activeSelf == false)
            {
                nextPullingIndex = i;
                break;
            }
        }
        if (i == pulledObjectList.Count)
            nextPullingIndex = i;
    }

    public void ReturnAllObject()
    {
        for (int i = nextPullingIndex - 1; i >= 0; i--)
        {
            StartCoroutine(FadeAction(i));
        }

        CheckIndex();
    }

    void ObjectActiveFalse()
    {
        for (int i = nextPullingIndex - 1; i >= 0; i--)
        {
            pulledObjectList[i].SetActive(false);
            pulledObjectList[i].transform.SetSiblingIndex((pulledObjectList.Count - 1) - _hierarchyIndex);
            _hierarchyIndex -= 1;
        }
    }

    IEnumerator FadeAction(int index)
    {

        if(initObject.GetComponent<TMP_Text>() != null)
        {
            TMP_Text _textComponent = pulledObjectList[index].GetComponent<TMP_Text>();
            float alpha = 1;

            while (alpha > 0)
            {
                _textComponent.color = new Color(_textComponent.color.r, _textComponent.color.g, _textComponent.color.b, alpha);
                alpha -= Time.deltaTime;
                yield return null;
            }

            _textComponent.color = new Color(_textComponent.color.r, _textComponent.color.g, _textComponent.color.b, 0);
            ObjectActiveFalse();
        }



        else if(initObject.GetComponent<Button>() != null)
        {
            Image _buttonImageComponent = pulledObjectList[index].GetComponent<Image>();
            TMP_Text _textComponent = pulledObjectList[index].transform.GetChild(0).GetComponent<TMP_Text>();
            float alpha = 1;

            while (alpha > 0)
            {
                _buttonImageComponent.color = new Color(_buttonImageComponent.color.r, _buttonImageComponent.color.g, _buttonImageComponent.color.b, alpha);
                _textComponent.color = new Color(_textComponent.color.r, _textComponent.color.g, _textComponent.color.b, alpha);
                alpha -= Time.deltaTime;
                yield return null;
            }

            _buttonImageComponent.color = new Color(_buttonImageComponent.color.r, _buttonImageComponent.color.g, _buttonImageComponent.color.b, 0);
            _textComponent.color = new Color(_textComponent.color.r, _textComponent.color.g, _textComponent.color.b, 0);
            ObjectActiveFalse();
        }



        else if(initObject.GetComponent<Image>() != null)
        {
            Image _ImageComponent = pulledObjectList[index].GetComponent<Image>();
            float alpha = 1;

            while (alpha > 0)
            {
                _ImageComponent.color = new Color(_ImageComponent.color.r, _ImageComponent.color.g, _ImageComponent.color.b, alpha);
                alpha -= Time.deltaTime;
                yield return null;
            }

            _ImageComponent.color = new Color(_ImageComponent.color.r, _ImageComponent.color.g, _ImageComponent.color.b, 0);
            ObjectActiveFalse();
        }


        else
        {
            Debug.Log("Error occurs at FadeAction on PullingManager.cs");
        }

        yield return null;
    }
}
