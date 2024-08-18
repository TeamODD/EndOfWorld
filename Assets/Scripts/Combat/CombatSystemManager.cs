using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public enum BattleState { START, PLAYERTURN, ENEMYTURN, WIN, LOSE }

public class CombatSystemManager : MonoBehaviour
{
    //싱글톤
    private static CombatSystemManager instance;

    //플레이어 및 적 HUD 매니저 변수
    [SerializeField]
    private CombatHUDManager playerHUD;
    [SerializeField]
    private CombatHUDManager enemyHUD;

    //현재 전투 상태 및 자원 변수
    private BattleState state;
    private int distance;
    private bool isHit;
    private int reservedDamageOrHealAmount;

    //플레이어, 적 스킬 및 상태 정보를 가져오기 위한 변수
    private Player player;
    private Enemy enemy;

    //임시
    public SkillSO playerSkill;
    public SkillSO playerSkill2; 
    public SkillSO playerSkill3;
    public SkillSO playerSkill4;

    //적 예약 스킬
    private SkillDB enemyReservationSkill;

    public static CombatSystemManager Instance
    {
        get
        {
            if (null == instance)
            {
                return null;
            }
            return instance;
        }
    }
    private void Awake()
    {
        if (instance == null)
            instance = this;

        else
        {
            Destroy(this.gameObject);
        }
    }

    private void Start()
    {
        BattleStart();
    }

    private void BattleStart()
    {
        state = BattleState.START;
        SetupBattle();
    }

    private void SetupBattle()
    {
        player = GameObject.FindObjectOfType<Player>();
        enemy = GameObject.FindObjectOfType<Enemy>();

        //임시로 스탯, 스킬 설정
        player.InitStat(100, 10, 5, 3);
        enemy.InitStat(100, 10, 2, 5);
        player.setSkill(playerSkill);
        player.setSkill(playerSkill2); 
        player.setSkill(playerSkill3);
        player.setSkill(playerSkill4);

        //현재 스탯 설정
        player.BattleStartStat();
        enemy.BattleStartStat();

        //거리 설정 지금은 임의로 정하지만 후에 어떻게 설정할 것인지
        distance = SetDistance();

        //적 첫 스킬 예약
        enemy.EnemySkillListReady();
        enemyReservationSkill = enemy.ReservationSkill(distance);

        //HUD 설정
        setHUDAll();
        playerHUD.SetSkillButton(player.combatSkillList);
        playerHUD.SetSkillButton(player.moveSkillList);

        //속도 비교 후 선공권
        state = CompareSpeed();

        if (state == BattleState.PLAYERTURN) PlayerTurn();
        else EnemyTurn();
    }

    private void PlayerTurn()
    {
        Debug.Log("플레이어 턴!");
        player.CoolTimeSet();
        player.DicreaseEffectDuration();
        player.TurnResetStat();
        player.ActivateEffect();

        setHUDAll();

        state = BattleState.PLAYERTURN;
    }

    private void EnemyTurn()
    {
        Debug.Log("적의 턴");
        enemy.CoolTimeSet();
        enemy.DicreaseEffectDuration();
        enemy.TurnResetStat();
        enemy.ActivateEffect();

        setHUDAll();

        state = BattleState.ENEMYTURN;

        if (enemy.isFrightened || enemy.isEnsnared || enemy.isParalysus) enemyReservationSkill = enemy.ReservationSkill(distance);

        if (enemyReservationSkill != null)
        {
            isHit = CheckHitAttackDistance(enemyReservationSkill) ? true : false;
            if(isHit) CaculateCombat(enemyReservationSkill);
            SkillCooltimeAndUsesSet(enemyReservationSkill);

        }

        if (enemyReservationSkill == null) Debug.Log("아무 행동 안함! 경축!");
        
        //뭐 여따가 값을 넘길건데 예약스킬이 null이면 아무행동하지않았다! isHit도 넘겨서 적중시 비적중시 체크, 그냥 스킬도 넘겨지므로 사용시 대사까지 한꺼번에
        setHUDAll();

        if(enemyReservationSkill != null && enemyReservationSkill.LINKSKILL != null)
        { 
            enemyReservationSkill = enemy.linkSkillList.Find(x => x.NAME == enemyReservationSkill.LINKSKILL.SKILLNAME);
        }

        else
        {
            enemyReservationSkill = enemy.ReservationSkill(distance);
        }

        if (player.currentHitPoint <= 0)
        {
            Debug.Log("적의 승리");
            //패배 이벤트
        }

        else
        {
            PlayerTurn();
        }
    }

    private int SetDistance()
    {
        int temp = 2;
        return temp;
    }

    private BattleState CompareSpeed()
    {
        if (player.speed >= enemy.speed) return BattleState.PLAYERTURN;
        else return BattleState.ENEMYTURN;
    }

    private void setHUDAll()
    {
        playerHUD.SetHUD(player);
        playerHUD.SetHPSlider(player.currentHitPoint);
        enemyHUD.SetHUD(enemy);
        enemyHUD.SetHPSlider(enemy.currentHitPoint);
        //여따가 적 이미지 거리 계산하는 함수 넣기
    }

    //사거리 체크
    private bool CheckHitAttackDistance(SkillDB usingSkill)
    {
        if (usingSkill.MINRANGE <= distance && usingSkill.MAXRANGE >= distance) return true;
        
        return false;
    }

    private void CaculateCombat(SkillDB usingSkill)
    {
        if (usingSkill != null)
        {
            for (int i = 0; i < usingSkill.NUMOFATTACK; i++)
            {
                reservedDamageOrHealAmount = usingSkill.TARGET == SkillSO.Target.Player ? player.CombatSkillActivate(usingSkill) : enemy.CombatSkillActivate(usingSkill);
                if (usingSkill.EFFECT.Length > 0)
                {
                    Debug.Log("버프/디버프 발동");
                    foreach (var effect in usingSkill.EFFECT)
                    {
                        if (effect.TARGET == StatusEffetSO.Target.Player) player.ApplyEffect(effect);
                        else enemy.ApplyEffect(effect);
                    }
                }
            }

            //이동 확인
            if (!player.isEnsnared)
            {
                distance = Mathf.Abs(distance + usingSkill.MOVE);
                if (distance == 0) distance = 1;
            }
        }
    }

    private void SkillCooltimeAndUsesSet(SkillDB usingSkill)
    {
        usingSkill.COOLTIME = usingSkill.MAXCOOLTIME;
        usingSkill.USES--;
    }

    public void OnSkillButton()
    {
        if (state != BattleState.PLAYERTURN)
            return;

        GameObject clickButton = EventSystem.current.currentSelectedGameObject;
        if (clickButton != null)
        {
            int skillIndex = clickButton.GetComponent<SkillButtonInfo>().skillIndex;
            SkillSO.SkillType skillType = clickButton.GetComponent<SkillButtonInfo>().skill.TYPE;
            SkillDB usingSkill = skillType == SkillSO.SkillType.combatSkill ? player.combatSkillList[skillIndex] : player.moveSkillList[skillIndex];

            isHit = CheckHitAttackDistance(usingSkill) ? true : false;
            
            if(isHit) CaculateCombat(usingSkill);
            SkillCooltimeAndUsesSet(usingSkill);

            setHUDAll();

            clickButton.GetComponent<SkillButtonInfo>().UsedSkillSet(usingSkill);
            playerHUD.SetButtonActivated();

            if (enemy.currentHitPoint <= 0)
            {
                Debug.Log("플레이어 승리");
                //승리 이벤트 추가
            }

            else
            {
                EnemyTurn();
            }
        }
    }
}
