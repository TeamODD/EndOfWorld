using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using TMPro;

public class OnUIHPTextChange : MonoBehaviour
{
    [SerializeField]
    PlayerData _playerData;

    TMP_Text _textComponent;

    private void Start()
    {
        _playerData.OnHPChanged.AddListener(TextChange);
        _textComponent = this.gameObject.GetComponent<TMP_Text>();
    }

    private void TextChange(int maxHp, int currentHp)
    {
        _textComponent.SetText("HP " +  currentHp + "/" + maxHp);
    }
}
