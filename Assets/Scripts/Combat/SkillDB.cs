using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static SkillSO;

public class SkillDB
{
    public string NAME { get; private set; }
    public SkillType TYPE { get; private set; }
    public int NUMOFATTACK { get; private set; }
    public int DAMAGE { get; private set; }
    public int DISTANCE { get; private set; }
    public StatusEffetSO[] EFFECT { get; private set; }
    public int MOVE { get; private set; }
    public Sprite ICON { get; private set; }
    
    public int USES;
    public int COOLTIME;

    public SkillDB(string name, SkillType skillType, int numOfAttack, int damage, int distance, StatusEffetSO[] effect, int move, Sprite icon, int uses, int cooltime)
    {
        NAME = name;
        TYPE = skillType;
        NUMOFATTACK = numOfAttack;
        DAMAGE = damage;
        DISTANCE = distance;
        EFFECT = effect;
        MOVE = move;
        ICON = icon;
        USES = uses;
        COOLTIME = cooltime;
    }
}
