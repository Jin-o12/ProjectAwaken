/// <summary>
/// 배틀의 흐름을 관리 및 제어
/// </summary>
using UnityEngine;

public class BattleManager : MonoBehaviour
{
    [SerializeField] EntityDataManager entityDataManager;
    [SerializeField] DeckManager deckManager;

    public enemyCode nowEnemyCode = enemyCode.Test_enemy_1; //임시 테스트로 적 지정정
    public PlayerStatus player;
    public EnemyStatus enemy;

    void Start()
    {
        player = entityDataManager.BattleBegin_PlayerSetting();             // Entity 배치 및 세팅
        enemy = entityDataManager.BattleBegin_EnemySetting(nowEnemyCode);   // Entity 배치 및 세팅

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
