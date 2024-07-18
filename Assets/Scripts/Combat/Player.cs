using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : StatSystem
{
    private List<SkillDB> skillList = new List<SkillDB>();

    public void ApplySkill(SkillSO skill)
    {
        SkillDB skillDB = new SkillDB(
            skill.SKILLNAME, 
            skill.DAMAGE, 
            skill.DISTANCE, 
            skill.EFFECT, 
            skill.MOVE, 
            skill.SKILLICON, 
            skill.USES
            );

        skillList.Add(skillDB);
    }

    public List<SkillDB> GetSkillList() { return skillList; }
}