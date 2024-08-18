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

    [SerializeField, Space(1), Header("��ų �̸�")]
    private string skillname;
    public string SKILLNAME { get { return skillname; } }

    [SerializeField, Space(1), Header("��ų Ÿ��")]
    private SkillType skilltype;
    public SkillType SKILLTYPE { get { return skilltype; } }

    [SerializeField, Space(1), Header("��ų ���� Ÿ��")]
    private SkillAttackType skillattacktype;
    public SkillAttackType SKILLATTACKTYPE { get {return skillattacktype;} }

    [SerializeField, Space(1), Header("��ų Ÿ��")]
    private Target target;
    public Target TARGET { get { return target; } }

    [SerializeField, Space(1), Header("��� ���� ���")]
    private CoefficientType coefficienttype;
    public CoefficientType COEFFICIENTTYPE { get { return coefficienttype; } }

    [SerializeField, Space(2), Header("�ּ� ���ݹ��� �Ÿ�")]
    private int minAttackRange;
    public int MINATTACKRANGE { get { return minAttackRange; } }

    [SerializeField, Space(1), Header("�ִ� ���ݹ��� �Ÿ�")]
    private int maxAttackRange;
    public int MAXATTACKRANGE { get { return maxAttackRange; } }

    [SerializeField, Space(1), Header("���� Ƚ��"), Range(0, 3)]
    private int numberOfAttack;
    public int NUMBEROFATTACK { get { return numberOfAttack; } }


    [SerializeField, Space(1), Header("������ ����")]
    private int damage;
    public int DAMAGE { get { return damage; } }

    [SerializeField, Space(1), Header("�ּ� ��� �Ÿ�")]
    private int minDistance;
    public int MINDISTANCE { get { return minDistance; } }
    
    [SerializeField, Space(1), Header("�ִ� ��� �Ÿ�")]
    private int maxDistance;
    public int MAXDISTANCE { get { return maxDistance; } }

    [SerializeField, Space(1), Header("�̵��Ÿ�")]
    private int move;
    public int MOVE { get { return move; } }

    [SerializeField, Space(1), Header("���� ���� / �����")]
    private StatusEffetSO[] effect;
    public StatusEffetSO[] EFFECT { get { return effect; } }

    [SerializeField, Space(1), Header("�����")]
    private SkillSO linkskill;
    public SkillSO LINKSKILL { get { return linkskill; } }

    [SerializeField, Space(2), Header("��ų ������")]
    private Sprite skillicon;
    public Sprite SKILLICON { get { return skillicon; } }

    [SerializeField, Space(1), Header("�ִ� ��밡�� Ƚ��")]
    private int uses;
    public int USES { get { return uses; } }

    [SerializeField, Space(1), Header("��Ÿ��")]
    private int cooltime;
    public int COOLTIME { get { return cooltime; } }

    [SerializeField, Space(1), Header("��� �� ���")]
    private string usingtext;
    public string USINGTEXT { get { return usingtext; } }

    [SerializeField, Space(1), Header("���� �� ���")]
    private string hittext;
    public string HITTEXT { get { return hittext; } }

    [SerializeField, Space(1), Header("�� ���� �� ���")]
    private string misstext;
    public string MISSTEXT { get { return misstext; } }
}

