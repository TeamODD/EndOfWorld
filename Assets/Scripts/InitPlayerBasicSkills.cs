using System.Collections.Generic;
using UnityEngine;

public class InitPlayerBasicSkills : MonoBehaviour
{
    public PlayerData _playerData;

    public List<SkillSO> SkillSO;

    void Start()
    {
        for (int i = 0; i < SkillSO.Count; i++)
        {
            ApplySkill(SkillSO[i]);
        }
    }

    private void ApplySkill(SkillSO skill)
    {

        SkillDB skillDB = new SkillDB(
            skill.SKILLNAME,
            skill.SKILLTYPE,
            skill.SKILLATTACKTYPE,
            skill.TARGET,
            skill.COEFFICIENTTYPE,
            skill.MINATTACKRANGE,
            skill.MAXATTACKRANGE,
            skill.NUMBEROFATTACK,
            skill.DAMAGE,
            skill.MINDISTANCE,
            skill.MAXDISTANCE,
            skill.MOVE,
            skill.EFFECT,
            skill.LINKSKILL,
            skill.SKILLICON,
            skill.USES,
            skill.COOLTIME,
            skill.USINGTEXT,
            skill.HITTEXT,
            skill.MISSTEXT
            );

        if (skillDB.TYPE == global::SkillSO.SkillType.combatSkill) _playerData.CombatSkill.Add(skillDB);
        else if (skillDB.TYPE == global::SkillSO.SkillType.moveSkill) _playerData.MoveSkill.Add(skillDB);
    }
}