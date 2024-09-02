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
        SpecialEncounter,
        ProgressLevel,
        SkipEncounterItem,
        StatIncreaseItem,
        SkillItem
    }

    public enum StatType
    {
        ATK,
        DEF,
        DEX
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
        [TextArea(3, 8)]
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

        //선택지 클릭시 팝업 화면이 떠야할 경우에 나오게 할 팝업 UI 오브젝트
        public GameObject PopupObject;
    }

    [System.Serializable]
    public class EncounterItem : Item
    {
        public GameObject Enemy;

        public List<CombatResultReport> CombatResultReportList;

        [System.Serializable]
        public class CombatResultReport
        {
            public CombatResult combatResult;

            public EncounterFile encounterFile;
        }

        public EncounterItem() : base(ItemType.Encounter)
        {
            CombatResultReportList = new List<CombatResultReport>();

            for (int i = 0; i < 3; i++) CombatResultReportList.Add(new CombatResultReport());

            CombatResultReportList[0].combatResult = CombatResult.Win;
            CombatResultReportList[1].combatResult = CombatResult.Lose;
            CombatResultReportList[2].combatResult = CombatResult.Escape;
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

    [System.Serializable]
    public class AddProgressLevel : Item
    {
        public int ProgressLevelRisingCount;

        public AddProgressLevel() : base(ItemType.ProgressLevel) {
            ProgressLevelRisingCount = 1;
        }
    }

    [System.Serializable]
    public class AddSkipEncounterItem : Item
    {
        public AddSkipEncounterItem() : base(ItemType.SkipEncounterItem) { }
    }

    [System.Serializable]
    public class AddStatIncreaseItem : Item
    {
        public StatType StatType;

        public int StatPoint;

        public AddStatIncreaseItem() : base(ItemType.StatIncreaseItem) { }
    }

    [System.Serializable]
    public class AddSkillItem : Item
    {
        public SkillSO Skill;

        public AddSkillItem() : base(ItemType.SkillItem) 
        {
        }
    }
}