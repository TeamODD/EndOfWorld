using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.iOS.Xcode;
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

    private void Awake()
    {
        if(instance == null)
            instance = this;

        else
        {
            Destroy(this.gameObject);
        }
    }

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
        player.ApplySkill(playerSkill);
        player.ApplySkill(playerSkill2);

        //���� ���� ����
        player.BattleStartStat();
        enemy.BattleStartStat();

        //�÷��̾� ��ų ��������
        playerSkillList = player.GetSkillList();
        
        //HUD ����
        playerHUD.SetHUD(player);
        playerHUD.SetSkillButton(playerSkillList);
        enemyHUD.SetHUD(enemy);

        //�Ÿ� ���� ������ ���Ƿ� �������� �Ŀ� ��� ������ ������
        distance = 2;

        //�ӵ� �� �� ������
        state = CompareSpeed();
        if (state == BattleState.PLAYERTURN) PlayerTurn();
        else EnemyTurn();
    }

    private BattleState CompareSpeed()
    {
        if(player.speed >= enemy.speed) return BattleState.PLAYERTURN;
        else return BattleState.ENEMYTURN;
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

    public void OnSkillButton()
    {
        if (state != BattleState.PLAYERTURN)
            return;

        GameObject clickButton = EventSystem.current.currentSelectedGameObject;
        if(clickButton != null)
        {
            int skillIndex = clickButton.GetComponent<SkillButtonInfo>().getSkillIndex();
            SkillDB usingSkill = playerSkillList[skillIndex];

            //����� ��ų�� ���� ����, ����, ����� ���� �����������
            bool isDead = enemy.AttackedByEnemy(usingSkill.DAMAGE);
            if (usingSkill.EFFECT != null) player.ApplyEffect(usingSkill.EFFECT);
            playerHUD.SetHUD(player);

            //�̵��� ���Ŀ� �׸��� �̷� ������ �÷��̾� ����, �� ���� �Լ��� ���� ��� ����
            //��ų ����ϸ� ��ü��üũ, ����üũ, HUD������ �Ѳ����� �̷�������Ѵ�.
            enemyHUD.SetHPSlider(enemy.currentHitPoint);

            if(isDead)
            {
                Debug.Log("�÷��̾� �¸�!");
            }

            else
            {
                EnemyTurn();
            }

        }

        //Ŭ���� ��ư�� �ε��� ��ȣ�� �����´�. �ε��� ��ȣ�� ��ư�� �����Ҷ� ���� 0���� ������ ������ ����
        //�׷� ��ư���� Ŭ������ ��ư�� �̹���, ���Ƚ��, ��Ÿ��, ��밡�ɺҰ��� ���� �õ��� ����

        //�׷� ���� ������ �÷��̾� ���� ������ �ִ� ��ų ������ ���� ��ư ����
        //�� ��ư���� ��ư������ ����������Ʈ ���ְ� ���� �ֱ�
        //���� �� ��ư�� ������ �ε����� ��ȯ�Ǹ� �ε����� � ��ų���� ��������
        //����� ��ų�� ���� ������ ���� �����ϰ� ������ ������ �÷��̾� ��ũ��Ʈ�� list�� ���� ������ ������� �̰Ը´�

        //������ ��ų���� UI ��ġ�� ����
        //��ġ�� ��ư�� �����ϰ� �ű⿡ ������ ���� ��
        //�׷����� ��ư���� ������ �����;��Ѵ�.
        //�׷� ��ư�� �ִ� ������ ������ �� player�� ����Ʈ���� ������ ������
        //index��? 1�� ��ư 2�� ��ư ������ 1�� ��ų 2�� ��ų �̷�������? �̷��� �ؾ߰ڴ� �׷� �� 
    }

    private void EnemyTurn()
    {
        state = BattleState.ENEMYTURN;
        Debug.Log("�� ��!");
        player.AttackedByEnemy(enemy.Attack(distance).DAMAGE);
        playerHUD.SetHPSlider(player.currentHitPoint);
        PlayerTurn();
    }

    private void Start()
    {
        BattleStart();
    }
}
