using System.Collections.Generic;
using Unity.Loading;
using Unity.VisualScripting;
using UnityEngine;

public abstract class Enemy : StatSystem
{
    const int MAXDISTANCE = 5;

    //��ȹ�ڰ� ����ϱ� ���ϵ��� ����� ��� ��ų�� �� ����Ʈ�� �ְ� �����Լ��� �����ϱ�
    [SerializeField, Space(1), Header("��ü ��ų ����Ʈ")]
    private SkillSO[] allSkillList;

    [SerializeField, Space(1), Header("��ȣ �Ÿ�")]
    private int firstPreferenceDistance;
    [SerializeField]
    private int secondPreferenceDistance;
    protected SkillDB lastSkill = null;
    protected SkillDB reservationSkill = null;
    protected bool isLinkAttack = false;

    public void EnemySkillListReady()
    {
        foreach (SkillSO skill in allSkillList) 
        {
            ApplySkill(skill);
        }
    }

    protected void ReservationSkill(int distance)
    {
        //����X, ����X, �ӹ�(���X)
        if (!isFrightened && !isParalysus)
        {
            int index = getRandomIndex(combatSkillList.Count);
            for (int i = 0; i < combatSkillList.Count; i++)
            {
                if (combatSkillList[index].MINDISTANCE <= distance && combatSkillList[index].MAXDISTANCE >= distance
                     && combatSkillList[index].COOLTIME == 0
                     && combatSkillList[index].USES > 0)
                {
                    reservationSkill = combatSkillList[i];
                    return;
                }

                else index = (index + 1) % combatSkillList.Count;
            }
        }
        
        //���� �̵���ų �����ϱ�(��ȣ�Ÿ� üũ X)
        //����O, �ӹ�X, ����X
        else if(isFrightened && !isParalysus && !isEnsnared)
        {
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

        //����X, �ӹ�X, ����X
        else if (!isFrightened && !isEnsnared && !isParalysus)
        {
            if (CheckPreferenceSkillUses())
            {
                reservationSkill = selectReservationMoveSkill(firstPreferenceDistance, distance);
            }

            else if(CheckAllSkillUses())
            {
                reservationSkill = selectReservationMoveSkill(secondPreferenceDistance, distance);
            }
            
        }

        //����O
        else
        {
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

    public SkillDB getReservationSkill()
    {
        return reservationSkill;
    }
}

//�߰��� �ؾ��� ��
//1. ����Ⱑ ������ �� ��ų������ �ǳʶٰ� ����� ��ų ����� �ؾ���.
//2. ����⿡ ���ִ� ��ų�� SO�� SO�� linkskillList���� ã�Ƴ��� ����ؾ���
//3. ���� �� cc�� �¾����� �̸� üũ�ϴ� �Լ� (�����̻��� �ִ���) �ִٸ� �����Լ� �ٽ� �ϱ� 

//���� �ؾ��Ұ� ���� ��ų ���� �ý��� -> �Ϸ�
//��ų ��Ÿ��, ���Ƚ�� ���� �ϱ� -> �ؾ����� �����Ŵ������� ������ ���� �� ���Ƚ��, ��Ÿ���� �����ؾ��Ѵ�.
//����, ���, �� ������ -> �Ϸ�
//�����ý��� �ٽ� �����

//�÷��̾� ��ư UI ����