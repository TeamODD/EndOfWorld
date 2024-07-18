using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Enemy : StatSystem
{
    [SerializeField]
    protected List<SkillSO> skillList = new List<SkillSO>();
    [SerializeField]
    protected Sprite enemySprite;

    protected SkillSO lastAttack;

    public abstract SkillSO Attack(int distance);
}
