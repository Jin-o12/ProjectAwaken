using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

/// <summary>
/// 손패 카드 관리
/// </summary>
public class Card_HandListUI : MonoBehaviour
{
    [SerializeField] private BattleCardPileUI battleCardPileUI;
    public void Awake()
    {
        battleCardPileUI = GetComponentInParent<BattleCardPileUI>();
    }

    public void AddCard(PointerEventData card)
    {
        // cardInfoList[nowSlot] = card.pointerDrag.GetComponent<CardViewer>().GetCardInfo();  //카드 데이터 저장
        // battleCardPileUI.AddCardInReadyFromHand(card.pointerDrag, nowSlot);
        // nowSlot++;
    }

    /* 카드 제거 */
    public void RemoveCard(PointerEventData card, GameObject snapZone)
    {
        // cardInfoList[nowSlot] = card.pointerDrag.GetComponent<CardViewer>().GetCardInfo();  //카드 데이터 저장
        // nowSlot--;
        // return nowSlot;
    }
}
