using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : StatSystem
{
    public void setSkill(List<SkillSO> skill)
    {
        foreach(SkillSO s in skill)
        {
            ApplySkill(s);
        }
    }
    
    public void RemoveAllUsedSkill()
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
