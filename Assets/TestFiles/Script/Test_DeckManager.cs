/// <summary>
/// 한 게임 세션동안 유지되는 모든 카드들의 데이터를 저장 및 수정
/// > 참조
///   └ CardDataManager: 카드에 대한 정보를 가져와 덱에 적재
///   └ CardUIController: 리스트의 값을 바탕으로 UI에 오브젝트 배치
/// </summary>
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Test_DeckManager : MonoBehaviour
{
    [SerializeField] public Test_CardDataManager cardDataManager;
    [SerializeField] public Test_EnemyBattleData enemyBattleData;
    [SerializeField] public Test_CardUIController cardUIController;
    [SerializeField] public Test_CardViewer cardViewer;

    /* 플레이어 손패, 덱, 버린덱 */
    private int handSize;    private int nextTurnDraw;
    private List<Card> playerHandList = new List<Card>();
    private List<Card> playerDeckList = new List<Card>();
    private List<Card> playerdiscardPile = new List<Card>();

    /* 적 정보 */
    private cardCord[][] EnemyActionList;

    /* Get Function */
    public List<Card> GetHandList()
    {
        return playerHandList;
    }

    ///------       전투       ------///

    /* 적 AI 불러오기 */
    public void SetEnemyActionList(enemyCode code)
    {
        EnemyActionList = enemyBattleData.GetBattleAction(code);
        /// 전체패턴과 턴당패턴을 구분하고, n번째 턴에 따른 다음 패턴의 실행 ///
    }

    /* 전투 시작시 덱과 카드 준비 */
    public void BattleBegin_CardSetting(enemyCode code)
    {
        handSize = GameConstants.handSize;
        nextTurnDraw = 1;

        for (int c = 0; c < handSize; c++)
        {
            DrawCardFromDeck();
        }
        SetEnemyActionList(code);
    }

    /* 턴 종료시 다음 턴 준비 */
    public void GoToNextTurn()
    {
        ReadyEnemyAction();
        for (int i = 0; i < nextTurnDraw; i++)
        { 
            DrawCardFromDeck();
        }
            
    }

    /* 턴 당 적 행동 설정(List<Card>) */
    public void ReadyEnemyAction()
    {

    }

    /* 덱으로부터 카드 한장 드로우 */
    public void DrawCardFromDeck()
    {
        // 덱이 비었을 시
        if (!playerDeckList.Any())
        {
            // 덱도 비어 있으면 드로우 안함
            if (!playerdiscardPile.Any())
            {
                Debug.Log("Can't Draw Any Card!");
                return;
            }
            else
            {
                // 버린 덱의 모든 카드를 덱으로 되돌림
                foreach (Card card in playerdiscardPile)
                {
                    playerDeckList.Add(card);
                }
            }
        }
        //// 랜덤 드로우 기능 추가 할 것 /////
        int draw = 0;   // 랜덤 수치 대입 및 해당 카드 드로우 카드로 지정
        Debug.Log("playerDeckList-DrawCardFromDeck() playerDeckList[draw]: " + playerDeckList[draw].GetName());
        Card drawCard = playerDeckList[draw];
        cardUIController.DrawCardUIToHand(drawCard);

        playerDeckList.RemoveAt(draw);
        playerHandList.Add(drawCard);

        return;
    }

    /* 카드 트레쉬 */
    public void AddCardTrashPile(Card card)
    {
        playerHandList.Remove(card);
        playerdiscardPile.Add(card);
    }

    ///------       정보 갱신       ------///

    /* 덱 목록에 카드 추가 (새로운 카드) */
    public void AddNewCardToDeck(cardCord code)
    {
        Card copyCard = cardDataManager.GetCardDataByNum(code);
        cardUIController.MakeCardInstance(copyCard);
        playerDeckList.Add(copyCard);
    }
}
