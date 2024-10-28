using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Unity.VisualScripting;

public class CombatHUDManager : MonoBehaviour
{
    [Space(1), Header("공통")]
    [SerializeField] private Slider hpSlider;
    [SerializeField] private TMP_Text hpText;
    
    [Space(1), Header("몬스터 전용")]
    [SerializeField] private TMP_Text monsterName;
    [SerializeField] private GameObject enemyImage;

    [Space(1), Header("플레이어 전용")]
    [SerializeField] private Button skillButton;
    [SerializeField] private GameObject CombatSkillParent;
    [SerializeField] private GameObject MoveSkillParent;

    //전투스킬과 이동스킬로 나누고 cc 걸린거에 따라 그거 전체를 비활성화 시키자
    private List<Button> combatSkillButtons = new List<Button>();
    private List<Button> moveSkillButtons = new List<Button>();

    public void SetHUD(StatSystem unit)
    {
        hpSlider.maxValue = unit.maxHitPoint;
        hpSlider.value = unit.currentHitPoint;
        hpText.text = unit.currentHitPoint + " / " + unit.maxHitPoint;
    }

    public void SetEnemyName(Enemy enemy)
    {
        monsterName.text = enemy.enemyName;
        enemyImage.GetComponent<Image>().sprite = enemy.enemySprite;
    }

    public void SetHPSlider(int currentHP)
    {
        hpSlider.value = currentHP;
    }

    public void SetSkillButton(List<SkillDB> skills)
    {
        int index = 0;

        foreach (SkillDB skill in skills)
        {
            Button button = Instantiate(skillButton);
            SkillButtonInfo info = button.GetComponent<SkillButtonInfo>();
            if(skill.TYPE == SkillSO.SkillType.combatSkill)
            {
                combatSkillButtons.Add(button);
                button.transform.SetParent(CombatSkillParent.transform);
            }

            else if(skill.TYPE == SkillSO.SkillType.moveSkill)
            {
                moveSkillButtons.Add(button);
                button.transform.SetParent(MoveSkillParent.transform);
            }

            info.InfoInit(index++, skill);
            button.onClick.AddListener(CombatSystemManager.Instance.OnSkillButton);
        }
    }

    //상태이상에 따라 사용 가능한지 안한지도 추가해야한다.
    public void SetButtonActivated(int distance, Player player)
    {
        if (player.isFrightened || player.isParalysus)
        {
            foreach(Button skill in combatSkillButtons) { skill.enabled = false; }
        }

        else
        {
            skillInteractableCheck(distance, combatSkillButtons);
        }

        if (player.isEnsnared || player.isParalysus)
        {
            foreach (Button skill in moveSkillButtons) { skill.enabled = false; }
        }

        else
        {
            skillInteractableCheck(distance, moveSkillButtons);
        }

    }

    private void skillInteractableCheck(int distance, List<Button> list)
    {
        foreach (Button skill in list)
        {
            if (skill.GetComponent<SkillButtonInfo>().skill.USES < 1 ||
                skill.GetComponent<SkillButtonInfo>().skill.COOLTIME > 0 ||
                skill.GetComponent<SkillButtonInfo>().skill.MINDISTANCE > distance ||
                skill.GetComponent<SkillButtonInfo>().skill.MAXDISTANCE < distance) skill.interactable = false;

            else skill.interactable = true;
        }
    }

    //이부분 고칠 예정 sprite는 1개며 scale 조정 방식으로 교체
    public void SetEnemySprite(Enemy enemy, int distance)
    {
        if (distance <= 0) distance = 1;
        if (distance > 5) distance = 5;

        enemyImage.GetComponent<Image>().transform.localScale = new Vector3(1.3f - 0.15f * distance, 1.3f - 0.15f * distance, 1f);
        enemyImage.GetComponent<Image>().SetNativeSize();
    }


    //상태이상 표시
    public void SetCharaterStatus(StatSystem unit)
    {
        if (unit == null) return;

        //매번 턴이 올때마다 체크해야할거 같다.
        //턴이 오면 상태이상 오브젝트 들어가서 자식 오브젝트 모두 삭제 후 다시 현재 상태이상에 따라 생성해서 부모오브젝트에 붙히기
        //좌우 스크롤이 되도록 해야하나? 그것도 물어봐야겠다.

        if (unit.isEnsnared) { }
        if (unit.isFrightened) { }
        if (unit.isParalysus) { }

        foreach(EffectDB effect in unit.effectList)
        {
            //아이콘만 가져와서 생성후 보여주기 생성하기 전 저번에 있었던 것들은 전부 삭제
        }
    }
}
