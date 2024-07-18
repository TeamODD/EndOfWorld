using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class StatSystem : MonoBehaviour
{
    public int maxHitPoint { get; private set; }
    public int attackPoint { get; private set; }
    public int defensePoint { get; private set; }
    public int speed { get; private set; }

    private int currentHitPoint;
    private int currentAttackPoint;
    private int currentDefensePoint;

    private List<EffectDB> effectList = new List<EffectDB>();

    //�⺻ ���� �����ϴ� �Լ�(���� ������ ǥ�� �����Ѵٸ� ���� ������ ���� �ִ� ������� ������ ����)
    public void InitStat(int maxHP, int ATK, int DEF, int SPD)
    {
        maxHitPoint = maxHP;
        attackPoint = ATK;
        defensePoint = DEF;
        speed = SPD;
    }

    //���� ���� �� ������ �ʱ�ȭ �ϴ� �Լ� / �������� �� �� 1���� ȣ��
    public void BattleStartStat()
    {
        currentHitPoint = maxHitPoint;
        currentAttackPoint = attackPoint;
        currentDefensePoint = defensePoint;
    }

    //�� ���۽� �⺻ ���ݷ�, �⺻ �������� �ʱ�ȭ �ϴ� �Լ� / �ڽ��� �� ���� ȣ��
    public void TurnResetStat()
    {
        currentAttackPoint = attackPoint;
        currentDefensePoint = defensePoint;
    }

    //List�� ����� ����,����� ���� �����Ű�� �Լ� / �� ���� �� ȣ��
    public void ActivateEffect()
    {
        foreach (EffectDB db in effectList) 
        {
            if (currentDefensePoint + db.HP > maxHitPoint) currentHitPoint = maxHitPoint;
            else currentHitPoint += db.HP;
            currentAttackPoint += db.ATK;
            currentDefensePoint += db.DEF;

            db.DURATION--;

            if (db.DURATION < 1) effectList.Remove(db);
            //���� ����� UI ��ũ��Ʈ
        }
    }

    //ScriptableObject�� �ۼ��� ����,����� asset�� �μ��� �޴� �Լ� / ����, ������� ���� ������ ȣ��
    public void ApplyEffect(StatusEffetSO statusEffect)
    {
        EffectDB effectDB = new EffectDB(
            statusEffect.EFFECTNAME, 
            statusEffect.HP, 
            statusEffect.ATTACK, 
            statusEffect.DEFENSE, 
            statusEffect.DURATION
            );

        effectList.Add(effectDB);
    }

    public int AttackedByEnemy(int damage)
    {
        currentHitPoint -= damage;

        return currentHitPoint;
    }

    public List<EffectDB> getEffectList() { return effectList; }
}