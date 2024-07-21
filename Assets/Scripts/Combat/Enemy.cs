using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Enemy : StatSystem
{
    [SerializeField]
    protected List<SkillSO> distanceOneSkillList = new List<SkillSO>();

    [SerializeField]
    protected List<SkillSO> distanceTwoSkillList = new List<SkillSO>();

    [SerializeField]
    protected List<SkillSO> distanceThreeSkillList = new List<SkillSO>();

    [SerializeField]
    protected List<SkillSO> distanceFourSkillList = new List<SkillSO>();

    [SerializeField]
    protected List<SkillSO> distanceFiveSkillList = new List<SkillSO>();

    [SerializeField]
    protected List<SkillSO> linkAttackSkillList = new List<SkillSO>();

    protected SkillSO lastAttack = null;

    public abstract SkillSO Attack(int distance);

    protected int getRandomSkillIndex(List<SkillSO> skillList)
    {
        int skillIndex;

        //최초 스킬 시전
        if (lastAttack == null) return skillIndex = Random.Range(0, skillList.Count);

        //최소 스킬 시전 이후
        if (skillList.Count <= 1) skillIndex = 0;

        else
        {
            do
            {
                skillIndex = Random.Range(0, skillList.Count);
            }
            while (skillList[skillIndex].SKILLNAME == lastAttack.SKILLNAME);

        }

        return skillIndex;
    }
}
