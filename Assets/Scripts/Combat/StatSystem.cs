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

    //기본 스탯 저장하는 함수(레벨 데이터 표가 존재한다면 일정 렙마다 값을 넣는 방식으로 레벨업 가능)
    public void InitStat(int maxHP, int ATK, int DEF, int SPD)
    {
        maxHitPoint = maxHP;
        attackPoint = ATK;
        defensePoint = DEF;
        speed = SPD;
    }

    //전투 시작 시 스탯을 초기화 하는 함수 / 전투시작 시 단 1번만 호출
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

    //턴 시작시 기본 공격력, 기본 방어력으로 초기화 하는 함수 / 자신의 턴 마다 호출
    public void TurnResetStat()
    {
        currentAttackPoint = attackPoint;
        currentDefensePoint = defensePoint;
        currentShieldPoint = 0;

        isFrightened = false;
        isEnsnared = false;
        isParalysus = false;
    }

    //쿨타임 관리하는 함수 작성하기
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

    //List에 저장된 버프,디버프 들을 적용시키는 함수 / 턴 시작 시 호출
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

    //ScriptableObject로 작성된 버프,디버프 asset을 인수로 받는 함수 / 버프, 디버프를 받을 때마다 호출
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