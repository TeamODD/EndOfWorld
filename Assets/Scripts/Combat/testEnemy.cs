using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class testEnemy : Enemy
{
    public override SkillSO Attack(int distance)
    {
        if (lastAttack != null && lastAttack.ISLINK)
        {
            SkillSO skill = linkAttackSkillList.Find(skill => skill.SKILLNAME == lastAttack.LINKATTACK);
            return lastAttack = skill;
        }

        switch(distance)
        {
            case 1: return lastAttack = distanceOneSkillList[getRandomSkillIndex(distanceOneSkillList)];
            case 2: return lastAttack = distanceTwoSkillList[getRandomSkillIndex(distanceTwoSkillList)];
            default: return null;
        }
    }
}
