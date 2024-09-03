using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEditor;
using UnityEngine.UI;


namespace EndOfWorld.EncounterSystem
{
    public class EncounterManager : MonoBehaviour
    {
        public static EncounterManager Instance { get; private set; }
        [SerializeField]
        private short _chaperIndex;

        [SerializeField]
        private List<EncounterFile> _unusedEncounterFileList;

        [SerializeField]
        private List<EncounterFile> _usedEncounterFileList = new List<EncounterFile>();
        public SavedData _savedData;

        [SerializeField]
        private EncounterFileListForAcquiredFiles _encounterFileListForAcquiredFiles;

        [SerializeField]
        private PlayerData _playerData;

        [SerializeField]
        private VerticalLayoutGroup _verticalLayoutGroup;

        [SerializeField]
        private SceneTransitionManager _sceneTransitionManager;

        private EncounterFile _encounterFile;

        private PrintManager _printManager;

        private List<Item> _itemList;

        private List<ChoiceContents> _choiceItemList;

        private int _thisProgressLevel = 1;

        private int _encounterFileIndex;

        private EnchantManager _enchantManager;

        [SerializeField]
        private Canvas _enchantUICanvas;

        private bool IsConneting = false;

        private bool _isWaitingPrint = true;

        [HideInInspector]
        public bool _isCombatEnd = false;

        [HideInInspector]
        public CombatResult CombatResult;

        private void Awake()
        {
            _printManager = GameObject.FindWithTag("PrintManager").GetComponent<PrintManager>();

            _enchantManager = GameObject.FindWithTag("EnchantManager").GetComponent<EnchantManager>();

            if (_playerData == null)
                _playerData = GameObject.FindWithTag("PlayerData").GetComponent<PlayerData>();

            _sceneTransitionManager.OnCombatEnd.AddListener(GetCombatResult);
        }

        private void Start()
        {
            BringSavedData();
            BringAcquiredFiles();


            SelectRandomEncounterFile();
            StartCoroutine(PrintEncounter(true));
        }

        private void Update()
        {
            //정렬 버그 fix를 위한 코드
            _verticalLayoutGroup.enabled = false;
            _verticalLayoutGroup.enabled = true;
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
            if (_savedData.ProgressLevel > 1) //저장 된적이 있는지 없는지 구분
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

        IEnumerator PrintEncounter(bool IsNeedToWait)
        {
            CopyItems();

            //오브젝트를 지울 때 걸리는 시간을 기다려주기 위함
            if (IsNeedToWait)
                yield return new WaitForSeconds(1f);


            foreach (var item in _itemList)
            {
                switch (item.ItemType)
                {
                    case ItemType.Text:
                    case ItemType.Sprite:
                    case ItemType.Choice:

                        CallPrintManager(item);

                        _isWaitingPrint = true;
                        yield return new WaitUntil(() => _printManager.isPrintDone == true);
                        yield return null;

                        _printManager.isPrintDone = false;
                        _isWaitingPrint = false;
                        yield return null;

                        break;

                    case ItemType.Encounter:
                        GameObject _enemy = ((EncounterItem)item).Enemy;
                        //_sceneTransitionManager.EnemyData = _enemy;
                        _sceneTransitionManager.LoadCombatScene(_enemy);

                        //전투가 끝날 때까지 대기
                        yield return new WaitUntil(() => this._isCombatEnd == true);
                        yield return null;

                        this._isCombatEnd = false;

                        for(int i = 0; i < 3; i++)
                        {
                            if( ((EncounterItem)item).CombatResultReportList[i].combatResult == this.CombatResult )
                            {
                                this._encounterFile = ((EncounterItem)item).CombatResultReportList[i].encounterFile;

                                ConnectEncounter();
                            }
                        }

                        break;

                    case ItemType.SetHP:
                        ((AddHPItem)item).AddHpPoint(((AddHPItem)item).HpPoint);
                        break;

                    case ItemType.UpgradeArmor:
                        ((UpgradeArmorItem)item).AddDefensePoint(((UpgradeArmorItem)item).DefensePoint);
                        break;

                    case ItemType.Enchant:
                        _enchantUICanvas.gameObject.SetActive(true);
                        _enchantManager.StartEnchantManager();

                        yield return new WaitUntil(() => _enchantManager.IsEnchantDone == true);
                        yield return null;

                        _enchantManager.IsEnchantDone = false;
                        _enchantUICanvas.gameObject.SetActive(false);
                        yield return null;

                        break;

                    case ItemType.SpecialEncounter:
                        SaveSpecialEncounter( (SpecialEncounterItem)item );
                        break;

                    case ItemType.ProgressLevel:
                        this._thisProgressLevel += ((AddProgressLevel)item).ProgressLevelRisingCount;
                        break;

                    case ItemType.SkipEncounterItem:
                        ConveyToUsedList();

                        SelectRandomEncounterFile();
                        CopyItems();
                        SkipEncounter();
                        break;

                    case ItemType.StatIncreaseItem:

                        switch( ((AddStatIncreaseItem)item).StatType )
                        {
                            case StatType.ATK:
                                _playerData.AttackPoint += ((AddStatIncreaseItem)item).StatPoint;
                                break;

                            case StatType.DEF:
                                _playerData.DefencePoint += ((AddStatIncreaseItem)item).StatPoint;
                                break;

                            case StatType.DEX:
                                _playerData.SpeedPoint += ((AddStatIncreaseItem)item).StatPoint;
                                break;
                        }
                        break;


                    case ItemType.SkillItem:
                        SkillSO skill = ((AddSkillItem)item).Skill;
                        _playerData.ApplySkill(skill);
                        break;

                }

            }

            yield return null;
        }

        //선택지에서 골랐을 시
        public void TakeAChoice(int index)
        {
            //페이드 효과가 끝나지 않았는 데 눌렀을시 반환
            if (_isWaitingPrint == true) return;


            //선택지가 다음 encounterFile이 아닌 팝업 오브젝트를 가지고 있다면
            if (_choiceItemList[index].PopupObject != null)
            {
                GameObject popupObject = GameObject.Instantiate(_choiceItemList[index].PopupObject) as GameObject;

                popupObject.transform.SetParent(_verticalLayoutGroup.gameObject.transform, false);

                return;
            } 
            //팝업 오브젝트의 삭제는 오브젝트 내에서 이루어지도록 하겠음


            ConveyToUsedList();


            if (_choiceItemList[index].encounterFile != null)
            {
                _encounterFile = _choiceItemList[index].encounterFile;
                IsConneting = true;

                CopyItems();
                ConnectEncounter();
            }
            else
            {
                IsConneting = false;

                SelectRandomEncounterFile();
                CopyItems();
                SkipEncounter();
            }

            SaveData();
        }

        /// <summary>
        /// SpecialEncounter가 있는지 확인하고 저장
        /// </summary>
        private void SaveSpecialEncounter(SpecialEncounterItem specialEncounterItem)
        {
            _encounterFileListForAcquiredFiles.SaveToAcquiredFileList(specialEncounterItem.SpecialEncounterFile);

            //리스트에 중복되는 요소 제거
            _encounterFileListForAcquiredFiles.AcquiredEncounterFileList =
                _encounterFileListForAcquiredFiles.AcquiredEncounterFileList.Distinct().ToList();
        }

        private void SkipEncounter()
        {
            _printManager.ReturnAllObjects();
            StartCoroutine(PrintEncounter(true));
        }

        private void ConnectEncounter()
        {
            _printManager.ReturnChoiceObjects();
            StartCoroutine(PrintEncounter(true));
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

        public void EndCombat()
        {
            this._isCombatEnd = true;
        }

        public void GetCombatResult(CombatResult combatResult)
        {
            this.CombatResult = combatResult;
        }

        private void ConveyToUsedList()
        {
            if (IsConneting) return;

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