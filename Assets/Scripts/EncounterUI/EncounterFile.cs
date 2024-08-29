using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EndOfWorld.EncounterSystem
{
    public enum ItemType
    {
        Text,
        Sprite,
        Choice,
    }

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

        [Tooltip("Ư�� ���ǿ� ���� ���߿� ��ī���� ������ ���� ��� �̰��� �����ʽÿ�.")]
        public EncounterFile SpecialEncounterFile;

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
    }
}
