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
    protected List<GameObject> pulledObjectList = new List<GameObject>();

    protected int nextPullingIndex = 0;

    private static int hierarchyIndex = 0;

    private int objectInstantiateCount = 3;



    private void Awake()
    {
        init();
    }

    private void init()
    {
        canvas = this.GetComponent<PrintManager>().encounterUICanvas;

        //�̸� ������Ʈ �����س���
        for (int i = 0; i < objectInstantiateCount; i++)
        {
            pulledObjectList.Add(Instantiate(_object, canvas.transform.position, Quaternion.identity));
            pulledObjectList[i].transform.SetParent(canvas.transform, false);
            pulledObjectList[i].SetActive(false);
        }
    }

    public void PullObject()
    {
        //Ǯ���� ������Ʈ�� �ִ��� ������ üũ�Ͽ� Instantiate Ȥ�� Ȱ��ȭ
        CheckIndex();

        if (nextPullingIndex > pulledObjectList.Count - 1)
        {
            pulledObjectList.Add(Instantiate(_object, canvas.transform.position, Quaternion.identity));
            pulledObjectList[nextPullingIndex].transform.SetParent(canvas.transform);
        }
        else
            pulledObjectList[nextPullingIndex].SetActive(true);

        pulledObjectList[nextPullingIndex].transform.SetSiblingIndex(hierarchyIndex++);
        nextPullingIndex++;
    }

    public void ReturnAllObject()
    {
        for (int i = nextPullingIndex - 1; i >= 0; i--)
        {
            pulledObjectList[i].SetActive(false);
        }

        CheckIndex();
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
}
