using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class StatSystem : MonoBehaviour
{
    public int maxHitPoint { get; private set; }
    public int attackPoint { get; private set; }
    public int defensePoint { get; private set; }
    public int speed { get; private set; }

    protected int currentHitPoint;
    protected int currentAttackPoint;
    protected int currentDefensePoint;

    protected List<EffectDB> effectList = new List<EffectDB>();

    //���� ���� �� ������ �ʱ�ȭ �ϴ� �Լ� / �������� �� �� 1���� ȣ��
    protected void BattleStartStatus()
    {
        currentHitPoint = maxHitPoint;
        currentAttackPoint = attackPoint;
        currentDefensePoint = defensePoint;
    }

    //�� ���۽� �⺻ ���ݷ�, �⺻ �������� �ʱ�ȭ �ϴ� �Լ� / �ڽ��� �� ���� ȣ��
    protected void TurnResetStatus()
    {
        currentAttackPoint = attackPoint;
        currentDefensePoint = defensePoint;
    }

    //List�� ����� ����,����� ���� �����Ű�� �Լ� / �� ���� �� ȣ��
    protected void ActivateEffect()
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
    public void ApplyEffect(StatusEffet statusEffect)
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
}

//��ų�� ��ũ���ͺ������Ʈ �� �����ε� ������ �����Ұ��ΰ�
//�̸� ������ �Ÿ� �̵� ����or����� ������ų
//�̸� �̵� ����or����� 3�� �̵���ų
//2���� ������ ����Ʈ ����� �����ؾ��� �� ����.