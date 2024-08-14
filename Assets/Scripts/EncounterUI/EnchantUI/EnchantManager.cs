using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using EndOfWorld.EncounterSystem;
using Unity.VisualScripting;
using TMPro;

public class EnchantManager : MonoBehaviour
{
    private enum Stats
    {
        HP,
        ATK,
        DEF,
        DEX
    }

    [SerializeField]
    private Image _background;

    [SerializeField]
    private Image _foreground;

    [SerializeField]
    private Image _choiceA;
    [SerializeField]
    private Image _choiceB;

    private TMP_Text _textA;
    private TMP_Text _textB;

    private CanvasGroup _canvasGroup;

    private int[,] _statsAmount = new int[4, 3];

    private string _statsA;
    private string _statsB;

    private int _amountA;
    private int _amountB;

    private PlayerData _playerData;

    [HideInInspector]
    public bool IsEnchantDone = false;

    private void Start()
    {
        init();
    }

    private void init()
    {
        //�ʱⰪ ����
        for (int i = 0; i < 4; i++)
        {
            for (int j = 0; j < 3; j++)
            {
                if (i == 0)
                {
                    _statsAmount[i, j] = (j + 1) * 5;
                }

                _statsAmount[i, j] = j + 1;
            }
        }

        _textA = _choiceA.gameObject.transform.GetChild(0).GetComponent<TMP_Text>();
        _textB = _choiceB.gameObject.transform.GetChild(0).GetComponent<TMP_Text>();
        _canvasGroup = this.gameObject.transform.parent.GetComponent<CanvasGroup>();
        _playerData = GameObject.FindWithTag("PlayerData").GetComponent<PlayerData>();


        if(this.gameObject.transform.parent.gameObject.activeSelf)
        {
            this.gameObject.transform.parent.gameObject.SetActive(false);
        }
    }

    public void StartEnchantManager()
    {
        DrawTwoRandomStats();
        StartCoroutine(FadeOut());
    }


    private void DrawTwoRandomStats()
    {
        int choiceA = Random.Range(0, 4);
        int choiceB = Random.Range(0, 4);

        //�ߺ� ����
        while(choiceA == choiceB)
        {
            choiceA = Random.Range(0, 4);
            choiceB = Random.Range(0, 4);
        }

        switch(choiceA)
        {
            case 0:
                _statsA = "�ִ� ü��";
                break;
            case 1:
                _statsA = "���ݷ�";
                break;
            case 2:
                _statsA = "����";
                break;
            case 3:
                _statsA = "��ø��";
                break;
        }

        switch (choiceB)
        {
            case 0:
                _statsB = "�ִ� ü��";
                break;
            case 1:
                _statsB = "���ݷ�";
                break;
            case 2:
                _statsB = "����";
                break;
            case 3:
                _statsB = "��ø��";
                break;
        }

        _amountA = _statsAmount[choiceA, Random.Range(0, 3)];
        _amountB = _statsAmount[choiceB, Random.Range(0, 3)];
    }

    private void PrintText()
    {
        _textA.text = _statsA + "+" + _amountA;
        _textB.text = _statsB + "+" + _amountB;
    }



    IEnumerator FadeIn()
    {
        float time = 1;

        while(time > 0)
        {
            _canvasGroup.alpha = time;
            time -= Time.deltaTime;
            yield return null;
        }

        _canvasGroup.alpha = 0;

        this.IsEnchantDone = true;
        yield return null;
    }

    IEnumerator FadeOut()
    {
        float time = 0;

        PrintText();

        while (time < 1)
        {
            _canvasGroup.alpha = time;
            time += Time.deltaTime;
            yield return null;
        }

        _canvasGroup.alpha = 1;

        yield return null;
    }



    public void SelectA()
    {
        switch(_statsA)
        {
            case "�ִ� ü��":
                _playerData.SetMaxHP(_amountA);
                break;
            case "���ݷ�":
                _playerData.SetATK(_amountA);
                break;
            case "����":
                _playerData.SetDEF(_amountA);
                break;
            case "��ø��":
                _playerData.SetDEX(_amountA);
                break;
        }

        StartCoroutine(FadeIn());
    }

    public void SelectB()
    {
        switch (_statsB)
        {
            case "�ִ� ü��":
                _playerData.SetMaxHP(_amountB);
                break;
            case "���ݷ�":
                _playerData.SetATK(_amountB);
                break;
            case "����":
                _playerData.SetDEF(_amountB);
                break;
            case "��ø��":
                _playerData.SetDEX(_amountB);
                break;
        }

        StartCoroutine(FadeIn());
    }
}

