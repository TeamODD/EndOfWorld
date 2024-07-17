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

    //전투 시작 시 스탯을 초기화 하는 함수 / 전투시작 시 단 1번만 호출
    protected void BattleStartStatus()
    {
        currentHitPoint = maxHitPoint;
        currentAttackPoint = attackPoint;
        currentDefensePoint = defensePoint;
    }

    //턴 시작시 기본 공격력, 기본 방어력으로 초기화 하는 함수 / 자신의 턴 마다 호출
    protected void TurnResetStatus()
    {
        currentAttackPoint = attackPoint;
        currentDefensePoint = defensePoint;
    }

    //List에 저장된 버프,디버프 들을 적용시키는 함수 / 턴 시작 시 호출
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
            //버프 디버프 UI 스크립트
        }
    }

    //ScriptableObject로 작성된 버프,디버프 asset을 인수로 받는 함수 / 버프, 디버프를 받을 때마다 호출
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

//스킬도 스크립터블오브젝트 할 예정인데 구성을 어케할것인가
//이름 데미지 거리 이동 버프or디버프 전투스킬
//이름 이동 버프or디버프 3개 이동스킬
//2개로 나눠서 리스트 만들고 관리해야할 것 같다.