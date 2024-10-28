using UnityEngine;

public abstract class Enemy : StatSystem
{
    private const int MAXDISTANCE = 5;

    //기획자가 사용하기 편하도록 사용할 모든 스킬을 이 리스트에 넣고 내부함수로 구분하기

    [Space(1), Header("몬스터 이름")]
    public string enemyName;

    [SerializeField, Space(1), Header("전체 스킬 리스트")]
    private SkillSO[] allSkillList;

    [SerializeField, Space(1), Header("선호 거리")]
    private int firstPreferenceDistance;
    [SerializeField]
    private int secondPreferenceDistance;

    [Space(1), Header("적 스프라이트")]
    public Sprite enemySprite;

    public void EnemySkillListReady()
    {
        foreach (SkillSO skill in allSkillList)
        {
            ApplySkill(skill);
        }
    }

    //현재 
    public SkillDB ReservationSkill(int distance)
    {
        Debug.Log("1");
        //공포X, 마비X, 속박(상관X)
        if (!isFrightened && !isParalysus)
        {
            int index = getRandomIndex(combatSkillList.Count);
            for (int i = 0; i < combatSkillList.Count; i++)
            {
                Debug.Log(combatSkillList[index].NAME);
                if (combatSkillList[index].MINDISTANCE <= distance && combatSkillList[index].MAXDISTANCE >= distance
                     && combatSkillList[index].COOLTIME == 0
                     && combatSkillList[index].USES > 0)
                {
                    return combatSkillList[index];
                }

                else index = (index + 1) % combatSkillList.Count;
            }
        }

        //랜덤 이동스킬 선택하기(선호거리 체크 X)
        //공포O, 속박X, 마비X
        if (isFrightened && !isParalysus && !isEnsnared)
        {
            Debug.Log("2");

            int index = getRandomIndex(moveSkillList.Count);
            for (int i = 0; i < moveSkillList.Count; i++)
            {
                if (moveSkillList[index].COOLTIME == 0 && moveSkillList[index].USES > 0)
                {
                    return moveSkillList[index];
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
                return selectReservationMoveSkill(firstPreferenceDistance, distance);
            }

            else if (CheckAllSkillUses())
            {
                return selectReservationMoveSkill(secondPreferenceDistance, distance);
            }

        }

        //마비O
        if (isParalysus)
        {
            Debug.Log("4");
        }

        return null;
    }

    private SkillDB selectReservationMoveSkill(int preference, int distance)
    {
        int gap = MAXDISTANCE;
        SkillDB tempSkill = null;

        for (int i = 0; i < moveSkillList.Count; i++)
        {
            if (moveSkillList[i].USES <= 0 || moveSkillList[i].COOLTIME > 0) continue;

            if (tempSkill != null && gap == Mathf.Abs(preference - Mathf.Abs(distance - moveSkillList[i].MOVE)) && tempSkill.USES < moveSkillList[i].USES)
            {
                tempSkill = moveSkillList[i];
            }

            if (gap > (Mathf.Abs(preference - Mathf.Abs(distance - moveSkillList[i].MOVE))))
            {
                tempSkill = moveSkillList[i];
            }
        }

        if (tempSkill != null)
        {
            return tempSkill;
        }

        else
        {
            return null;
        }
            
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

    public SkillDB findSkill(SkillSO findSkill)
    {
        int findIndex = -1;
        if (findSkill.SKILLTYPE == SkillSO.SkillType.combatSkill)
        {
            findIndex = combatSkillList.FindIndex(x => x.NAME == findSkill.name);
            if (findIndex != -1) return combatSkillList[findIndex];
        }
        
        else
        {
            findIndex = moveSkillList.FindIndex(x => x.NAME == findSkill.name);
            if(findIndex != -1) return moveSkillList[findIndex];
        }
        
        return null;
    }
}