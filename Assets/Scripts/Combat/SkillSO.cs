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
        moveSkill
    }

    [SerializeField, Space(4), Header("��ų �̸�")]
    private string skillname;
    public string SKILLNAME { get { return skillname; } }

    [SerializeField, Space(2), Header("��ų Ÿ��")]
    private SkillType skilltype;
    public SkillType SKILLTYPE { get { return skilltype; } }

    [SerializeField, Space(2), Header("���� Ƚ��"), Range(0, 3)]
    private int numberOfAttack;
    public int NUMBEROFATTACK { get { return numberOfAttack; } }

    [SerializeField, Space(2), Header("������ ����")]
    private int damage;
    public int DAMAGE { get { return damage; } }

    [SerializeField, Space(2), Header("�����Ÿ�")]
    private int distance;
    public int DISTANCE { get { return distance; } }

    [SerializeField, Space(2), Header("���� / ����� ����Ʈ")]
    private StatusEffetSO[] effect;
    public StatusEffetSO[] EFFECT { get { return effect; } }

    [SerializeField, Space(2), Header("�̵��Ÿ�")]
    private int move;
    public int MOVE { get { return move; } }

    [SerializeField, Space(2), Header("��ų ������")]
    private Sprite skillicon;
    public Sprite SKILLICON { get { return skillicon; } }

    [SerializeField, Space(2), Header("�ִ� ��밡�� Ƚ��")]
    private int uses;
    public int USES { get { return uses; } }

    [SerializeField, Space(2), Header("��Ÿ��")]
    private int cooltime;
    public int COOLTIME { get { return cooltime; } }

    [SerializeField, Space(2), Header("����� ��� üũ")]
    private bool isLink;
    public bool ISLINK { get { return isLink; } }

    [SerializeField, Space(2), Header("���轺ų / ���� �������� �� ��ĭ")]
    private string linkattack = null;
    public string LINKATTACK { get { return linkattack; } }
}
