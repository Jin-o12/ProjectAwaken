using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

/// <summary>
/// 대기 큐의 카드 정보 관리 및 실행
/// </summary>
public class Card_ReadyQueue : MonoBehaviour
{
    [Header("참조 컴포넌트")]
    [SerializeField] public BattleFieldUI battleFieldUI;
    [SerializeField] public BattleCardPileUI battleCardPileUI;

    List<CardViewer> cardInfoList;
    int nowSlot;

    void Awake()
    {
        cardInfoList = battleCardPileUI.GetCardInQueue();
        nowSlot = 0;
    }

    /* 카드 추가 */
    public void AddCard(GameObject cardObject)
    {
        battleCardPileUI.PutDownReadyCard();
        cardInfoList = battleCardPileUI.GetCardInQueue();
    }

    public void RemoveCard(GameObject cardObject)
    {
        cardInfoList = battleCardPileUI.GetCardInQueue();
    }

    public void PlayCardAction()
    {
        cardInfoList = battleCardPileUI.GetCardInQueue();
        foreach (CardViewer listCard in cardInfoList)
        {
            CardInfo card = listCard.GetCardInfo();
            EntityStatus target = null;

            // 대상 지정
            if (card.GetTarget() == "Player")
                target = battleFieldUI.GetPlayerStatus();
            else if (card.GetTarget() == "Enemy")
                target = battleFieldUI.GetEnemyStatus();

            // 대상 존재시 실행
            if (target != null)
                card.Activate(target, card.GetValue());
            else
                Debug.Log("Card_ReadyQueue: target has no exist");
        }
        battleFieldUI.UpdateEntityView();                    // 게임 화면 갱신
    }
}
