using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

public class PlayerData : MonoBehaviour
{
    public static PlayerData Instance { get; private set; }
    [SerializeField]
    private int maxHP;
    [SerializeField]
    private int currentHP;
    [SerializeField]
    private int attackPoint;
    [SerializeField]
    private int defencePoint;
    [SerializeField]
    private int speedPoint;

    [HideInInspector]
    public List<SkillDB> MoveSkill;

    [HideInInspector]
    public List<SkillDB> CombatSkill;


    public void DeleteDuplication()
    {
        MoveSkill = MoveSkill.Distinct().ToList();

        CombatSkill = CombatSkill.Distinct().ToList();
    }

    public int MaxHP
    {
        get => maxHP;
        set
        {
            int gap=value-maxHP;
            maxHP=value;
            if(gap>0)
            {
                CurrentHP+=gap;
            }
            else
            {
                CurrentHP=MaxHP<CurrentHP? MaxHP : CurrentHP;
            }
        }
    }
    public int CurrentHP
    {
        get => currentHP;
        set
        {
            currentHP=value<=MaxHP ? value : MaxHP;
            OnHPChanged.Invoke(MaxHP, CurrentHP);
        }
    }
    public int AttackPoint
    {
        get => attackPoint;
        set
        {
            attackPoint=value>=0 ? value : 0;
            OnAttackPointChanged.Invoke(AttackPoint);
        }
    }
    public int DefencePoint
    {
        get => defencePoint;
        set
        {
            defencePoint=value>=0 ? value : 0;
            OnDefencePointChanged.Invoke(DefencePoint);
        }
    }
    public int SpeedPoint
    {
        get => speedPoint;
        set
        {
            speedPoint=value>=0 ? value : 0;
            OnSpeedPointChanged.Invoke(SpeedPoint);
        }
    }
    
    private void Awake()
    {
        DontDestroyOnLoad(this.gameObject);

        CombatSkill = new List<SkillDB>();
        MoveSkill = new List<SkillDB>();

        MaxHP+=100;
        AttackPoint=10;
        DefencePoint=10;
        SpeedPoint=10;
    }

    public void ApplySkill(SkillSO skill)
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
            skill.USINGTEXT,
            skill.HITTEXT,
            skill.MISSTEXT
            );

        if (skillDB.TYPE == global::SkillSO.SkillType.combatSkill) this.CombatSkill.Add(skillDB);
        else if (skillDB.TYPE == global::SkillSO.SkillType.moveSkill) this.MoveSkill.Add(skillDB);
    }

    public UnityEvent<int, int> OnHPChanged;
    public UnityEvent<int> OnAttackPointChanged;
    public UnityEvent<int> OnDefencePointChanged;
    public UnityEvent<int> OnSpeedPointChanged; 
}