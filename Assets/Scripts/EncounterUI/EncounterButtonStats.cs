using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace EndOfWorld.EncounterSystem
{
    public class EncounterButtonStats : MonoBehaviour
    {
        EncounterManager encounterManager;

        public int myIndex;

        public void OnGiveSelectedChoiceIndex()
        {
            encounterManager = GameObject.FindWithTag("EncounterManager").GetComponent<EncounterManager>();
            encounterManager.TakeAChoice(myIndex);
        }
    }
}
