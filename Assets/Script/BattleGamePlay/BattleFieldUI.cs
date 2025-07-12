using UnityEngine;
public class BattleFieldUI : MonoBehaviour
{
    [Header("UI_EntityField 속성")]
    [SerializeField] public RectTransform EntityFieldUI;

    [Header("Prefaps")]
    [SerializeField] GameObject entityPrefab;

    private PlayerStatus player;
    private EnemyStatus enemy;

    private GameObject playerObj;
    private GameObject enemyObj;

    private EntityStatusViewer playerViewer;
    private EntityStatusViewer enemyViewer;



    /* Entity 위치에 대한 고정 상수 값 (초기 배치) */
    private const float PLAYER_X_POS = 200.0f;
    private const float PLAYER_Y_POS = 500.0f;
    private const float ENEMY_X_POS = 1080.0f;
    private const float ENEMY_Y_POS = 500.0f;

    public PlayerStatus GetPlayerStatus() { return player; }

    public EnemyStatus GetEnemyStatus() { return enemy; }


    /* (프리팹, 판넬UI)를 바탕으로 오브젝트 생성 및 위치 지정 */
    public void BattleUISetting_Entity()
    {
        // 플레이어
        player = EntityDataLoader.GetPlayerStatusById(GameConstants.playerCode);
        playerObj = Instantiate(entityPrefab, EntityFieldUI);
        playerViewer = playerObj.GetComponent<EntityStatusViewer>();
        playerViewer.Setup(player);
        // 플레이어 오브젝트 위치   
        RectTransform playerRt = playerObj.GetComponent<RectTransform>();
        playerRt.anchoredPosition = new Vector2(PLAYER_X_POS, PLAYER_Y_POS);

        // 상대
        enemy = EntityDataLoader.GetEnemyStatusById(GameConstants.nowBattleEnemyCode);                              // 적 개체(클래스) 생성
        enemyObj = Instantiate(entityPrefab, EntityFieldUI);                                                        // 적 오브젝트 생성
        enemyViewer = enemyObj.GetComponent<EntityStatusViewer>();
        enemyViewer.Setup(enemy);
        GameConstants.nowBattleEnemy = EntityDataLoader.GetEnemyStatusById(GameConstants.nowBattleEnemyCode);       // 현재 배틀 적 개체(클래스) 생성 => 중복으로 삭제 고려중
        // 상대 오브젝트 위치
        RectTransform enemyRt = enemyObj.GetComponent<RectTransform>();                                             // 적 개체 위치값
        enemyRt.anchoredPosition = new Vector2(ENEMY_X_POS, ENEMY_Y_POS);                                           // 적 개체 위치 지정

        UpdateEntityView();
    }

    public void UpdateEntityView()
    {
        playerViewer.RewritingStatusUI();
        enemyViewer.RewritingStatusUI();
        return;
    }
}
