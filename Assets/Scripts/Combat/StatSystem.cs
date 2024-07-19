using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class StatSystem : MonoBehaviour
{
    public int maxHitPoint { get; private set; }
    public int attackPoint { get; private set; }
    public int defensePoint { get; private set; }
    public int speed { get; private set; }

    public int currentHitPoint { get; private set; }
    public int currentAttackPoint { get; private set; }
    public int currentDefensePoint { get; private set; }

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
        if (effectList.Count < 1) return;

        for(int i =0; i < effectList.Count; i++) {
            EffectDB db = effectList[i];

            if (currentDefensePoint + db.HP > maxHitPoint) currentHitPoint = maxHitPoint;
            else currentHitPoint += db.HP;
            currentAttackPoint += db.ATK;
            currentDefensePoint += db.DEF;

            if (db.DURATION < 1) effectList.Remove(db);
            //���� ����� UI ��ũ��Ʈ
        }

    }

    public void DicreaseEffectDuration()
    {
        for(int i =0; i< effectList.Count; i++)
        {
            EffectDB db = effectList[i];
            if (--db.DURATION < 1) effectList.Remove(db);

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

        ActivateEffect();
    }

    //�Ŀ� ���� ������ �ϴ� �Լ��� ���� ����
    public bool AttackedByEnemy(int damage)
    {
        int currentDamage = (damage * currentAttackPoint) - currentDefensePoint;
        if (currentDamage < 0) currentDamage = 0;
        currentHitPoint -= currentDamage;

        Debug.Log(currentDamage + " / " + currentHitPoint);
        bool isDead = currentHitPoint <= 0 ? true : false;
        return isDead;
    }

    public List<EffectDB> getEffectList() { return effectList; }
}