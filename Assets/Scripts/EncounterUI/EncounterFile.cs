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

        [Tooltip("챕터 번호")]
        public short ChaperLevel;
        [Tooltip("진행도 번호")]
        public short ProgressLevel;
        [Space(10f)]

        [SerializeReference]
        public List<Item> ItemList = new List<Item>();

        [Tooltip("특정 조건에 의해 나중에 인카운터 파일이 생길 경우 이곳에 넣으십시오.")]
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
