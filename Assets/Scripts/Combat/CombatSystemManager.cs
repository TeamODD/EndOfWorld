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

    private List<SkillDB> playerSkillList;

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
        if(instance == null)
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
        enemy.InitStat(100, 10, 2, 1);
        player.setSkill(playerSkill);
        player.setSkill(playerSkill2);

        //현재 스탯 설정
        player.BattleStartStat();
        enemy.BattleStartStat();

        //플레이어 스킬 가져오기
        //playerSkillList = player.GetSkillList();
        
        //HUD 설정
        playerHUD.SetHUD(player);
        enemyHUD.SetHUD(enemy);
        playerHUD.SetSkillButton(playerSkillList);
        
        //거리 설정 지금은 임의로 정하지만 후에 어떻게 설정할 것인지
        distance = SetDistance();

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

        playerHUD.SetHUD(player);
        enemyHUD.SetHUD(enemy);

        state = BattleState.PLAYERTURN;
        Debug.Log("플레이어 턴!");
    }

    private void EnemyTurn()
    {/*
        state = BattleState.ENEMYTURN;
        Debug.Log("적 턴!");

        SkillSO enemyAttackSkill = enemy.Attack(distance, player);
        //공격횟수 추가
        for(int i = 0; i < enemyAttackSkill.NUMBEROFATTACK; i++)
        {
            player.AttackedByEnemy(enemyAttackSkill.DAMAGE);
            playerHUD.SetHPSlider(player.currentHitPoint);
        }
        PlayerTurn();
    */}

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


    //스킬 사용시 -> 공격이 닿았는지 체크, 닿았다면 데미지 체크, 사용된 버프 디버프(적, 본인 둘다) 체크, 이동이 되었는지 체크, 사용된 스킬 횟수 체크, 사용이 전부 완료된 스킬은 비활성화 예정
    public void OnSkillButton()
    {
        if (state != BattleState.PLAYERTURN)
            return;

        GameObject clickButton = EventSystem.current.currentSelectedGameObject;
        if (clickButton != null)
        {
            int skillIndex = clickButton.GetComponent<SkillButtonInfo>().getSkillIndex();
            SkillDB usingSkill = playerSkillList[skillIndex];

            //사용한 스킬에 따라 공격, 버프, 디버프 등을 설정해줘야함
            //bool isDead = enemy.AttackedByEnemy(usingSkill.DAMAGE);

            if (usingSkill.EFFECT != null)
            {
                foreach(StatusEffetSO effect in usingSkill.EFFECT)
                {
                    player.ApplyEffect(effect);
                }
            }
            playerHUD.SetHUD(player);

            //이동은 추후에 그리고 이런 공격은 플레이어 공격, 적 공격 함수로 따로 떼어낼 예정
            //스킬 사용하면 적체력체크, 버프체크, HUD설정이 한꺼번에 이루어져야한다.
            enemyHUD.SetHPSlider(enemy.currentHitPoint);

            if (true)
            {
                Debug.Log("플레이어 승리!");
                //player.SetSkillList(playerSkillList);
            }

            else
            {
                //EnemyTurn();
            }

        }

    }

}
