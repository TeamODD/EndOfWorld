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
    [SerializeField]
    private CombatLogSystemManager logSystemManager;
    [SerializeField]
    private GameObject enemyParent;

    //현재 전투 상태 및 자원 변수
    private BattleState state;
    private int distance;
    private bool isHit;
    private int reservedDamageOrHealAmount;
    private SceneTransitionManager dataManager;

    //플레이어, 적 스킬 및 상태 정보를 가져오기 위한 변수
    private Player player;
    private Enemy enemy;

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
        //몬스터 데이터 및 전투 상황 가져오기
        dataManager = GameObject.FindObjectOfType<SceneTransitionManager>();
        Instantiate(dataManager.EnemyData, enemyParent.transform);
        enemy = GameObject.FindObjectOfType<Enemy>();
        InitialStat initialStat = enemy.GetComponent<InitialStat>();
        enemy.InitStat(initialStat.MaxHP, initialStat.AttackPoint, initialStat.DefencePoint, initialStat.SpeedPoint);
        enemyHUD.SetEnemyName(enemy);

        //거리
        distance = initialStat.StartingDistance;
        
        //플레이어 데이터 가져오기
        PlayerData playerData = GameObject.FindObjectOfType<PlayerData>();
        player = GameObject.FindObjectOfType<Player>();
        player.InitStat(playerData.MaxHP, playerData.AttackPoint, playerData.DefencePoint, playerData.SpeedPoint);

        //플레이어 스킬 설정
        player.setPlayerSkillList(playerData.CombatSkill, playerData.MoveSkill);

        //현재 스탯 설정
        player.BattleStartStat();
        enemy.BattleStartStat();

        //적 첫 스킬 예약
        enemy.EnemySkillListReady();

        if (initialStat.StartingSkill != null) enemyReservationSkill = enemy.findSkill(initialStat.StartingSkill);
        else enemyReservationSkill = enemy.ReservationSkill(distance);

        //HUD 설정
        setHUDAll();
        playerHUD.SetSkillButton(player.combatSkillList);
        playerHUD.SetSkillButton(player.moveSkillList);

        //속도 비교 후 선공권
        state = CompareSpeed();

        if (state == BattleState.PLAYERTURN) PlayerTurn();
        else EnemyTurn();
    }

    private void setHUDAll()
    {
        playerHUD.SetHUD(player);
        playerHUD.SetHPSlider(player.currentHitPoint);
        enemyHUD.SetHUD(enemy);
        enemyHUD.SetHPSlider(enemy.currentHitPoint);
        enemyHUD.SetEnemySprite(enemy, distance);
    }

    private void PlayerTurn()
    {
        Debug.Log("플레이어 턴!");
        player.CoolTimeSet();
        player.DicreaseEffectDuration();
        player.TurnResetStat();
        player.ActivateEffect();

        playerHUD.SetButtonActivated(distance);

        setHUDAll();

        state = BattleState.PLAYERTURN;
    }

    private void EnemyTurn()
    {
        enemy.CoolTimeSet();
        enemy.DicreaseEffectDuration();
        enemy.TurnResetStat();
        enemy.ActivateEffect();
        setHUDAll();

        state = BattleState.ENEMYTURN;
        if (enemy.isFrightened || enemy.isEnsnared || enemy.isParalysus) enemyReservationSkill = enemy.ReservationSkill(distance);
        isHit = CheckHitAttackDistance(enemyReservationSkill) ? true : false;
        logSystemManager.setTextInContents(enemyReservationSkill, isHit);

        if (enemyReservationSkill != null)
        {
            if (isHit) CaculateCombat(enemyReservationSkill);
            SkillCooltimeAndUsesSet(enemyReservationSkill);

        }

        setHUDAll();

        if (enemyReservationSkill != null && enemyReservationSkill.LINKSKILL != null)
        {
            enemyReservationSkill = enemy.linkSkillList.Find(x => x.NAME == enemyReservationSkill.LINKSKILL.SKILLNAME);
        }

        else
        {
            enemyReservationSkill = enemy.ReservationSkill(distance);
        }


        if (player.currentHitPoint <= 0)
        {
            CombatResultSetting();
            dataManager.UnLoadCombatScene(CombatResult.Lose);
        }

        else if(distance >= 6)
        {
            CombatResultSetting();
            dataManager.UnLoadCombatScene(CombatResult.Escape);
        }

        else
        {
            PlayerTurn();
        }
    }

    private BattleState CompareSpeed()
    {
        if (player.speed >= enemy.speed) return BattleState.PLAYERTURN;
        else return BattleState.ENEMYTURN;
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
                logSystemManager.SetDamageText(usingSkill, reservedDamageOrHealAmount);
                if (usingSkill.EFFECT.Length > 0)
                {
                    foreach (var effect in usingSkill.EFFECT)
                    {
                        if (effect.TARGET == StatusEffetSO.Target.Player) player.ApplyEffect(effect);
                        else enemy.ApplyEffect(effect);
                    }
                }
            }

            //나중에 상태이상 체크
            distance = Mathf.Abs(distance + usingSkill.MOVE);
            if (distance == 0) distance = 1;

            //이거 몬스터도 해야함!!
            //이동 확인
        }

        logSystemManager.startScroll();
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
            logSystemManager.setTextInContents(usingSkill, isHit);

            if(isHit) CaculateCombat(usingSkill);
            SkillCooltimeAndUsesSet(usingSkill);

            setHUDAll();
            clickButton.GetComponent<SkillButtonInfo>().UsedSkillSet(usingSkill);

            if (enemy.currentHitPoint <= 0)
            {
                CombatResultSetting();
                dataManager.UnLoadCombatScene(CombatResult.Win);
            }

            else if (distance >= 6)
            {
                CombatResultSetting();
                dataManager.UnLoadCombatScene(CombatResult.Escape);
            }

            else
            {
                EnemyTurn();
            }
        }
    }

    private void CombatResultSetting()
    {
        PlayerData playerData = GameObject.FindObjectOfType<PlayerData>();
        if (player.currentHitPoint <= 0) playerData.CurrentHP = 0;
        else playerData.CurrentHP = player.currentHitPoint;
        player.RemoveAllUsedSkill();
        playerData.CombatSkill = player.getCombatSkillList();
        playerData.MoveSkill = player.getMoveSkillList();
    }
}
