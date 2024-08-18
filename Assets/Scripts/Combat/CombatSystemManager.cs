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

    //���� ���� ���� �� �ڿ� ����
    private BattleState state;
    private int distance;
    private bool isHit;
    private int reservedDamageOrHealAmount;

    //�÷��̾�, �� ��ų �� ���� ������ �������� ���� ����
    private Player player;
    private Enemy enemy;

    //�ӽ�
    public SkillSO playerSkill;
    public SkillSO playerSkill2; 
    public SkillSO playerSkill3;
    public SkillSO playerSkill4;

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
        player = GameObject.FindObjectOfType<Player>();
        enemy = GameObject.FindObjectOfType<Enemy>();

        //�ӽ÷� ����, ��ų ����
        player.InitStat(100, 10, 5, 3);
        enemy.InitStat(100, 10, 2, 5);
        player.setSkill(playerSkill);
        player.setSkill(playerSkill2); 
        player.setSkill(playerSkill3);
        player.setSkill(playerSkill4);

        //���� ���� ����
        player.BattleStartStat();
        enemy.BattleStartStat();

        //�Ÿ� ���� ������ ���Ƿ� �������� �Ŀ� ��� ������ ������
        distance = SetDistance();

        //�� ù ��ų ����
        enemy.EnemySkillListReady();
        enemyReservationSkill = enemy.ReservationSkill(distance);

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

        if (enemyReservationSkill != null)
        {
            isHit = CheckHitAttackDistance(enemyReservationSkill) ? true : false;
            if(isHit) CaculateCombat(enemyReservationSkill);
            SkillCooltimeAndUsesSet(enemyReservationSkill);

        }

        if (enemyReservationSkill == null) Debug.Log("�ƹ� �ൿ ����! ����!");
        
        //�� ������ ���� �ѱ�ǵ� ���ེų�� null�̸� �ƹ��ൿ�����ʾҴ�! isHit�� �Ѱܼ� ���߽� �����߽� üũ, �׳� ��ų�� �Ѱ����Ƿ� ���� ������ �Ѳ�����
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
            //�й� �̺�Ʈ
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
        //������ �� �̹��� �Ÿ� ����ϴ� �Լ� �ֱ�
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
            
            if(isHit) CaculateCombat(usingSkill);
            SkillCooltimeAndUsesSet(usingSkill);

            setHUDAll();

            clickButton.GetComponent<SkillButtonInfo>().UsedSkillSet(usingSkill);
            playerHUD.SetButtonActivated();

            if (enemy.currentHitPoint <= 0)
            {
                Debug.Log("�÷��̾� �¸�");
                //�¸� �̺�Ʈ �߰�
            }

            else
            {
                EnemyTurn();
            }
        }
    }
}
