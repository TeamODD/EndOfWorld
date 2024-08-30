using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Burst.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;


namespace EndOfWorld.EncounterSystem
{
    public enum ItemType
    {
        Text,
        Sprite,
        Choice,
        Encounter,
        SetHP,
        UpgradeArmor,
        Enchant,
        SpecialEncounter
    }

    [System.Serializable]
    public class Item
    {
        [HideInInspector]
        public ItemType itemType;

        public Item(ItemType itemType)
        {
            this.itemType = itemType;
        }
        public ItemType ItemType
        {
            get => itemType;
        } 
    }

    [System.Serializable]
    public class TextItem : Item
    {
        [TextArea (3, 8)]
        public string text;

        public TextItem(/*string text*/) : base(ItemType.Text)
        {
            //this.text = text;
        }

        public string Text
        {
            get => text;
        }
    }

    [System.Serializable]
    public class SpriteItem : Item
    {
        public Sprite sprite;

        public SpriteItem(/*Sprite sprite*/) : base(ItemType.Sprite)
        {
            //this.sprite = sprite;
        }

        public Sprite Sprite
        {
            get => sprite;
        }
    }

    [System.Serializable]
    public class ChoiceItem : Item
    {
        public List<ChoiceContents> choiceList;

        public ChoiceItem() : base(ItemType.Choice)
        { }


    }

    [System.Serializable]
    public class ChoiceContents
    {
        public string text;
        public EncounterFile encounterFile;
    }

    [System.Serializable]
    public class EncounterItem : Item
    {
        public GameObject Monster;

        public EncounterItem() : base(ItemType.Encounter)
        {
            Monster = new GameObject();
        }
    }

    [System.Serializable]
    public class AddHPItem : Item
    {
        public int HpPoint;

        PlayerData _playerData;

        public void AddHpPoint(int amount)
        {
            _playerData = GameObject.FindWithTag("PlayerData").GetComponent<PlayerData>();

            _playerData.CurrentHP += amount;
        }

        public AddHPItem() : base(ItemType.SetHP)
        { }
    }

    [System.Serializable]
    public class UpgradeArmorItem : Item
    {
        public int DefensePoint;

        PlayerData _playerData;

        public void AddDefensePoint(int amount)
        {
            _playerData = GameObject.FindWithTag("PlayerData").GetComponent<PlayerData>();

            _playerData.DefencePoint += amount;
        }

        public UpgradeArmorItem() : base(ItemType.UpgradeArmor) { }
    }

    [System.Serializable]
    public class EnchantItem : Item
    {
        EnchantManager _enchantManager;

        public EnchantItem() : base(ItemType.Enchant) 
        {

        }
    }

    [System.Serializable]
    public class SpecialEncounterItem : Item
    {
        public EncounterFile SpecialEncounterFile;

        public SpecialEncounterItem() : base(ItemType.SpecialEncounter) { }
    }
}