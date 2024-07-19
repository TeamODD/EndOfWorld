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
}
