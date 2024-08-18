using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "skill", menuName = "skillScriptable/CreateSkillData", order = 2)]

public class SkillSO : ScriptableObject
{
    public enum SkillType
    {
        combatSkill,
        moveSkill,
        linkSkill
    }

    public enum SkillAttackType
    {
        None,
        Attack,
        Defense,
        Heal
    }

    public enum CoefficientType
    {
        HP,
        ATK,
        DEF,
        SPD
    }

    public enum Target
    {
        Player,
        Enemy
    }

    [SerializeField, Space(1), Header("스킬 이름")]
    private string skillname;
    public string SKILLNAME { get { return skillname; } }

    [SerializeField, Space(1), Header("스킬 타입")]
    private SkillType skilltype;
    public SkillType SKILLTYPE { get { return skilltype; } }

    [SerializeField, Space(1), Header("스킬 공격 타입")]
    private SkillAttackType skillattacktype;
    public SkillAttackType SKILLATTACKTYPE { get {return skillattacktype;} }

    [SerializeField, Space(1), Header("스킬 타겟")]
    private Target target;
    public Target TARGET { get { return target; } }

    [SerializeField, Space(1), Header("기술 적용 계수")]
    private CoefficientType coefficienttype;
    public CoefficientType COEFFICIENTTYPE { get { return coefficienttype; } }

    [SerializeField, Space(2), Header("최소 공격범위 거리")]
    private int minAttackRange;
    public int MINATTACKRANGE { get { return minAttackRange; } }

    [SerializeField, Space(1), Header("최대 공격범위 거리")]
    private int maxAttackRange;
    public int MAXATTACKRANGE { get { return maxAttackRange; } }

    [SerializeField, Space(1), Header("공격 횟수"), Range(0, 3)]
    private int numberOfAttack;
    public int NUMBEROFATTACK { get { return numberOfAttack; } }


    [SerializeField, Space(1), Header("데미지 비율")]
    private int damage;
    public int DAMAGE { get { return damage; } }

    [SerializeField, Space(1), Header("최소 사용 거리")]
    private int minDistance;
    public int MINDISTANCE { get { return minDistance; } }
    
    [SerializeField, Space(1), Header("최대 사용 거리")]
    private int maxDistance;
    public int MAXDISTANCE { get { return maxDistance; } }

    [SerializeField, Space(1), Header("이동거리")]
    private int move;
    public int MOVE { get { return move; } }

    [SerializeField, Space(1), Header("적용 버프 / 디버프")]
    private StatusEffetSO[] effect;
    public StatusEffetSO[] EFFECT { get { return effect; } }

    [SerializeField, Space(1), Header("연계기")]
    private SkillSO linkskill;
    public SkillSO LINKSKILL { get { return linkskill; } }

    [SerializeField, Space(2), Header("스킬 아이콘")]
    private Sprite skillicon;
    public Sprite SKILLICON { get { return skillicon; } }

    [SerializeField, Space(1), Header("최대 사용가능 횟수")]
    private int uses;
    public int USES { get { return uses; } }

    [SerializeField, Space(1), Header("쿨타임")]
    private int cooltime;
    public int COOLTIME { get { return cooltime; } }

    [SerializeField, Space(1), Header("사용 시 대사")]
    private string usingtext;
    public string USINGTEXT { get { return usingtext; } }

    [SerializeField, Space(1), Header("적중 시 대사")]
    private string hittext;
    public string HITTEXT { get { return hittext; } }

    [SerializeField, Space(1), Header("비 적중 시 대사")]
    private string misstext;
    public string MISSTEXT { get { return misstext; } }
}

