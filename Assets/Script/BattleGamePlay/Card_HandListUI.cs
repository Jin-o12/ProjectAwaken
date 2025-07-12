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

    public void AddCard()
    {
        battleCardPileUI.PutDownHandCard();
    }
}
