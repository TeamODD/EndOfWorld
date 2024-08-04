using System.Collections;
using System.Collections.Generic;
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

    //�÷��̾�, �� ��ų �� ���� ������ �������� ���� ����
    private Player player;
    private Enemy enemy;

    private List<SkillDB> playerSkillList;

    //�ӽ�
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
        
        //�ӽ÷� ����, ��ų ����
        player.InitStat(100, 10, 5, 3);
        enemy.InitStat(100, 10, 2, 1);
        player.setSkill(playerSkill);
        player.setSkill(playerSkill2);

        //���� ���� ����
        player.BattleStartStat();
        enemy.BattleStartStat();

        //�÷��̾� ��ų ��������
        //playerSkillList = player.GetSkillList();
        
        //HUD ����
        playerHUD.SetHUD(player);
        enemyHUD.SetHUD(enemy);
        playerHUD.SetSkillButton(playerSkillList);
        
        //�Ÿ� ���� ������ ���Ƿ� �������� �Ŀ� ��� ������ ������
        distance = SetDistance();

        //�ӵ� �� �� ������
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
        Debug.Log("�÷��̾� ��!");
    }

    private void EnemyTurn()
    {/*
        state = BattleState.ENEMYTURN;
        Debug.Log("�� ��!");

        SkillSO enemyAttackSkill = enemy.Attack(distance, player);
        //����Ƚ�� �߰�
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


    //��ų ���� -> ������ ��Ҵ��� üũ, ��Ҵٸ� ������ üũ, ���� ���� �����(��, ���� �Ѵ�) üũ, �̵��� �Ǿ����� üũ, ���� ��ų Ƚ�� üũ, ����� ���� �Ϸ�� ��ų�� ��Ȱ��ȭ ����
    public void OnSkillButton()
    {
        if (state != BattleState.PLAYERTURN)
            return;

        GameObject clickButton = EventSystem.current.currentSelectedGameObject;
        if (clickButton != null)
        {
            int skillIndex = clickButton.GetComponent<SkillButtonInfo>().getSkillIndex();
            SkillDB usingSkill = playerSkillList[skillIndex];

            //����� ��ų�� ���� ����, ����, ����� ���� �����������
            //bool isDead = enemy.AttackedByEnemy(usingSkill.DAMAGE);

            if (usingSkill.EFFECT != null)
            {
                foreach(StatusEffetSO effect in usingSkill.EFFECT)
                {
                    player.ApplyEffect(effect);
                }
            }
            playerHUD.SetHUD(player);

            //�̵��� ���Ŀ� �׸��� �̷� ������ �÷��̾� ����, �� ���� �Լ��� ���� ��� ����
            //��ų ����ϸ� ��ü��üũ, ����üũ, HUD������ �Ѳ����� �̷�������Ѵ�.
            enemyHUD.SetHPSlider(enemy.currentHitPoint);

            if (true)
            {
                Debug.Log("�÷��̾� �¸�!");
                //player.SetSkillList(playerSkillList);
            }

            else
            {
                //EnemyTurn();
            }

        }

    }

}
