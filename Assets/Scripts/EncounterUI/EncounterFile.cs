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

        [Tooltip("챕터 번호")]
        public short chaperIndex;
        [Tooltip("진행도 번호")]
        public short progressIndex;
        [Space(10f)]

        [SerializeReference]
        public List<Item> itemList = new List<Item>();

        [Space (10f)]
        [Header("If it have special condition")]
        [Space (5f)]
        [Tooltip("특정 조건에 의해 나중에 인카운터 파일이 생길 경우 이곳에 넣으십시오.")]
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
