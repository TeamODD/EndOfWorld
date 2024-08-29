using UnityEngine;
using static StatusEffetSO;

public class EffectDB
{
    public string NAME { get; private set; }
    public Sprite ICON { get; private set; }
    public Target TARGET { get; private set; }
    public int HP { get; private set; }
    public int ATK { get; private set; }
    public int DEF { get; private set; }
    public Meze MEZE { get; private set; }

    public int DURATION;


    public EffectDB(string name, Sprite icon, Target target, int hp, int atk, int def, Meze meze, int duration)
    {
        NAME = name;
        ICON = icon;
        TARGET = target;
        HP = hp;
        ATK = atk;
        DEF = def;
        MEZE = meze;
        DURATION = duration;
    }
}
