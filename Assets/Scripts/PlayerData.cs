using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerData : MonoBehaviour
{
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

    [SerializeField]
    public List<SkillSO> Skill = new List<SkillSO>();

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

        MaxHP+=100;
        AttackPoint=10;
        DefencePoint=10;
        SpeedPoint=10;
    }

    public UnityEvent<int, int> OnHPChanged;
    public UnityEvent<int> OnAttackPointChanged;
    public UnityEvent<int> OnDefencePointChanged;
    public UnityEvent<int> OnSpeedPointChanged;
}
