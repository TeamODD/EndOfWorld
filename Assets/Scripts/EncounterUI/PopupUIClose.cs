using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EndOfWorld.EncounterSystem;

public class PopupUIClose : MonoBehaviour
{
    private EncounterManager _encounterManager;

    [SerializeField]
    private GameObject _parentObject;

    private void Start()
    {
        _encounterManager = GameObject.FindWithTag("EncounterManager").GetComponent<EncounterManager>();
    }

    public void OnClosePopup()
    {
        _encounterManager.isPopupUIShow = false;

        Destroy(_parentObject);
    }
}
