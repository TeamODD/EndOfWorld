using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Linq;
using static UnityEngine.GraphicsBuffer;
using UnityEditor;

namespace EndOfWorld.EncounterSystem
{
    public class EncounterManager : MonoBehaviour
    {
        [SerializeField]
        private short _chaperIndex;

        [SerializeField]
        private List<EncounterFile> _unusedEncounterFileList;

        [SerializeField]
        private List<EncounterFile> _usedEncounterFileList = new List<EncounterFile>();

        [SerializeField]
        private SavedData _savedData;

        [SerializeField]
        private EncounterFileListForAcquiredFiles _encounterFileListForAcquiredFiles;

        private EncounterFile _encounterFile;

        private PrintManager _printManager;

        private List<Item> _itemList;

        private List<ChoiceContents> _choiceItemList;

        private int _thisProgressLevel = 0;

        private int _encounterFileIndex;


        private void Awake()
        {
            _printManager = GameObject.FindWithTag("PrintManager").GetComponent<PrintManager>();
        }

        private void Start()
        {
            BringSavedData();
            BringAcquiredFiles();


            SelectRandomEncounterFile();
            StartCoroutine(PrintEncounter());
        }


        /// <summary>
        /// 특정한 조건에 의해 추가된 EncounterFile을 가져오는 함수
        /// </summary>
        private void BringAcquiredFiles()
        {
            if (_encounterFileListForAcquiredFiles.AcquiredEncounterFileList == null) return;
            int listLength = _encounterFileListForAcquiredFiles.AcquiredEncounterFileList.Count;

            for (short i = 0; i < listLength; i++)
            {
                //한번도 추가 된 적 없다면 && EncounterFile의 chaperIndex가 EncounterManager의 chaperIndex와 같다면 
                if (!_encounterFileListForAcquiredFiles.IsItAdded(i) && _encounterFileListForAcquiredFiles.GetChaperIndex(i) == this._chaperIndex)
                {   //리스트에 추가
                    _unusedEncounterFileList.Add(_encounterFileListForAcquiredFiles.AcquiredEncounterFileList[i].GetEncounterFile());
                }
            }

            //unusedEncounterFileList = unusedEncounterFileList.Distinct().ToList(); //중복 제거
            SaveData();
        }

        private void BringSavedData()
        {
            if (_savedData.ProgressLevel > 0) //저장 된적이 있는지 없는지 구분
            {
                this._unusedEncounterFileList = _savedData.UnusedEncounterFiles;
                this._usedEncounterFileList = _savedData.UsedEncounterFiles;
                this._thisProgressLevel = _savedData.ProgressLevel;
            }
        }

        public void SelectRandomEncounterFile()
        {
            ShuffleList();

            int i;

            for (i = 0; i < _unusedEncounterFileList.Count; i++)
            {
                if (_unusedEncounterFileList[i].ProgressLevel == _thisProgressLevel)
                {
                    _encounterFile = _unusedEncounterFileList[i];
                    _encounterFileIndex = i;
                    return;
                }
            }

            if (i == _unusedEncounterFileList.Count)
            {
                Debug.LogError("No more encounter file that match with this progress level at EncounterManager.cs");
            }
        }

        IEnumerator PrintEncounter()
        {
            //인카운터 파일 내 리스트에 요소가 없을 경우(special encounter만 저장하고 싶을 경우)
            if (_encounterFile.ItemList == null)
            {
                CheckAndSaveSpecialEncounter();
                ConveyToUsedList();
                SaveData();
                yield return null;
            }

            CopyItems();

            CheckAndSaveSpecialEncounter();

            foreach (var item in _itemList)
            {
                CallPrintManager(item);

                yield return new WaitUntil(() => _printManager.isPrintDone == true);
                yield return null;

                _printManager.isPrintDone = false;
                yield return null;
            }

            yield return null;
        }

        //선택지에서 골랐을 시
        public void TakeAChoice(int index)
        {
            ConveyToUsedList();

            switch(_choiceItemList[index].EventType)
            {
                case EventType.Encounter:
                    _encounterFile = _choiceItemList[index].encounterFile;
                    CopyItems();
                    ConnectEncounter();

                    break;

                case EventType.Heal:
                    _thisProgressLevel += 1;


                    break;

                case EventType.UpgradeArmor:
                    _thisProgressLevel += 1;

                    break;

                case EventType.Enchant:
                    _thisProgressLevel += 1;


                    break;
            }


            SelectRandomEncounterFile();
            CopyItems();
            SkipEncounter();

            SaveData();
        }

        /// <summary>
        /// SpecialEncounter가 있는지 확인하고 저장
        /// </summary>
        private void CheckAndSaveSpecialEncounter()
        {
            bool CheckIsHavingSpecialEncounter()
            {
                return this._encounterFile.SpecialEncounterFile != null;
            }

            if (CheckIsHavingSpecialEncounter())
            {
                _encounterFileListForAcquiredFiles.SaveToAcquiredFileList(this._encounterFile.SpecialEncounterFile);

                //리스트에 중복되는 요소 제거
                _encounterFileListForAcquiredFiles.AcquiredEncounterFileList =
                    _encounterFileListForAcquiredFiles.AcquiredEncounterFileList.Distinct().ToList();
            }
        }

        private void SkipEncounter()
        {
            _printManager.ReturnObjects();
            StartCoroutine(PrintEncounter());
        }

        private void ConnectEncounter()
        {
            _printManager.ReturnChoiceObjects();
            StartCoroutine(PrintEncounter());
        }

        private void CopyItems()
        {
            this._itemList = new List<Item>(_encounterFile.ItemList);
        }

        private void CallPrintManager(Item item)
        {
            switch (item.ItemType)
            {
                case ItemType.Text:
                    TextItem textItem = item as TextItem;
                    string text = textItem.text;

                    _printManager.PrintContent(text);
                    break;

                case ItemType.Sprite:
                    SpriteItem spriteItem = item as SpriteItem;
                    Sprite sprite = spriteItem.sprite;

                    _printManager.PrintContent(sprite);
                    break;

                case ItemType.Choice:
                    ChoiceItem choiceItem = item as ChoiceItem;
                    _choiceItemList = choiceItem.choiceList;

                    _printManager.PrintContent(_choiceItemList);
                    break;
            }


            Debug.Log("Print " + item.ItemType);
        }

        private static System.Random random = new System.Random();
        private void ShuffleList()
        {
            //Fisher Yates algorithm (Knuth Shuffle)
            //리스트 길이를 하나씩 줄여가며 그 안에서 랜덤한 요소와 리스트의 끝에 있는 요소를 바꾼다.

            int listLength = _unusedEncounterFileList.Count;

            int n = _unusedEncounterFileList.Count;
            while (n > 1)
            {
                n--;
                int k = random.Next(n + 1);
                EncounterFile value = _unusedEncounterFileList[k];
                _unusedEncounterFileList[k] = _unusedEncounterFileList[n];
                _unusedEncounterFileList[n] = value;
            }
        }

        private void ConveyToUsedList()
        {
            _usedEncounterFileList.Add(_unusedEncounterFileList[_encounterFileIndex]);
            _unusedEncounterFileList.RemoveAt(_encounterFileIndex);
        }

        private void SaveData()
        {
            _savedData.UnusedEncounterFiles = new List<EncounterFile>(_unusedEncounterFileList);
            _savedData.UsedEncounterFiles = new List<EncounterFile>(_usedEncounterFileList);
            _savedData.ProgressLevel = this._thisProgressLevel;

            EditorUtility.SetDirty(_savedData);

            _encounterFileListForAcquiredFiles.SaveData();
        }
    }
}