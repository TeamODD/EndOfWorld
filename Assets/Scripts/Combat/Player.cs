using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : StatSystem
{

    //임시 스킬 넣는방법
    public void setSkill(SkillSO skill) { ApplySkill(skill); }
    public List<SkillDB> GetCombatSkillList() { return combatSkillList; }
    public List<SkillDB> GetMoveSkillList() { return moveSkillList; }
    public void SetSkillList(List<SkillDB> _combatSkillList, List<SkillDB> _mobeSkillList) 
    {
        combatSkillList = _combatSkillList;
        moveSkillList = _mobeSkillList;
        RemoveAllUsedSkill();
    }
    
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
