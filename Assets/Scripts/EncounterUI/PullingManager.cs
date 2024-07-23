using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public abstract class PullingManager : MonoBehaviour
{
    [SerializeField]
    protected GameObject _object;
    private Canvas canvas;
    protected List<GameObject> list = new List<GameObject>();

    protected int nextPullingIndex = 0;
    private static int hierarchyIndex = 0;
    private int objectInstantiateCount = 3;

    private void Awake()
    {
        init();
    }

    private void init()
    {
        canvas = this.GetComponent<PrintManager>().canvas;
        for (int i = 0; i < objectInstantiateCount; i++)
        {
            list.Add(Instantiate(_object, canvas.transform.position, Quaternion.identity));
            list[i].transform.SetParent(canvas.transform, false);
            list[i].SetActive(false);
        }
    }

    public void PullObject()
    {
        //풀링할 오브젝트가 있는지 없는지 체크하여 Instantiate 혹은 활성화
        indexCheck();

        if (nextPullingIndex > list.Count - 1)
        {
            list.Add(Instantiate(_object, canvas.transform.position, Quaternion.identity));
            list[nextPullingIndex].transform.SetParent(canvas.transform);
        }
        else
            list[nextPullingIndex].SetActive(true);

        list[hierarchyIndex++].transform.SetSiblingIndex(nextPullingIndex);
        nextPullingIndex++;
    }
    private void indexCheck()
    {
        int i;
        for (i = 0; i < list.Count; i++)
        {
            if (list[i].activeSelf == false)
            {
                nextPullingIndex = i;
                break;
            }
        }
        if (i == list.Count)
            nextPullingIndex = i;
    }
}
