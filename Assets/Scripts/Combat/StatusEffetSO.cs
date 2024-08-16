using UnityEngine;

[CreateAssetMenu(fileName = "statusEffect", menuName = "effectScriptable/CreateEffectData", order = 1)]
public class StatusEffetSO : ScriptableObject
{
    public enum Meze
    {
        None,
        Frightened,
        Ensnared,
        Paralysis
    }

    public enum Target
    {
        Player,
        Enemy
    }

    [SerializeField]
    private string effectname;
    public string EFFECTNAME { get { return effectname; } }

    [SerializeField]
    private Target target;
    public Target TARGET { get { return target; } }

    [SerializeField]
    private int hp;
    public int HP { get { return hp; } }

    [SerializeField]
    private int attack;
    public int ATTACK { get { return attack; } }

    [SerializeField]
    private int defense;
    public int DEFENSE { get { return defense; } }

    [SerializeField]
    private Meze meze;
    public Meze MEZE { get { return meze; } }

    [SerializeField]
    private int duration;
    public int DURATION { get { return duration; } }

}
