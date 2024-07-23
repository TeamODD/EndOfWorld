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

    [SerializeField, Space(4), Header("스킬 이름")]
    private string skillname;
    public string SKILLNAME { get { return skillname; } }

    [SerializeField, Space(2), Header("스킬 타입")]
    private SkillType skilltype;
    public SkillType SKILLTYPE { get { return skilltype; } }

    [SerializeField, Space(2), Header("공격 횟수"), Range(0, 3)]
    private int numberOfAttack;
    public int NUMBEROFATTACK { get { return numberOfAttack; } }

    [SerializeField, Space(2), Header("데미지 비율")]
    private int damage;
    public int DAMAGE { get { return damage; } }

    [SerializeField, Space(2), Header("사정거리")]
    private int distance;
    public int DISTANCE { get { return distance; } }

    [SerializeField, Space(2), Header("버프 / 디버프 리스트")]
    private StatusEffetSO[] effect;
    public StatusEffetSO[] EFFECT { get { return effect; } }

    [SerializeField, Space(2), Header("이동거리")]
    private int move;
    public int MOVE { get { return move; } }

    [SerializeField, Space(2), Header("스킬 아이콘")]
    private Sprite skillicon;
    public Sprite SKILLICON { get { return skillicon; } }

    [SerializeField, Space(2), Header("최대 사용가능 횟수")]
    private int uses;
    public int USES { get { return uses; } }

    [SerializeField, Space(2), Header("쿨타임")]
    private int cooltime;
    public int COOLTIME { get { return cooltime; } }

    [SerializeField, Space(2), Header("연계기 사용 체크")]
    private bool isLink;
    public bool ISLINK { get { return isLink; } }

    [SerializeField, Space(2), Header("연계스킬 / 존재 하지않을 시 빈칸")]
    private string linkattack = null;
    public string LINKATTACK { get { return linkattack; } }
}
