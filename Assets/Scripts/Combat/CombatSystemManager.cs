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
        //적 오브젝트 생성
        //거리 설정
        //플레이어?는 유지
        //스테이지 정보 혹은 배경 정보 받아와서 넣기
        //플레이어 스킬 UI 작업 등 HUD함수 따로 만들 예정
    }


    private void Start()
    {
        BattleStart();
    }
}
