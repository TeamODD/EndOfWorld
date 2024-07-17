using UnityEngine;

[CreateAssetMenu(fileName = "statusEffect", menuName = "effectScriptable/CreateEffectData", order = int.MaxValue)]
public class StatusEffet : ScriptableObject
{
    [SerializeField]
    private string effectname;
    public string EFFECTNAME { get { return effectname; } }

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
    private int duration;
    public int DURATION { get { return duration; } }

}
