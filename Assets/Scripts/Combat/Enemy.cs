using UnityEngine;

public abstract class Enemy : StatSystem
{
    private const int MAXDISTANCE = 5;

    //��ȹ�ڰ� ����ϱ� ���ϵ��� ����� ��� ��ų�� �� ����Ʈ�� �ְ� �����Լ��� �����ϱ�

    [Space(1), Header("���� �̸�")]
    public string enemyName;

    [SerializeField, Space(1), Header("��ü ��ų ����Ʈ")]
    private SkillSO[] allSkillList;

    [SerializeField, Space(1), Header("��ȣ �Ÿ�")]
    private int firstPreferenceDistance;
    [SerializeField]
    private int secondPreferenceDistance;

    [Space(1), Header("�� ��������Ʈ")]
    public Sprite enemySprite;

    public void EnemySkillListReady()
    {
        foreach (SkillSO skill in allSkillList)
        {
            ApplySkill(skill);
        }
    }

    //���� 
    public SkillDB ReservationSkill(int distance)
    {
        Debug.Log("1");
        //����X, ����X, �ӹ�(���X)
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

        //���� �̵���ų �����ϱ�(��ȣ�Ÿ� üũ X)
        //����O, �ӹ�X, ����X
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

        //����X, �ӹ�X, ����X
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

        //����O
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