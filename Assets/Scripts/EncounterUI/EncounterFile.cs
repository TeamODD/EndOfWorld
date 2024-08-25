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
        [HideInInspector]
        public ItemType ItemType;

        [Tooltip("é�� ��ȣ")]
        public short ChaperLevel;
        [Tooltip("���൵ ��ȣ")]
        public short ProgressLevel;
        [Space(10f)]

        [SerializeReference]
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
    }
}