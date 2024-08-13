using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerData : MonoBehaviour
{
    public int MaxHP { get; private set; }

    public int HP { get; private set; }

    public int ATK { get; private set; }

    public int DEF { get; private set; }

    public int DEX { get; private set; }

    public int Distance;

    public MonsterName MonsterName { get; private set; }
    
    private void Awake()
    {
        init();
    }

    private void init()
    {
        DontDestroyOnLoad(this.gameObject);

        MaxHP = 0;
        HP = 0;
        ATK = 0;
        DEF = 0;
        DEX = 0;
        Distance = 0;
    }

    public void SetMaxHP(int amount)
    {
        this.MaxHP += amount;
    }

    public void SetHP(int amount)
    {
        this.HP += amount;
    }

    public void SetATK(int amount)
    {
        this.ATK += amount;
    }

    public void SetDEF(int amount)
    {
        this.DEF += amount;
    }

    public void SetDEX(int amount)
    {
        this.DEX += amount;
    }
}
