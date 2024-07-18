using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class testEnemy : Enemy
{
    public override SkillSO Attack(int distance)
    {
        switch(distance)
        {
            case 1: return skillList[0];
            case 2: return skillList[1];
            default: return null;
        }
    }
}
