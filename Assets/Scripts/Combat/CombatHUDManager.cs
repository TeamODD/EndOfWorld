using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CombatHUDManager : MonoBehaviour
{
    [SerializeField] private TMP_Text ATK_Text;
    [SerializeField] private TMP_Text DEF_Text;
    [SerializeField] private Slider hpSlider;
    [SerializeField] private Button skillButton;
    [SerializeField] private GameObject CombatSkillParent;
    [SerializeField] private GameObject MoveSkillParent;

    private List<Button> skillButtons = new List<Button>();

    public void SetHUD(StatSystem unit)
    {
        ATK_Text.text = "" + unit.currentAttackPoint;
        DEF_Text.text = "" + unit.currentDefensePoint;
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
    //거리에 따른 적 이미지 크기 조절 함수 만들기

}
