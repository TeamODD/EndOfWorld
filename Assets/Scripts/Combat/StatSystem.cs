using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class StatSystem : MonoBehaviour
{
    public int maxHitPoint { get; private set; }
    public int attackPoint { get; private set; }
    public int defensePoint { get; private set; }
    public int speed { get; private set; }

    public int currentHitPoint { get; private set; }
    public int currentAttackPoint { get; private set; }
    public int currentDefensePoint { get; private set; }
    public int currentShieldPoint { get; private set; }
    public bool isFrightened { get; private set; }
    public bool isEnsnared { get; private set; }
    public bool isParalysus { get; private set; }

    private List<EffectDB> effectList = new List<EffectDB>();
    protected List<SkillDB> combatSkillList = new List<SkillDB>();
    protected List<SkillDB> moveSkillList = new List<SkillDB>();
    protected List<SkillDB> linkSkillList = new List<SkillDB>();

    protected void ApplySkill(SkillSO skill)
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
            skill.TEXT
            );

        if (skillDB.TYPE == SkillSO.SkillType.combatSkill) combatSkillList.Add(skillDB);
        else if(skillDB.TYPE == SkillSO.SkillType.moveSkill) moveSkillList.Add(skillDB);
        else linkSkillList.Add(skillDB);
    }

    //�⺻ ���� �����ϴ� �Լ�(���� ������ ǥ�� �����Ѵٸ� ���� ������ ���� �ִ� ������� ������ ����)
    public void InitStat(int maxHP, int ATK, int DEF, int SPD)
    {
        maxHitPoint = maxHP;
        attackPoint = ATK;
        defensePoint = DEF;
        speed = SPD;
    }

    //���� ���� �� ������ �ʱ�ȭ �ϴ� �Լ� / �������� �� �� 1���� ȣ��
    public void BattleStartStat()
    {
        currentHitPoint = maxHitPoint;
        currentAttackPoint = attackPoint;
        currentDefensePoint = defensePoint;
        currentShieldPoint = 0;

        isFrightened = false;
        isEnsnared = false;
        isParalysus = false;
    }

    //�� ���۽� �⺻ ���ݷ�, �⺻ �������� �ʱ�ȭ �ϴ� �Լ� / �ڽ��� �� ���� ȣ��
    public void TurnResetStat()
    {
        currentAttackPoint = attackPoint;
        currentDefensePoint = defensePoint;
        currentShieldPoint = 0;

        isFrightened = false;
        isEnsnared = false;
        isParalysus = false;
    }

    //��Ÿ�� �����ϴ� �Լ� �ۼ��ϱ�
    public void CoolTimeSet()
    {
        foreach(SkillDB skill in combatSkillList)
        {
            if (skill.COOLTIME > 0) --skill.COOLTIME;
        }

        foreach(SkillDB skill in moveSkillList)
        {
            if (skill.COOLTIME > 0) --skill.COOLTIME;
        }
    }

    //List�� ����� ����,����� ���� �����Ű�� �Լ� / �� ���� �� ȣ��
    public void ActivateEffect()
    {
        if (effectList.Count < 1) return;

        for(int i =0; i < effectList.Count; i++) {
            EffectDB db = effectList[i];

            if (currentDefensePoint + db.HP > maxHitPoint) currentHitPoint = maxHitPoint;
            else currentHitPoint += db.HP;
            currentAttackPoint += db.ATK;
            currentDefensePoint += db.DEF;
            
            switch(db.MEZE)
            {
                case StatusEffetSO.Meze.None: break;
                case StatusEffetSO.Meze.Frightened: isFrightened = true; break;
                case StatusEffetSO.Meze.Ensnared: isEnsnared = true; break;
                case StatusEffetSO.Meze.Paralysis: isParalysus = true; break;
            }

            if (db.DURATION < 1) effectList.Remove(db);
        }

    }

    public void DicreaseEffectDuration()
    {
        for(int i =0; i< effectList.Count; i++)
        {
            EffectDB db = effectList[i];
            if (--db.DURATION < 1) effectList.Remove(db);

        }
    }

    //ScriptableObject�� �ۼ��� ����,����� asset�� �μ��� �޴� �Լ� / ����, ������� ���� ������ ȣ��
    public void ApplyEffect(StatusEffetSO statusEffect)
    {
        EffectDB effectDB = new EffectDB(
            statusEffect.EFFECTNAME, 
            statusEffect.TARGET,
            statusEffect.HP, 
            statusEffect.ATTACK, 
            statusEffect.DEFENSE,
            statusEffect.MEZE,
            statusEffect.DURATION
            );

        effectList.Add(effectDB);
    }

    public int CombatSkillActivate(SkillDB skill)
    {
        int coefficient = GetCoefficient(skill);
        int skillDamage = (int)((float)skill.DAMAGE / 100 * coefficient);
        switch(skill.ATTACKTYPE)
        {
            case SkillSO.SkillAttackType.Attack:
                bool isNoDamage = currentShieldPoint >= skillDamage ? true : false;
                if(!isNoDamage)
                {
                    currentHitPoint -= (skillDamage - currentShieldPoint);
                    return (skillDamage - currentShieldPoint);
                }
                else return 0;

            case SkillSO.SkillAttackType.Defense:
                currentShieldPoint = skillDamage;
                return skillDamage;

            case SkillSO.SkillAttackType.Heal:
                if (currentHitPoint + skillDamage > maxHitPoint) currentHitPoint = maxHitPoint;
                else currentHitPoint += skillDamage;
                return skillDamage;

            case SkillSO.SkillAttackType.None: break;
        }

        return 0;
    }

    private int GetCoefficient(SkillDB skill)
    {
        switch(skill.COEFFICIENTTYPE)
        {
            case SkillSO.CoefficientType.HP: return maxHitPoint;
            case SkillSO.CoefficientType.ATK: return currentAttackPoint;
            case SkillSO.CoefficientType.DEF: return currentDefensePoint;
            case SkillSO.CoefficientType.SPD: return speed;
        }

        return 0;
    }
}