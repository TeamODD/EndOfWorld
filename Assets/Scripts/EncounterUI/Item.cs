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
        [SerializeField]
        protected readonly ItemType itemType;

        protected Item(ItemType itemType)
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
        public bool Encounter;

        public EncounterItem() : base(ItemType.Encounter)
        { }
    }

    [System.Serializable]
    public class SetHPItem : Item
    {
        public bool _SetHP;

        PlayerData _playerData;

        public void SetHP(int amount)
        {
            _playerData = GameObject.FindWithTag("PlayerData").GetComponent<PlayerData>();

            _playerData.CurrentHP += amount;
        }

        public SetHPItem() : base(ItemType.SetHP)
        { }
    }

    [System.Serializable]
    public class UpgradeArmorItem : Item
    {
        public bool UpgradeArmor;

        PlayerData _playerData;

        public void SetDef(int amount)
        {
            _playerData = GameObject.FindWithTag("PlayerData").GetComponent<PlayerData>();

            _playerData.DefencePoint += amount;
        }

        public UpgradeArmorItem() : base(ItemType.UpgradeArmor) { }
    }

    [System.Serializable]
    public class EnchantItem : Item
    {
        public bool Enchant;

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