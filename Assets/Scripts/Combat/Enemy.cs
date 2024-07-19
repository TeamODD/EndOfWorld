using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Enemy : StatSystem
{
    [SerializeField]
    protected List<SkillSO> skillList = new List<SkillSO>();

    protected SkillSO lastAttack;

    public abstract SkillSO Attack(int distance);
}
