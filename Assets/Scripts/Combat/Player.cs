using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : StatSystem
{

    public void setPlayerSkillList(List<SkillDB> combat, List<SkillDB> move)
    {
        combatSkillList = combat;
        moveSkillList = move;
    }

    public List<SkillDB> getCombatSkillList() { return combatSkillList; }

    public List<SkillDB> getMoveSkillList() { return moveSkillList; }


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
