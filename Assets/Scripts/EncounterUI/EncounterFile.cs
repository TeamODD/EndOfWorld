using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EndOfWorld.EncounterSystem
{
    [CreateAssetMenu(fileName = "EncounterFile", menuName = "EncounterSystem/EncounterFile")]
    [System.Serializable]
    public class EncounterFile : ScriptableObject
    {

        [Tooltip("챕터 번호")]
        public short ChaperLevel = 1;
        [Tooltip("진행도 번호")]
        public short ProgressLevel;
        [Space(10f)]

        [SerializeReference]
        [ArrayElementTitle("itemType")]
        public List<Item> ItemList = new List<Item>();


        [ContextMenu("AddText")]
        public void AddText()
        {
            ItemList.Add(new TextItem());
        }

        [ContextMenu("AddSprite")]
        public void AddSprite()
        {
            ItemList.Add(new SpriteItem());
        }

        [ContextMenu("AddChoice")]
        public void AddChoice()
        {
            ItemList.Add(new ChoiceItem());
        }

        [ContextMenu("AddEncounter")]
        public void AddEncounter()
        {
            ItemList.Add(new EncounterItem());
        }

        [ContextMenu("AddSetHP")]
        public void AddSetHP()
        {
            ItemList.Add(new AddHPItem());
        }

        [ContextMenu("AddUpgradeArmor")]
        public void AddUpgradeArmor()
        {
            ItemList.Add(new UpgradeArmorItem());
        }

        [ContextMenu("AddEnchant")]
        public void AddEnchant()
        {
            ItemList.Add(new EnchantItem());
        }

        [ContextMenu("AddSpecialEncounter")]
        public void AddSpecialEncounter()
        {
            ItemList.Add(new SpecialEncounterItem());
        }

        [ContextMenu("AddProgressLevel")]
        public void AddProgressLevel()
        {
            ItemList.Add(new AddProgressLevel());
        }

        [ContextMenu("AddSkipEncounterItem")]
        public void AddSkipEncounterItem()
        {
            ItemList.Add(new AddSkipEncounterItem());
        }

        [ContextMenu("AddStatIncreaseItem")]
        public void AddStatIncreaseItem()
        {
            ItemList.Add(new AddStatIncreaseItem());
        }

        [ContextMenu("AddSkillItem")]
        public void AddSkillItem()
        {
            ItemList.Add(new AddSkillItem());
        }
    }
}
