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

    [SerializeField, Space(1), Header("�̸�")]
    private string effectname;
    public string EFFECTNAME { get { return effectname; } }

    [SerializeField, Space(1), Header("������ �̹���")]
    private Sprite effecticon;
    public Sprite EFFECTICON { get { return effecticon; } }

    [SerializeField, Space(1), Header("��ų ���� ���")]
    private Target target;
    public Target TARGET { get { return target; } }

    [SerializeField, Space(1), Header("ü�� ����/����� ��ġ")]
    private int hp;
    public int HP { get { return hp; } }

    [SerializeField, Space(1), Header("���ݷ� ����/����� ��ġ")]
    private int attack;
    public int ATTACK { get { return attack; } }

    [SerializeField, Space(1), Header("���� ����/����� ��ġ")]
    private int defense;
    public int DEFENSE { get { return defense; } }

    [SerializeField, Space(1), Header("�����̻�")]
    private Meze meze;
    public Meze MEZE { get { return meze; } }

    [SerializeField, Space(1), Header("���ӽð�")]
    private int duration;
    public int DURATION { get { return duration; } }

}
