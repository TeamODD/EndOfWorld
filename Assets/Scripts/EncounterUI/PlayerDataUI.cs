using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class PlayerDataUI : MonoBehaviour
{
    [SerializeField]
    private PlayerData _playerData;

    [SerializeField]
    private GameObject _data;

    [SerializeField]
    private Canvas _canvas;

    private CanvasGroup _canvasGroup;

    private TMP_Text _maxHPText;
    private TMP_Text _attackPointText;
    private TMP_Text _defensePointText;
    private TMP_Text _speedPointText;

    private int _maxHP;
    private int _attackPoint;
    private int _defensePoint;
    private int _speedPoint;

    VerticalLayoutGroup _verticalLayoutGroup;

    private void Awake()
    {
        if(_playerData == null)
            _playerData = GameObject.FindWithTag("PlayerData").GetComponent<PlayerData>();

        _canvasGroup = _canvas.GetComponent<CanvasGroup>();

        _verticalLayoutGroup = _data.GetComponent<VerticalLayoutGroup>();
    }

    private void Start()
    {
        GetData();
        GetTextObject();

        if(_canvas.gameObject.activeSelf)
        {
            _canvasGroup.alpha = 0;
            _canvas.gameObject.SetActive(false);
        }
    }




    private void GetData()
    {
        this._maxHP = _playerData.MaxHP;
        this._attackPoint = _playerData.AttackPoint;
        this._defensePoint = _playerData.DefencePoint;
        this._speedPoint = _playerData.SpeedPoint;
    }

    private void GetTextObject()
    {
        _maxHPText = _data.transform.GetChild(0).gameObject.GetComponent<TMP_Text>();
        _attackPointText = _data.transform.GetChild(1).gameObject.GetComponent<TMP_Text>();
        _defensePointText = _data.transform.GetChild(2).gameObject.GetComponent<TMP_Text>();
        _speedPointText = _data.transform.GetChild(3).gameObject.GetComponent<TMP_Text>();
    }

    private void PrintText()
    {
        _maxHPText.text = "Max Hp : " + this._maxHP.ToString();
        _attackPointText.text = "Attack Point : " + this._attackPoint.ToString();
        _defensePointText.text = "Defense Point : " + this._defensePoint.ToString();
        _speedPointText.text = "Speed Point : " + this._speedPoint.ToString();
    }

    public void ShowPlayerData()
    {
        _canvas.gameObject.SetActive(true);
        StartCoroutine(FadeOut());
    }

    public void ClosePlayerData()
    {
        StartCoroutine(FadeIn());
    }

    IEnumerator FadeIn()
    {
        float time = 1;

        while (time > 0)
        {
            _canvasGroup.alpha = time;
            time -= Time.deltaTime;
            yield return null;
        }

        _canvasGroup.alpha = 0;

        _canvas.gameObject.SetActive(false);
        yield return null;
    }

    IEnumerator FadeOut()
    {
        float time = 0;

        GetData();
        PrintText();

        _verticalLayoutGroup.enabled = false;
        Debug.Log("false");
        _verticalLayoutGroup.enabled = true;

        while (time < 1)
        {
            _canvasGroup.alpha = time;
            time += Time.deltaTime;
            yield return null;
        }

        _canvasGroup.alpha = 1;

        yield return null;
    }

    private void Update()
    {
        //���� ���� fix�� ���� �ڵ�
        _verticalLayoutGroup.enabled = false;
        _verticalLayoutGroup.enabled = true;
    }
}
