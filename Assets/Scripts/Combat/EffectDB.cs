using static StatusEffetSO;

public class EffectDB
{
    public string NAME { get; private set; }
    public int HP { get; private set; }
    public int ATK { get; private set; }
    public int DEF { get; private set; }
    public Meze MEZE { get; private set; }

    public int DURATION;


    public EffectDB(string name, int hp, int atk, int def, Meze meze, int duration)
    {
        NAME = name;
        HP = hp;
        ATK = atk;
        DEF = def;
        MEZE = meze;
        DURATION = duration;
    }
}
