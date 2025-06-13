using UnityEngine;
/// <summary>
/// 배틀의 흐름을 관리 및 제어
/// </summary>
public class BattleManager : MonoBehaviour
{
    public static EntityCord nowEnemyCode = GameConstants.nowBattleEnemy;
    public PlayerStatus player = GameConstants.playerData;
    public EnemyStatus enemy = EntityDataLoader.GetEnemyStatusById(nowEnemyCode);

    /* 한 턴 혹은 전투동안 사용되는 일회성 값 */
    private int chainStack;                 // 연계 가능 스텍 수치
    private int nextTurnDraw;               // 다음 차례 드로우 예정 카드

    /// <summary>
    /// 
    /// </summary>
    void Start()
    {
    
    }

    void Update()
    {

    }
}
