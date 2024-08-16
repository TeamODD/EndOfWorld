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

    private List<SkillDB> playerCombatSkillList;
    private List<SkillDB> playerMoveSkillList;

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

        //���� ���� ����
        player.BattleStartStat();
        enemy.BattleStartStat();

        //�Ÿ� ���� ������ ���Ƿ� �������� �Ŀ� ��� ������ ������
        distance = SetDistance();
        
        //�÷��̾� ��ų ��������
        playerCombatSkillList = player.GetCombatSkillList();
        playerMoveSkillList = player.GetMoveSkillList();

        //�� ù ��ų ����
        enemy.EnemySkillListReady();
        enemy.ReservationSkill(distance);

        //HUD ����
        setHUDAll();
        //playerHUD.SetSkillButton(playerSkillList);

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

        setHUDAll();

        state = BattleState.PLAYERTURN;
        Debug.Log("�÷��̾� ��!");
    }

    private void EnemyTurn()
    {
        Debug.Log("���� ��");
        enemy.DicreaseEffectDuration();
        enemy.TurnResetStat();
        enemy.ActivateEffect();

        setHUDAll();

        state = BattleState.ENEMYTURN;
        
        //����⸦ ��� �ؾ��ұ�
        if (enemy.isFrightened && enemy.reservationSkill.TYPE == SkillSO.SkillType.combatSkill) enemy.ReservationSkill(distance);
        if (enemy.isEnsnared && enemy.reservationSkill.TYPE == SkillSO.SkillType.moveSkill) enemy.ReservationSkill(distance);
        if (enemy.isParalysus) enemy.ReservationSkill(distance);

        CaculateCombat(enemy.reservationSkill);

        if(player.currentHitPoint <= 0)
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
    }

    private void CaculateCombat(SkillDB usingSkill)
    {
        //���� ��Ÿ��� ��Ҵ°�?�� ����� �ʿ��ҵ��ϴ�.
        //�ؾ�����
        //��Ÿ� ���
        //�ʱ� �Ÿ�
        //�÷��̾� ��ư
        //��ư ���Ƚ�� ���� ����
        //���� ����� ó�� lastattack ó�� ���ߴ�
        //���߽� ȿ���ߵ� ���� �� �� ��ũ��Ʈ���� ���� ó���ؾ��ҵ���
        //�� ��������� ���� �� ó��
        //�ؽ�Ʈ ó��
        //�̰͵� ���ϸ� �׽�Ʈ�غ��� ������ ��

        Debug.Log(usingSkill.NAME);
        if (usingSkill != null)
        {
            for (int i = 0; i < usingSkill.NUMOFATTACK; i++)
            {
                int receiveDamage = usingSkill.TARGET == SkillSO.Target.Player ? player.CombatSkillActivate(usingSkill) : enemy.CombatSkillActivate(usingSkill);
                Debug.Log(usingSkill.TARGET + " " + receiveDamage);
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

            //���Ƚ��, ��Ÿ�� ����
            usingSkill.COOLTIME = usingSkill.MAXCOOLTIME;
            usingSkill.USES--;

            //HUD �����ϱ�(����� ��ų�� ��ư ��Ÿ��, ���Ƚ�� ������ ���� �����ؾ��Ѵ�.)
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

            //�׾����� ���׾����� Ȯ��
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
