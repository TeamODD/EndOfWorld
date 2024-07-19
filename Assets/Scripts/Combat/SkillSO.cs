using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "skill", menuName = "skillScriptable/CreateSkillData", order = 2)]

public class SkillSO : ScriptableObject
{
    [SerializeField]
    private string skillname;
    public string SKILLNAME { get { return skillname; } }

    [SerializeField]
    private int damage;
    public int DAMAGE { get { return damage; } }

    [SerializeField]
    private int distance;
    public int DISTANCE { get { return distance; } }

    [SerializeField]
    private StatusEffetSO effect;
    public StatusEffetSO EFFECT { get { return effect; } }

    [SerializeField]
    private int move;
    public int MOVE { get { return move; } }

    [SerializeField]
    private Sprite skillicon;
    public Sprite SKILLICON { get { return skillicon; } }

    [SerializeField]
    private int uses;
    public int USES { get { return uses; } }

    [SerializeField]
    private int cooltime;
    public int COOLTIME { get { return cooltime; } }
}
