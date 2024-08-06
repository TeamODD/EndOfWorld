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
        public ItemType itemType;

        [Tooltip("é�� ��ȣ")]
        public short chaperLevel;
        [Tooltip("���൵ ��ȣ")]
        public short progressLevel;
        [Space(10f)]

        [SerializeReference]
        public List<Item> itemList = new List<Item>();

        [Tooltip("Ư�� ���ǿ� ���� ���߿� ��ī���� ������ ���� ��� �̰��� �����ʽÿ�.")]
        public EncounterFile specialEncounterFile;

        [ContextMenu("AddText")]
        public void AddText()
        {
            itemList.Add(new TextItem());
        }

        [ContextMenu("AddSprite")]
        public void AddSprite()
        {
            itemList.Add(new SpriteItem());
        }

        [ContextMenu("AddChoice")]
        public void AddChoice()
        {
            itemList.Add(new ChoiceItem());
        }
    }
}
