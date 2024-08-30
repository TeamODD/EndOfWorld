using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : StatSystem
{
    //�ӽ� ��ų �ִ¹��
    public void setSkill(SkillSO skill) { ApplySkill(skill); }
    
    private void RemoveAllUsedSkill()
    {
        foreach(SkillDB skillDB in combatSkillList)
        {
            if (skillDB.USES < 1) combatSkillList.Remove(skillDB);
        }

        foreach(SkillDB skillDB in moveSkillList)
        {
            if (skillDB.USES < 1) moveSkillList.Remove(skillDB);
        }
    }

}
