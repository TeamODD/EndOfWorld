using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Unity.VisualScripting;
using TMPro;

namespace EndOfWorld.EncounterSystem
{
    [System.Serializable]
    public class StatsImageData
    {
        public List<Sprite> MaxHPImage;

        public List<Sprite> AttackPointImage;

        public List<Sprite> DefensePointImage;

        public List<Sprite> SpeedPointImage;
    }


    public class EnchantManager : MonoBehaviour
    {
        private enum Stats
        {
            HP,
            ATK,
            DEF,
            DEX
        }

        private enum SkillType
        {
            Fire,
            Water,
            Earth,
            Wind
        }

        /// <summary>
        /// 인챈트 UI 오브젝트
        /// </summary>
        [Header("UI Objects")]

        [SerializeField]
        private Image _backgroundBlackScreen;

        [SerializeField]
        private Image _UIbackground;

        [SerializeField]
        private Image _choiceA;
        [SerializeField]
        private Image _choiceB;
        [SerializeField]
        private Image _choiceC;

        [SerializeField]
        private CanvasGroup _canvasGroup;

        [SerializeField]
        private StatsImageData _statsImageData;

        private TMP_Text _textA;
        private TMP_Text _textB;
        private TMP_Text _textC;


        /// <summary>
        /// 랜덤으로 뽑아서 PlayerData에 전달할 SkillSO 리스트
        /// </summary>
        [Header("SkillDB")]

        [SerializeField]
        private List<SkillDB> _fireSkills;

        [SerializeField]
        private List<SkillDB> _waterSkills;

        [SerializeField]
        private List<SkillDB> _earthSkills;

        [SerializeField]
        private List<SkillDB> _windSkills;



        private SkillDB _skillDB1;

        private SkillDB _skillDB2;

        /// <summary>
        /// 스크립트 내부 지역 필드
        /// </summary>

        [HideInInspector]
        public bool IsEnchantDone = false;

        private int[,] _statsAmount = new int[4, 3];

        private string _statsA;

        private int _amountA;

        private PlayerData _playerData;

        private SkillType _skillType;


        private void Start()
        {
            init();
        }

        private void init()
        {
            //초기값 설정
            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    if (i == 0)
                    {
                        _statsAmount[i, j] = (j + 1) * 5;
                    }

                    _statsAmount[i, j] = j + 1;
                }
            }

            _textA = _choiceA.gameObject.transform.GetChild(0).GetComponent<TMP_Text>();
            _textB = _choiceB.gameObject.transform.GetChild(0).GetComponent<TMP_Text>();
            _textC = _choiceC.gameObject.transform.GetChild(0).GetComponent<TMP_Text>();

            _playerData = GameObject.FindWithTag("PlayerData").GetComponent<PlayerData>();



            if (_canvasGroup.gameObject.activeSelf)
            {
                _canvasGroup.gameObject.SetActive(false);
            }
        }

        public void StartEnchantManager()
        {
            DrawRandomSkillType();
            DrawRandomStats();

            ///스킬 랜덤 선택 후 출력하는 함수 
            ///

            StartCoroutine(FadeOut());
        }


        private void DrawRandomSkillType()
        {
            int enumNum = Random.Range(0, 4);

            _skillType = (SkillType)enumNum;
        }

        private void DrawRandomStats()
        {
            int choiceA = Random.Range(0, 4);

            switch (choiceA)
            {
                case 0:
                    _statsA = "최대 체력";
                    break;
                case 1:
                    _statsA = "공격력";
                    break;
                case 2:
                    _statsA = "방어력";
                    break;
                case 3:
                    _statsA = "민첩성";
                    break;
            }

            int imageNum = Random.Range(0, 3);

            _amountA = _statsAmount[choiceA, imageNum];

            PrintAbilityImage(choiceA, imageNum);
        }

        private void PrintAbilityImage(int index, int imageNum)
        {
            switch (index)
            {
                case 0:
                    _choiceA.sprite = _statsImageData.MaxHPImage[imageNum];
                    break;
                case 1:
                    _choiceA.sprite = _statsImageData.AttackPointImage[imageNum];
                    break;
                case 2:
                    _choiceA.sprite = _statsImageData.DefensePointImage[imageNum];
                    break;
                case 3:
                    _choiceA.sprite = _statsImageData.SpeedPointImage[imageNum];
                    break;
            }
        }

        private void DrawRandomTwoSkills()
        {
            int randomNumber1;
            int randomNumber2;


            switch (_skillType)
            {
                case SkillType.Fire:
                    if (_fireSkills.Count == 0) break;

                    randomNumber1 = Random.Range(0, _fireSkills.Count + 1);
                    randomNumber2 = Random.Range(0, _fireSkills.Count + 1);
                    while (randomNumber2 == randomNumber1)
                    {
                        if (_fireSkills.Count < 2) break;

                        randomNumber2 = Random.Range(0, _fireSkills.Count + 1);
                    }

                    _skillDB1 = _fireSkills[randomNumber1];
                    _skillDB2 = _fireSkills[randomNumber2];
                    break;

                case SkillType.Water:
                    if (_waterSkills.Count == 0) break;

                    randomNumber1 = Random.Range(0, _waterSkills.Count + 1);
                    randomNumber2 = Random.Range(0, _waterSkills.Count + 1);
                    while (randomNumber2 == randomNumber1)
                    {
                        if (_waterSkills.Count < 2) break;

                        randomNumber2 = Random.Range(0, _waterSkills.Count + 1);
                    }

                    _skillDB1 = _waterSkills[randomNumber1];
                    _skillDB2 = _waterSkills[randomNumber2];
                    break;

                case SkillType.Earth:
                    if (_earthSkills.Count == 0) break;

                    randomNumber1 = Random.Range(0, _earthSkills.Count + 1);
                    randomNumber2 = Random.Range(0, _earthSkills.Count + 1);
                    while (randomNumber2 == randomNumber1)
                    {
                        if (_earthSkills.Count < 2) break;

                        randomNumber2 = Random.Range(0, _earthSkills.Count + 1);
                    }

                    _skillDB1 = _earthSkills[randomNumber1];
                    _skillDB2 = _earthSkills[randomNumber2];
                    break;

                case SkillType.Wind:
                    if (_windSkills.Count == 0) break;

                    randomNumber1 = Random.Range(0, _windSkills.Count + 1);
                    randomNumber2 = Random.Range(0, _windSkills.Count + 1);
                    while (randomNumber2 == randomNumber1)
                    {
                        if (_windSkills.Count < 2) break;

                        randomNumber2 = Random.Range(0, _windSkills.Count + 1);
                    }

                    _skillDB1 = _windSkills[randomNumber1];
                    _skillDB2 = _windSkills[randomNumber2];
                    break;
            }
        }

        private void PrintText()
        {
            if (_textA.color.a < 255)
                _textA.color = new Color(_textA.color.r, _textA.color.g, _textA.color.b, 255);

            _textA.text = _statsA + "+" + _amountA;

            ///

            if (_skillDB1 != null)
                _textB.text = _skillDB1.NAME;

            if (_textB.color.a < 255)
                _textB.color = new Color(_textB.color.r, _textB.color.g, _textB.color.b, 255);

            ///

            if (_skillDB2 != null)
                _textC.text = _skillDB2.NAME;

            if (_textC.color.a < 255)
                _textC.color = new Color(_textC.color.r, _textC.color.g, _textC.color.b, 255);
        }

        IEnumerator FadeIn()
        {
            float time = 1;

            while (time > 0)
            {
                _canvasGroup.alpha = time;
                time -= Time.deltaTime;
                yield return null;
            }

            _canvasGroup.alpha = 0;

            this.IsEnchantDone = true;
            yield return null;
        }

        IEnumerator FadeOut()
        {
            float time = 0;

            PrintText();

            while (time < 1)
            {
                _canvasGroup.alpha = time;
                time += Time.deltaTime;
                yield return null;
            }

            _canvasGroup.alpha = 1;

            yield return null;
        }


        public void SelectA()
        {
            switch (_statsA)
            {
                case "최대 체력":
                    _playerData.MaxHP += _amountA;
                    break;
                case "공격력":
                    _playerData.AttackPoint += _amountA;
                    break;
                case "방어력":
                    _playerData.DefencePoint += _amountA;
                    break;
                case "민첩성":
                    _playerData.SpeedPoint += _amountA;
                    break;
            }

            StartCoroutine(FadeIn());
        }

        public void SelectB()
        {
            if(_playerData.MoveSkill.Contains(_skillDB1) || _playerData.CombatSkill.Contains(_skillDB1))
            {
                return;
            }

            switch(_skillDB1.TYPE)
            {
                case SkillSO.SkillType.moveSkill:
                    _playerData.MoveSkill.Add(_skillDB1);
                    break;

                case SkillSO.SkillType.combatSkill:
                    _playerData.CombatSkill.Add(_skillDB1);
                    break;
            }

            StartCoroutine(FadeIn());
        }

        public void SelectC()
        {
            if (_playerData.MoveSkill.Contains(_skillDB2) || _playerData.CombatSkill.Contains(_skillDB2))
            {
                return;
            }

            switch (_skillDB2.TYPE)
            {
                case SkillSO.SkillType.moveSkill:
                    _playerData.MoveSkill.Add(_skillDB2);
                    break;

                case SkillSO.SkillType.combatSkill:
                    _playerData.CombatSkill.Add(_skillDB2);
                    break;
            }

            StartCoroutine(FadeIn());
        }
    }


}