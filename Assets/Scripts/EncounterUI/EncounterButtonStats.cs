using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EncounterButtonStats : MonoBehaviour
{
    EncounterManager encounterManager;

    public int myIndex;

    public void GiveSelectedChoiceIndex()
    {
        encounterManager = GameObject.FindWithTag("EncounterManager").GetComponent<EncounterManager>();
        encounterManager.TakeAChoice(myIndex);
    }
}
