using System.Collections;
using System.Collections.Generic;
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

    //플레이어, 적 스킬 및 상태 정보를 가져오기 위한 변수
    private Player player;
    private Enemy enemy;

    private List<SkillDB> playerCombatSkillList;
    private List<SkillDB> playerMoveSkillList;

    //임시
    public SkillSO playerSkill;
    public SkillSO playerSkill2;

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

        //현재 스탯 설정
        player.BattleStartStat();
        enemy.BattleStartStat();

        //거리 설정 지금은 임의로 정하지만 후에 어떻게 설정할 것인지
        distance = SetDistance();
        
        //플레이어 스킬 가져오기
        playerCombatSkillList = player.GetCombatSkillList();
        playerMoveSkillList = player.GetMoveSkillList();

        //적 첫 스킬 예약
        enemy.EnemySkillListReady();
        enemy.ReservationSkill(distance);

        //HUD 설정
        setHUDAll();
        //playerHUD.SetSkillButton(playerSkillList);

        //속도 비교 후 선공권
        state = CompareSpeed();

        if (state == BattleState.PLAYERTURN) PlayerTurn();
        else EnemyTurn();
    }

    private void PlayerTurn()
    {
        player.DicreaseEffectDuration();
        player.TurnResetStat();
        player.ActivateEffect();

        setHUDAll();

        state = BattleState.PLAYERTURN;
        Debug.Log("플레이어 턴!");
    }

    private void EnemyTurn()
    {
        Debug.Log("적의 턴");
        enemy.DicreaseEffectDuration();
        enemy.TurnResetStat();
        enemy.ActivateEffect();

        setHUDAll();

        state = BattleState.ENEMYTURN;
        
        //연계기를 어떻게 해야할까
        if (enemy.isFrightened && enemy.reservationSkill.TYPE == SkillSO.SkillType.combatSkill) enemy.ReservationSkill(distance);
        if (enemy.isEnsnared && enemy.reservationSkill.TYPE == SkillSO.SkillType.moveSkill) enemy.ReservationSkill(distance);
        if (enemy.isParalysus) enemy.ReservationSkill(distance);

        CaculateCombat(enemy.reservationSkill);

        if(player.currentHitPoint <= 0)
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
    }

    private void CaculateCombat(SkillDB usingSkill)
    {
        //먼저 사거리에 닿았는가?도 계산이 필요할듯하다.
        //해야할일
        //사거리 계산
        //초기 거리
        //플레이어 버튼
        //버튼 사용횟수 정보 전달
        //몬스터 연계기 처리 lastattack 처리 안했다
        //적중시 효과발동 등은 음 적 스크립트에서 따로 처리해야할듯함
        //적 예약공격이 없을 시 처리
        //텍스트 처리
        //이것들 다하면 테스트해볼수 있을듯 함

        Debug.Log(usingSkill.NAME);
        if (usingSkill != null)
        {
            for (int i = 0; i < usingSkill.NUMOFATTACK; i++)
            {
                int receiveDamage = usingSkill.TARGET == SkillSO.Target.Player ? player.CombatSkillActivate(usingSkill) : enemy.CombatSkillActivate(usingSkill);
                Debug.Log(usingSkill.TARGET + " " + receiveDamage);
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

            //사용횟수, 쿨타임 설정
            usingSkill.COOLTIME = usingSkill.MAXCOOLTIME;
            usingSkill.USES--;

            //HUD 설정하기(사용한 스킬의 버튼 쿨타임, 사용횟수 정보를 따로 수정해야한다.)
            setHUDAll();
        }
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
            SkillDB usingSkill = skillType == SkillSO.SkillType.combatSkill ? playerCombatSkillList[skillIndex] : playerMoveSkillList[skillIndex];

            CaculateCombat(usingSkill);

            //죽었는지 안죽었는지 확인
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
