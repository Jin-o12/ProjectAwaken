using System.Collections.Generic;

/// <summary>
/// 게임 플레이와 관련된 모든 값을 저장합니다
/// </summary>
public class GameConstants
{
    /* 플레이어 설정 */
    public static PlayerStatus playerData;
    public static EntityCode playerCode = 0;
    public static int handSize = 5;                            // 플레이어 손패 최대치
    public static int readyQueueSize = 5;                      // 대기 큐 적재 최대치
    public static Dictionary<int, CardInfo> DeckList = new Dictionary<int, CardInfo>();          // 세션 당 덱 리스트 <덱 내 고유 번호, 고유 카드 정보>


    /* 현재 게임 상태 */
    public static EntityCode nowBattleEnemyCode = 0;
    public static EnemyStatus nowBattleEnemy = null;


    /* 게임 기본 설정 - 전투 */
    public static int numberOfReadySlots = 5;
}
