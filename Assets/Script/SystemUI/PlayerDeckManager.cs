using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 한 세션 내 플레이어의 덱과 카드 관리를 위한 스크립트
/// </summary>
public class PlayerDeckManager : MonoBehaviour
{
    public void AddCardInDeckList(CardCode cardCode)
    {
        int deckSize = GameConstants.DeckList.Count;
        CardInfo card = CardDataLoader.GetCardInfoByCode(cardCode); // 코드 바탕으로 카드 서치
        card.SetID(deckSize);                                       // 카드간에 구분(동일 카드 코드여도 따로 취급)을 위한 고유 코드
        GameConstants.DeckList[deckSize] = card;
    }
}
