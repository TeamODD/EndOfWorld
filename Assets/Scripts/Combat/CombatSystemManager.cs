using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public enum BattleState { START, PLAYERTURN, ENEMYTURN, WIN, LOSE }

public class CombatSystemManager : MonoBehaviour
{
    //�̱���
    private static CombatSystemManager instance;

    //�÷��̾� �� �� HUD �Ŵ��� ����
    [SerializeField]
    private CombatHUDManager playerHUD;
    [SerializeField]
    private CombatHUDManager enemyHUD;
    [SerializeField]
    private CombatLogSystemManager logSystemManager;
    [SerializeField]
    private GameObject enemyParent;

    //���� ���� ���� �� �ڿ� ����
    private BattleState state;
    private int distance;
    private bool isHit;
    private int reservedDamageOrHealAmount;
    private SceneTransitionManager dataManager;

    //�÷��̾�, �� ��ų �� ���� ������ �������� ���� ����
    private Player player;
    private Enemy enemy;

    //�� ���� ��ų
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
        //���� ������ �� ���� ��Ȳ ��������
        dataManager = GameObject.FindObjectOfType<SceneTransitionManager>();
        Instantiate(dataManager.EnemyData, enemyParent.transform);
        enemy = GameObject.FindObjectOfType<Enemy>();
        InitialStat initialStat = enemy.GetComponent<InitialStat>();

        enemy.InitStat(initialStat.MaxHP, initialStat.AttackPoint, initialStat.DefencePoint, initialStat.SpeedPoint);
        distance = initialStat.StartingDistance;

        //�÷��̾� ������ ��������
        PlayerData playerData = GameObject.FindObjectOfType<PlayerData>();
        player = GameObject.FindObjectOfType<Player>();
        player.InitStat(playerData.MaxHP, playerData.AttackPoint, playerData.DefencePoint, playerData.SpeedPoint);

        //�÷��̾� ��ų ����
        player.setPlayerSkillList(playerData.CombatSkill, playerData.MoveSkill);

        //���� ���� ����
        player.BattleStartStat();
        enemy.BattleStartStat();

        //�� ù ��ų ����
        enemy.EnemySkillListReady();
        if (initialStat.StartingSkill != null) enemyReservationSkill = enemy.findSkill(initialStat.StartingSkill);
        else enemyReservationSkill = enemy.ReservationSkill(distance);

        //HUD ����
        setHUDAll();
        playerHUD.SetSkillButton(player.combatSkillList);
        playerHUD.SetSkillButton(player.moveSkillList);

        //�ӵ� �� �� ������
        state = CompareSpeed();

        if (state == BattleState.PLAYERTURN) PlayerTurn();
        else EnemyTurn();
    }

    private void PlayerTurn()
    {
        Debug.Log("�÷��̾� ��!");
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
        Debug.Log("���� ��");
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
            if(isHit) CaculateCombat(enemyReservationSkill);
            SkillCooltimeAndUsesSet(enemyReservationSkill);
            
        }
        
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
            Debug.Log("���� �¸�");
            GetComponent<PlayerData>().CurrentHP = 0;
            player.RemoveAllUsedSkill();
            GetComponent<PlayerData>().CombatSkill = player.getCombatSkillList();
            GetComponent<PlayerData>().MoveSkill = player.getMoveSkillList();
            dataManager.UnLoadCombatScene(CombatResult.Lose);
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
        enemyHUD.SetEnemySprite(enemy, distance);
    }

    //��Ÿ� üũ
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
                    Debug.Log("����/����� �ߵ�");
                    foreach (var effect in usingSkill.EFFECT)
                    {
                        if (effect.TARGET == StatusEffetSO.Target.Player) player.ApplyEffect(effect);
                        else enemy.ApplyEffect(effect);
                    }
                }
            }

            //�̵� Ȯ��
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
            logSystemManager.setTextInContents(usingSkill, isHit);

            if(isHit) CaculateCombat(usingSkill);
            SkillCooltimeAndUsesSet(usingSkill);

            setHUDAll();
            clickButton.GetComponent<SkillButtonInfo>().UsedSkillSet(usingSkill);

            if (enemy.currentHitPoint <= 0)
            {
                Debug.Log("�÷��̾� �¸�");
                GetComponent<PlayerData>().CurrentHP = player.currentHitPoint;
                player.RemoveAllUsedSkill();
                GetComponent<PlayerData>().CombatSkill = player.getCombatSkillList();
                GetComponent<PlayerData>().MoveSkill = player.getMoveSkillList();
                dataManager.UnLoadCombatScene(CombatResult.Win);
            }

            else
            {
                EnemyTurn();
            }
        }
    }
}
