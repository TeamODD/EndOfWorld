using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
/// <summary>
/// 인카운터 씬과 전투 씬 간 공유 데이터 관리를 위한 매니저 클래스입니다.
/// 적, 시작 거리를 저장하며, 전투 씬을 Additive 모드로 로드/언로드합니다.
/// </summary>
public class SceneTransitionManager : MonoBehaviour
{
    // 필수 데이터 저장 (적)
    public GameObject EnemyData;

    /// <summary>
    /// 전투 씬이 로드/언로드 됐을 때 활성화/비활성화 할 인카운터 UI입니다.
    /// </summary>
    [SerializeField]
    private GameObject encounterUI;

    [SerializeField]
    private GameObject screenUI;

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);    
    }
    /// <summary>
    /// 인카운터 씬에서 사용하는 전투 씬 로드 함수입니다.
    /// 적과 시작 거리를 인자로 전달하여 변수를 변경한 뒤 전투 씬을 로드합니다.
    /// </summary>
    /// <param name="startingDistance"></param>
    public void LoadCombatScene(GameObject enemyData)
    {
        EnemyData=enemyData;
        encounterUI.SetActive(false);
        screenUI.SetActive(false);
        SceneManager.LoadScene("CombatScene", LoadSceneMode.Additive);
    }
    /// <summary>
    /// 전투 씬을 언로드 함수입니다.
    /// 전투 씬에서 전투 결과를 인자로 전달하여 전투씬을 언로드한 뒤 전투 결과를 이벤트로 발생시킵니다. (사용이 어려울 시 전투 결과를 변수로 변경)
    /// </summary>
    /// <param name="combatResult"></param>
    public void UnLoadCombatScene(CombatResult combatResult)
    {
        SceneManager.UnloadSceneAsync("CombatScene");
        encounterUI.SetActive(true);
        screenUI.SetActive(true);
        OnCombatEnd.Invoke(combatResult);
    }

    public UnityEvent<CombatResult> OnCombatEnd;
}
public enum CombatResult
{
    Win,
    Lose,
    Escape
}