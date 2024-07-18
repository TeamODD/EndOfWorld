using System.Collections;
using System.Collections.Generic;
using UnityEditor.iOS.Xcode;
using UnityEngine;

public enum battleState { START, PLAYERTURN, ENEMYTURN, WIN, LOSE }

public class CombatSystemManager : MonoBehaviour
{
    private battleState state;
    private int distance;

    private Player player;
    private Enemy enemy;

    private void BattleStart()
    {
        state = battleState.START;
        SetupBattle();
    }

    private void SetupBattle()
    {
        //�� ������Ʈ ����
        //�Ÿ� ����
        //�÷��̾�?�� ����
        //�������� ���� Ȥ�� ��� ���� �޾ƿͼ� �ֱ�
        //�÷��̾� ��ų UI �۾� �� HUD�Լ� ���� ���� ����
    }


    private void Start()
    {
        BattleStart();
    }
}
