using System.Collections.Generic;
using Unity.Loading;
using Unity.VisualScripting;
using UnityEngine;

public abstract class Enemy : StatSystem
{
    private const int MAXDISTANCE = 5;

    //기획자가 사용하기 편하도록 사용할 모든 스킬을 이 리스트에 넣고 내부함수로 구분하기
    [SerializeField, Space(1), Header("전체 스킬 리스트")]
    private SkillSO[] allSkillList;

    [SerializeField, Space(1), Header("선호 거리")]
    private int firstPreferenceDistance;
    [SerializeField]
    private int secondPreferenceDistance;

    public SkillDB lastSkill { get; private set; } = null;
    public SkillDB reservationSkill { get; private set; } = null;
    public bool isLinkAttack { get; private set; } = false;

    public void EnemySkillListReady()
    {
        foreach (SkillSO skill in allSkillList) 
        {
            ApplySkill(skill);
        }
    }

    //현재 
    public void ReservationSkill(int distance)
    {
        Debug.Log("1");
        //공포X, 마비X, 속박(상관X)
        if (!isFrightened && !isParalysus)
        {
            int index = getRandomIndex(combatSkillList.Count);
            for (int i = 0; i < combatSkillList.Count; i++)
            {
                if (combatSkillList[index].MINDISTANCE <= distance && combatSkillList[index].MAXDISTANCE >= distance
                     && combatSkillList[index].COOLTIME == 0
                     && combatSkillList[index].USES > 0)
                {
                    reservationSkill = combatSkillList[index];
                    return;
                }

                else index = (index + 1) % combatSkillList.Count;
            }
        }
        

        //랜덤 이동스킬 선택하기(선호거리 체크 X)
        //공포O, 속박X, 마비X
        if(isFrightened && !isParalysus && !isEnsnared)
        {
            Debug.Log("2");

            int index = getRandomIndex(moveSkillList.Count);
            for (int i = 0; i < moveSkillList.Count; i++) 
            {
                if (moveSkillList[index].COOLTIME == 0 && moveSkillList[index].USES > 0)
                {
                    reservationSkill = moveSkillList[index];
                    return;
                }

                else index = (index + 1) % moveSkillList.Count;
            }
        }

        //공포X, 속박X, 마비X
        if (!isEnsnared && !isParalysus)
        {
            Debug.Log("3");

            if (CheckPreferenceSkillUses())
            {
                reservationSkill = selectReservationMoveSkill(firstPreferenceDistance, distance);
            }

            else if(CheckAllSkillUses())
            {
                reservationSkill = selectReservationMoveSkill(secondPreferenceDistance, distance);
            }
            
        }

        //마비O
        if(isParalysus)
        {
            Debug.Log("4");

            reservationSkill = null;
            lastSkill = null;
        }
    }

    private SkillDB selectReservationMoveSkill(int preference, int distance)
    {
        int gap = MAXDISTANCE;
        SkillDB tempSkill = null;

        for (int i = 0; i < moveSkillList.Count; i++)
        {
            if (moveSkillList[i].USES <= 0) continue;

            if (tempSkill != null && gap == Mathf.Abs(preference - Mathf.Abs(distance - moveSkillList[i].MOVE)) && tempSkill.USES < moveSkillList[i].USES)
            {
                tempSkill = moveSkillList[i];
            }

            if (gap > (Mathf.Abs(preference - Mathf.Abs(distance - moveSkillList[i].MOVE))))
            {
                tempSkill = moveSkillList[i];
            }
        }

        if (tempSkill != null) return tempSkill;
        else return null;
    }

    private bool CheckAllSkillUses()
    {
        foreach(SkillDB skill in combatSkillList)
        {
            if (skill.USES > 0 &&
               skill.COOLTIME <= 1) return true;
        }

        return false;
    }

    private bool CheckPreferenceSkillUses()
    {
        foreach(SkillDB skill in combatSkillList)
        {
            if (skill.MINDISTANCE <= firstPreferenceDistance && 
                skill.MAXDISTANCE >= firstPreferenceDistance && 
                skill.USES > 0 && 
                skill.COOLTIME <= 1) return true;
        }

        return false;
    }

    private int getRandomIndex(int count)
    {
        return Random.Range(0, count);
    }
}

//추가로 해야할 것
//1. 연계기가 존재할 때 스킬예약을 건너뛰고 예약된 스킬 사용을 해야함.
//2. 연계기에 들어가있는 스킬은 SO로 SO와 linkskillList에서 찾아내서 사용해야함
//3. 예약 후 cc를 맞았을때 이를 체크하는 함수 (상태이상이 있는지) 있다면 예약함수 다시 하기 

//현재 해야할게 몬스터 스킬 예약 시스템 -> 완료
//스킬 쿨타임, 사용횟수 관리 하기 -> 해야할일 전투매니저에서 공격이 나갈 때 사용횟수, 쿨타임을 설정해야한다.
//공격, 방어, 힐 나누기 -> 완료
//전투시스템 다시 만들기

//플레이어 버튼 UI 관리