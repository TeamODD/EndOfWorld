using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.iOS.Xcode;
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
        
        //임시로 스탯, 스킬 설정
        player.InitStat(100, 10, 5, 3);
        enemy.InitStat(100, 10, 2, 1);
        player.ApplySkill(playerSkill);
        player.ApplySkill(playerSkill2);

        //현재 스탯 설정
        player.BattleStartStat();
        enemy.BattleStartStat();

        //플레이어 스킬 가져오기
        playerSkillList = player.GetSkillList();
        
        //HUD 설정
        playerHUD.SetHUD(player);
        playerHUD.SetSkillButton(playerSkillList);
        enemyHUD.SetHUD(enemy);

        //거리 설정 지금은 임의로 정하지만 후에 어떻게 설정할 것인지
        distance = 2;

        //속도 비교 후 선공권
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
        Debug.Log("플레이어 턴!");
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

            //사용한 스킬에 따라 공격, 버프, 디버프 등을 설정해줘야함
            bool isDead = enemy.AttackedByEnemy(usingSkill.DAMAGE);
            if (usingSkill.EFFECT != null) player.ApplyEffect(usingSkill.EFFECT);
            playerHUD.SetHUD(player);

            //이동은 추후에 그리고 이런 공격은 플레이어 공격, 적 공격 함수로 따로 떼어낼 예정
            //스킬 사용하면 적체력체크, 버프체크, HUD설정이 한꺼번에 이루어져야한다.
            enemyHUD.SetHPSlider(enemy.currentHitPoint);

            if(isDead)
            {
                Debug.Log("플레이어 승리!");
            }

            else
            {
                EnemyTurn();
            }

        }

        //클릭한 버튼의 인덱스 번호를 가져온다. 인덱스 번호는 버튼을 생성할때 따로 0부터 변수로 설정할 예정
        //그럼 버튼인포 클래스는 버튼의 이미지, 사용횟수, 쿨타임, 사용가능불가능 등을 맡도록 하자

        //그럼 내일 할일은 플레이어 현재 가지고 있는 스킬 개수에 따라 버튼 생성
        //각 버튼에다 버튼인포를 에드컴포넌트 해주고 정보 넣기
        //이후 그 버튼을 누르면 인덱스가 반환되며 인덱스로 어떤 스킬인지 가져오기
        //사용한 스킬에 대한 정보도 따로 저장하고 전투가 끝나면 플레이어 스크립트의 list를 지금 정보로 덮어씌우자 이게맞다

        //가져온 스킬들을 UI 배치할 예정
        //배치는 버튼을 생성하고 거기에 정보를 담을 것
        //그럴려면 버튼에서 정보를 가져와야한다.
        //그럼 버튼에 있는 정보를 고쳤을 때 player의 리스트에도 영향이 가야함
        //index로? 1번 버튼 2번 버튼 눌리면 1번 스킬 2번 스킬 이런식으로? 이렇게 해야겠다 그럼 음 
    }

    private void EnemyTurn()
    {
        state = BattleState.ENEMYTURN;
        Debug.Log("적 턴!");
        player.AttackedByEnemy(enemy.Attack(distance).DAMAGE);
        playerHUD.SetHPSlider(player.currentHitPoint);
        PlayerTurn();
    }

    private void Start()
    {
        BattleStart();
    }
}
