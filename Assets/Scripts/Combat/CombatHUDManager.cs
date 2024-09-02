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

    //�����̻� ǥ��
    public void SetCharaterStatus(StatSystem unit)
    {
        if (unit == null) return;

        //�Ź� ���� �ö����� üũ�ؾ��Ұ� ����.
        //���� ���� �����̻� ������Ʈ ���� �ڽ� ������Ʈ ��� ���� �� �ٽ� ���� �����̻� ���� �����ؼ� �θ������Ʈ�� ������
        //�¿� ��ũ���� �ǵ��� �ؾ��ϳ�? �װ͵� ������߰ڴ�.

        if (unit.isEnsnared) { }
        if (unit.isFrightened) { }
        if (unit.isParalysus) { }

        foreach(EffectDB effect in unit.effectList)
        {
            //�����ܸ� �����ͼ� ������ �����ֱ� �����ϱ� �� ������ �־��� �͵��� ���� ����
        }
    }
}
