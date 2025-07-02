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

    CardInfo[] cardInfoList;                    // 대기열
    int nowSlot;

    void Awake()
    {
        cardInfoList = new CardInfo[GameConstants.readyQueueSize];
        for (int i = 0; i < cardInfoList.Length; i++)
        {
            cardInfoList[i] = null;
        }
        nowSlot = 0;
    }

    /* 카드 추가 */
    public void AddCard(PointerEventData card)
    {
        cardInfoList[nowSlot] = card.pointerDrag.GetComponent<CardViewer>().GetCardInfo();  //카드 데이터 저장
        battleCardPileUI.AddCardInReadyFromHand(card.pointerDrag, nowSlot);
        nowSlot++;
    }

    /* 카드 제거 */
    public int RemoveCard(PointerEventData card, GameObject snapZone)
    {
        int removeSlot = -1;
        // 동일 카드를 배열 내에서 찾음
        for (int i = 0; i < cardInfoList.Length; i++)
        {
            if (cardInfoList[i]!=null && cardInfoList[i].GetID() == card.pointerDrag.GetComponent<CardViewer>().GetCardInfo().GetID())
                removeSlot = i;
        }

        if (removeSlot == -1)
            Debug.LogWarning("제거 대상 카드가 존재하지 않음");
        cardInfoList[removeSlot] = null;  //카드 데이터 제거

        // UI 상에서 카드 오브젝트 이동
        /// 여기에 코드 추가 ///
        
        nowSlot--;
        return nowSlot;
    }

    public void PlayCardAction()
    {
        for (int i = 0; i < cardInfoList.Length; i++)
        {
            CardInfo card = cardInfoList[i];
            EntityStatus target = null;

            // 대상 지정
            if (card.GetTarget() == "Enemy")
                target = battleFieldUI.GetPlayerStatus();
            else if (card.GetTarget() == "Player")
                target = battleFieldUI.GetEnemyStatus();

            // 대상 존재시 실행
            if (target != null)
                card.Activate(target, card.GetValue());
            else
                Debug.Log("Card_ReadyQueue: target has no exist");
        }
    }
}
