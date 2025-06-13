using UnityEngine;
/// <summary>
/// 배틀의 흐름을 관리 및 제어
/// </summary>
public class Test_BattleManager : MonoBehaviour
{
    [SerializeField] Test_EntityDataManager entityDataManager;
    [SerializeField] Test_DeckManager deckManager;

    public enemyCode nowEnemyCode = enemyCode.Test_enemy_1; //임시 테스트로 적 지정
    public tPlayerStatus player;
    public tEnemyStatus enemy;

    void Start()
    {
        player = entityDataManager.BattleBegin_PlayerSetting();             // Player Entity 배치 및 세팅
        enemy = entityDataManager.BattleBegin_EnemySetting(nowEnemyCode);   // Enemy Entity 배치 및 세팅

        /// 임시 초기덱, 게임 시작 기능과 함께 초기 덱 기능 구현 예정 ///
        deckManager.AddNewCardToDeck(cardCord.Card_char1_Slash);
        deckManager.AddNewCardToDeck(cardCord.Card_char1_QuickBash);
        deckManager.AddNewCardToDeck(cardCord.Card_char1_Counter);
        deckManager.AddNewCardToDeck(cardCord.Card_char1_Spill);

        deckManager.BattleBegin_CardSetting(nowEnemyCode);                  // 전투 시작시 덱과 카드 준비
    }

    void Update()
    {

    }
}




