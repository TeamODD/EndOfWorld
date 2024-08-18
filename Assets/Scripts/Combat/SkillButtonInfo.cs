using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SkillButtonInfo : MonoBehaviour
{
    public int skillIndex { get; private set; }
    public SkillDB skill { get; private set; }

    [SerializeField]
    private TMP_Text skillName;
    [SerializeField]
    private TMP_Text skillUses;


    public void InfoInit(int index, SkillDB _skill)
    {
        skillIndex = index;
        skill = _skill;
        skillName.text = skill.NAME;
        setUsesText();
    }

    private void setUsesText()
    {
        skillUses.text = "" + skill.USES;
    }

    public void UsedSkillSet(SkillDB _skill)
    {
        skill = _skill;
        setUsesText();
    }
    //���Ƚ���� �� ������ �� Ȥ�� ���� ��Ÿ�� ���� �ش� ��ư�� ��Ȱ��ȭ(Ŭ�� ���ϵ���) ��Ű�� �Լ� ����
}
