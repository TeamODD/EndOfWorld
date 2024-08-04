using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SkillButtonInfo : MonoBehaviour
{
    private int skillIndex;
    [SerializeField]
    private TMP_Text skillName;
    [SerializeField]
    private TMP_Text skillUses;

    public void InfoInit(int index, string skillname, int skilluses)
    {
        skillIndex = index;
        skillName.text = skillname;
        skillUses.text = "" + skilluses;
    }

    public int getSkillIndex()
    {
        return skillIndex;
    }

    //사용횟수가 다 떨어질 시 혹은 현재 쿨타임 동안 해당 버튼을 비활성화(클릭 못하도록) 시키는 함수 예정
}
