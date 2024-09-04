using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace EndOfWorld.EncounterSystem
{
    public class EncounterButtonStats : MonoBehaviour
    {
        EncounterManager encounterManager;

        public int myIndex;

        private bool isClicked = false;

        public void OnGiveSelectedChoiceIndex()
        {
            //중복 클릭 방지
            if (isClicked) return;

            encounterManager = GameObject.FindWithTag("EncounterManager").GetComponent<EncounterManager>();

            encounterManager.TakeAChoice(myIndex);

            this.isClicked = true;
            Invoke("ResetClick", 1.1f);
        }

        private void ResetClick()
        {
            isClicked = false;
        }
    }
}
