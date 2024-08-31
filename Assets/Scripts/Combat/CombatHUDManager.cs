using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CombatHUDManager : MonoBehaviour
{
    [SerializeField] private TMP_Text monsterName;
    [SerializeField] private Slider hpSlider;
    [SerializeField] private Button skillButton;
    [SerializeField] private GameObject CombatSkillParent;
    [SerializeField] private GameObject MoveSkillParent;

    private List<Button> skillButtons = new List<Button>();

    public void SetHUD(StatSystem unit)
    {
        if(monsterName != null) monsterName.text = unit.name;
        hpSlider.maxValue = unit.maxHitPoint;
        hpSlider.value = unit.currentHitPoint;
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
            skillButtons.Add(button);
            SkillButtonInfo info = button.GetComponent<SkillButtonInfo>();
            if(skill.TYPE == SkillSO.SkillType.combatSkill)
            {
                button.transform.SetParent(CombatSkillParent.transform);
            }

            else if(skill.TYPE == SkillSO.SkillType.moveSkill)
            {
                button.transform.SetParent(MoveSkillParent.transform);
            }

            info.InfoInit(index++, skill);
            button.onClick.AddListener(CombatSystemManager.Instance.OnSkillButton);
        }
    }

    public void SetButtonActivated(int distance)
    {
        foreach(Button skill in  skillButtons)
        {
            if (skill.GetComponent<SkillButtonInfo>().skill.USES < 1 || 
                skill.GetComponent<SkillButtonInfo>().skill.COOLTIME > 0 ||
                skill.GetComponent<SkillButtonInfo>().skill.MINDISTANCE > distance ||
                skill.GetComponent<SkillButtonInfo>().skill.MAXDISTANCE < distance) skill.interactable = false;

            else skill.interactable = true;
        }
    }

    public void SetEnemySprite(Enemy enemy, int distance)
    {
        enemy.GetComponent<SpriteRenderer>().sprite = enemy.enemySprites[distance - 1];
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
