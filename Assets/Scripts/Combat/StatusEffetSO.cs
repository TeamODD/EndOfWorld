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

    [SerializeField, Space(1), Header("이름")]
    private string effectname;
    public string EFFECTNAME { get { return effectname; } }

    [SerializeField, Space(1), Header("아이콘 이미지")]
    private Sprite effecticon;
    public Sprite EFFECTICON { get { return effecticon; } }

    [SerializeField, Space(1), Header("스킬 지정 대상")]
    private Target target;
    public Target TARGET { get { return target; } }

    [SerializeField, Space(1), Header("체력 버프/디버프 수치")]
    private int hp;
    public int HP { get { return hp; } }

    [SerializeField, Space(1), Header("공격력 버프/디버프 수치")]
    private int attack;
    public int ATTACK { get { return attack; } }

    [SerializeField, Space(1), Header("방어력 버프/디버프 수치")]
    private int defense;
    public int DEFENSE { get { return defense; } }

    [SerializeField, Space(1), Header("상태이상")]
    private Meze meze;
    public Meze MEZE { get { return meze; } }

    [SerializeField, Space(1), Header("지속시간")]
    private int duration;
    public int DURATION { get { return duration; } }

}
