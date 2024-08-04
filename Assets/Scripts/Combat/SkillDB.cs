using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static SkillSO;

public class SkillDB
{
    public string NAME { get; private set; }
    public SkillType TYPE { get; private set; }
    public SkillAttackType ATTACKTYPE { get; private set; }
    public Target TARGET { get; private set; }
    public CoefficientType COEFFICIENTTYPE { get; private set; }
    public int MINRANGE { get; private set; }
    public int MAXRANGE { get; private set; }
    public int NUMOFATTACK { get; private set; }
    public int DAMAGE { get; private set; }
    public int MINDISTANCE { get; private set; }
    public int MAXDISTANCE { get; private set; }
    public int MOVE { get; private set; }
    public StatusEffetSO[] EFFECT { get; private set; }
    public SkillSO LINKSKILL { get; private set; }
    public Sprite ICON { get; private set; }
    public int MAXUSES { get; private set; }
    public int MAXCOOLTIME {  get; private set; }
    public int USES;
    public int COOLTIME;
    public string TEXT { get; private set; }

    public SkillDB(string name,
                   SkillType skillType,
                   SkillAttackType attackType,
                   Target target,
                   CoefficientType coefficientType,
                   int minRange,
                   int maxRange,
                   int numOfAttack,
                   int damage,
                   int minDistance,
                   int maxDistance,
                   int move,
                   StatusEffetSO[] effect,
                   SkillSO linkskill,
                   Sprite icon,
                   int uses,
                   int cooltime,
                   string text)
    {
        NAME = name;
        TYPE = skillType;
        ATTACKTYPE = attackType;
        TARGET = target;
        COEFFICIENTTYPE = coefficientType;
        MINRANGE = minRange;
        MAXRANGE = maxRange;
        NUMOFATTACK = numOfAttack;
        DAMAGE = damage;
        MINDISTANCE = minDistance;
        MAXDISTANCE = maxDistance;
        MOVE = move;
        EFFECT = effect;
        LINKSKILL = linkskill;
        ICON = icon;
        USES = uses;
        MAXUSES = uses;
        COOLTIME = 0;
        MAXCOOLTIME = cooltime;
        TEXT = text;
    }
}
