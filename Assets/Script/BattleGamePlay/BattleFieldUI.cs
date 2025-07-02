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
    

    /* Entity 위치에 대한 고정 상수 값 (초기 배치) */
    private const float PLAYER_X_POS = 200.0f;
    private const float PLAYER_Y_POS = 500.0f;
    private const float ENEMY_X_POS = 1080.0f;
    private const float ENEMY_Y_POS = 500.0f;

    public PlayerStatus GetPlayerStatus()   { return player; }

    public EnemyStatus GetEnemyStatus()     { return enemy; }


    /* (프리팹, 판넬UI)를 바탕으로 오브젝트 생성 및 위치 지정 */
    public void BattleUISetting_Entity()
    {
        // 플레이어
        playerObj = Instantiate(entityPrefab, EntityFieldUI);
        RectTransform playerRt = playerObj.GetComponent<RectTransform>();
        playerRt.anchoredPosition = new Vector2(PLAYER_X_POS, PLAYER_Y_POS);

        // 상대
        enemy = EntityDataLoader.GetEnemyStatusById(GameConstants.nowBattleEnemyCode);
        if (GameConstants.nowBattleEnemyCode == 0)
            Debug.Log("Battle Enemy Code is null: " + GameConstants.nowBattleEnemy);
        enemyObj = Instantiate(entityPrefab, EntityFieldUI);
        RectTransform enemyRt = enemyObj.GetComponent<RectTransform>();
        enemyRt.anchoredPosition = new Vector2(ENEMY_X_POS, ENEMY_Y_POS);
    }
}
