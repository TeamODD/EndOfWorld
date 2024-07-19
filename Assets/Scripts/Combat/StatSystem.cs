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

    //기본 스탯 저장하는 함수(레벨 데이터 표가 존재한다면 일정 렙마다 값을 넣는 방식으로 레벨업 가능)
    public void InitStat(int maxHP, int ATK, int DEF, int SPD)
    {
        maxHitPoint = maxHP;
        attackPoint = ATK;
        defensePoint = DEF;
        speed = SPD;
    }

    //전투 시작 시 스탯을 초기화 하는 함수 / 전투시작 시 단 1번만 호출
    public void BattleStartStat()
    {
        currentHitPoint = maxHitPoint;
        currentAttackPoint = attackPoint;
        currentDefensePoint = defensePoint;
    }

    //턴 시작시 기본 공격력, 기본 방어력으로 초기화 하는 함수 / 자신의 턴 마다 호출
    public void TurnResetStat()
    {
        currentAttackPoint = attackPoint;
        currentDefensePoint = defensePoint;
    }

    //List에 저장된 버프,디버프 들을 적용시키는 함수 / 턴 시작 시 호출
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
            //버프 디버프 UI 스크립트
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

    //ScriptableObject로 작성된 버프,디버프 asset을 인수로 받는 함수 / 버프, 디버프를 받을 때마다 호출
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

    //후에 방어력 계산까지 하는 함수로 변경 예정
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