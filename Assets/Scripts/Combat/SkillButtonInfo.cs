using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

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
        this.GetComponent<Image>().sprite = skill.ICON;
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
    //사용횟수가 다 떨어질 시 혹은 현재 쿨타임 동안 해당 버튼을 비활성화(클릭 못하도록) 시키는 함수 예정
}
