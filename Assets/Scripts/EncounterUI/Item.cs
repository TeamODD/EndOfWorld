using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace EndOfWorld.EncounterSystem
{
    public enum EventType
    {
        Encounter,
        Heal,
        UpgradeArmor,
        Enchant
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
        public EventType EventType;
        public EncounterFile encounterFile;
    }
}