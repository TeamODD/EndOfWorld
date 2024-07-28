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
        public short chaperIndex;
        [Tooltip("���൵ ��ȣ")]
        public short progressIndex;
        [Space(10f)]

        [SerializeReference]
        public List<Item> itemList = new List<Item>();

        [Space (10f)]
        [Header("If it have special condition")]
        [Space (5f)]
        [Tooltip("Ư�� ���ǿ� ���� ���߿� ��ī���� ������ ���� ��� �̰��� �����ʽÿ�.")]
        public bool isHaveSpecialEncounter = false;
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
