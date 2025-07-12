using UnityEngine;
using UnityEngine.EventSystems;

/// <summary>
/// 카드가 스냅되는 구역에 대한 이벤트 기능
/// </summary>
public class SnapZone : MonoBehaviour
{
    [Header("참조 컴포넌트")]
    [SerializeField] public Card_ReadyQueue readyQueue;
    [SerializeField] public Card_HandListUI handListUI;
    [SerializeField] public BattleCardPileUI battleCardPileUI;

    // public void OnDrop(PointerEventData eventData)
    // {
    //     GameObject droppedObj = eventData.pointerDrag;

    //     // 드롭 성공 여부 설정
    //     Draggable draggable = droppedObj.GetComponent<Draggable>();
    //     if (draggable != null)
    //     {
    //         draggable.wasDropped = true;
    //     }

    //     if (droppedObj != null && droppedObj.CompareTag("Card"))
    //     {
            
    //     }
    // }

    public void CardDropped(GameObject card)
    { 
        if (gameObject.name == "Ready_SnapZoneBox")
            {
                readyQueue.AddCard(card);                      // 카드 데이터 이동
            }
            else if (gameObject.name == "Hand_SnapZoneBox")
            {
                handListUI.AddCard();
            }
    }
}
