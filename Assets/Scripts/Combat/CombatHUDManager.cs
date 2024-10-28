using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Unity.VisualScripting;

public class CombatHUDManager : MonoBehaviour
{
    [Space(1), Header("����")]
    [SerializeField] private Slider hpSlider;
    [SerializeField] private TMP_Text hpText;
    
    [Space(1), Header("���� ����")]
    [SerializeField] private TMP_Text monsterName;
    [SerializeField] private GameObject enemyImage;

    [Space(1), Header("�÷��̾� ����")]
    [SerializeField] private Button skillButton;
    [SerializeField] private GameObject CombatSkillParent;
    [SerializeField] private GameObject MoveSkillParent;

    //������ų�� �̵���ų�� ������ cc �ɸ��ſ� ���� �װ� ��ü�� ��Ȱ��ȭ ��Ű��
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

    //�����̻� ���� ��� �������� �������� �߰��ؾ��Ѵ�.
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

    //�̺κ� ��ĥ ���� sprite�� 1���� scale ���� ������� ��ü
    public void SetEnemySprite(Enemy enemy, int distance)
    {
        if (distance <= 0) distance = 1;
        if (distance > 5) distance = 5;

        enemyImage.GetComponent<Image>().transform.localScale = new Vector3(1.3f - 0.15f * distance, 1.3f - 0.15f * distance, 1f);
        enemyImage.GetComponent<Image>().SetNativeSize();
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
