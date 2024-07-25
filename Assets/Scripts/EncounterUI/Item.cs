using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;


namespace EndOfWorld.EncounterSystem
{
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
        [SerializeField]
        [TextArea (3, 8)]
        private string text;

        public TextItem(string text) : base(ItemType.Text)
        {
            this.text = text;
        }

        public string Text
        {
            get => text;
        }
    }

    [System.Serializable]
    public class SpriteItem : Item
    {
        [SerializeField]
        private Sprite sprite;

        public SpriteItem(Sprite sprite) : base(ItemType.Sprite)
        {
            this.sprite = sprite;
        }

        public Sprite Sprite
        {
            get => sprite;
        }
    }

    [System.Serializable]
    public class ChoiceItem : Item
    {
        [SerializeField]
        private List<string> choice;

        public ChoiceItem() : base(ItemType.Choice)
        { }
    }
}