using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CardUIController : MonoBehaviour
{
    [Header("참조 스크립트")]
    [SerializeField] DeckManager deckManager;
    [SerializeField] EntityDataManager entityDataManager;

    [Header("카드 UI")]
    [SerializeField] public Transform handPanel;         // 카드가 나열될 패널
    [SerializeField] public Transform ReadyQueuePanel;   // 카드 대기 패널
    [SerializeField] public GameObject cardPrefab;       // 카드 프리팹 (UI용)

    public Card cilkedCard = null;
    public int connectable;
    public int handCardNum = 0;                                             // 현재 손에 있는 카드 갯수

    /* Ready Slot */
    public int numberOfReadySlots = 5;                                      // Ready 슬롯 크기
    public RectTransform readySlotUI;                                       // Ready 슬롯 전체 UI
    public RectTransform slotPrefab;                                        // Ready 슬롯의 개별 슬롯
    public List<Card> cardInQueue = new List<Card>() { null };              // Ready 슬롯 내 카드 목록
    public List<RectTransform> slotsTrans = new List<RectTransform>();      // Ready 슬롯 내 각 스냅 슬롯의 Transform 정보

    void Start()
    {
        connectable = 0;
        InitReadyQueue();        // Ready Queue 기본 설정
    }

    /* 덱의 카드 손으로 드로우 */
    public void DrawCardUIToHand(Card card)
    {
        handCardNum++;

        // 카드 프리팹 데이터 가져와 배치 및 오브젝트 저장
        GameObject cardObj = Instantiate(cardPrefab, handPanel);
        card.SetCardObject(cardObj);
        ResetHandCardPos();
    }

    /* 전체 손패 위치 재정렬 */
    void ResetHandCardPos()
    {
        int order = 0;
        List<Card> cardList = deckManager.GetHandList();
        foreach (Card card in cardList)
        {
            float xPos = 0;
            float yPos = 0;

            //위치 계산
            RectTransform rt = card.GetCardObject().GetComponent<RectTransform>();
            xPos = (order - (handCardNum / 2)) * 50;
            rt.anchoredPosition = new Vector2(xPos, yPos);

            order++;
        }
    }

    void InitReadyQueue()
    {
        //스냅 구역에 대한 tranform, 가로세로 폭 저장
        RectTransform readyQueue = readySlotUI.GetComponent<RectTransform>();
        float width = readyQueue.rect.width;

        // 한 스냅의 위치(크기) = 슬롯의 가로길이 / 적재 가능 카드의 갯수
        float spacing = width / (numberOfReadySlots + 1);

        // 스냅 위치 적재량 만큼 복사 (오브젝트 생성)
        for (int i = 0; i < numberOfReadySlots; i++)
        {
            RectTransform newSlot = Instantiate(slotPrefab, readyQueue);
            newSlot.anchoredPosition = new Vector2(width / 2 - spacing * (i + 1), 0);
            slotsTrans.Add(newSlot);

            // Card Queue에도 그만큼의 빈 공간 생성
            cardInQueue.Add(null);
        }
    }

    /* 현재 카드 위치에 따라 가장 가까운 snap 슬롯을 찾고, 슬롯 위치를 반환 */
    public RectTransform GetNearestSlotPosition(Vector2 cardPos, float snapThreshold, Card card)
    {
        float minDistance = float.MaxValue;
        RectTransform slot = null;
        int slotIndex = 0;
        // 슬롯들을 검사하고 우선순위 슬롯을 판별
        for (int i = 0; i < slotsTrans.Count; i++)
        {
            float dist = Vector2.Distance(cardPos, slotsTrans[i].position);
            if (dist < minDistance && dist < snapThreshold)
            {
                minDistance = dist;
                slot = slotsTrans[i];
                slotIndex = i;
            }
        }
        while (slotIndex > 0)
        {
            if (cardInQueue[slotIndex - 1] == null)
            {
                slot = slotsTrans[slotIndex - 1];
                slotIndex--;
            }
            
            Debug.Log("slotIndex: " + slotIndex);
            cardInQueue[slotIndex] = card;
            return slot;
        }
        // 스냅 가능 슬롯이 없는 경우
        return null;
    }

    public void OnClickActionButton()
    {
        for (int i = 0; i < cardInQueue.Count; i++)
        {
            Card card = cardInQueue[i];
            if (card != null)
            {
                Debug.Log("Button pushed: " + card.GetName());
                entityDataManager.ExecuteCardEffect(card, card.GetTarget());

                // 손패에서 제거 및 버린 덱에 추가
                deckManager.AddCardTrashPile(card);

                // 현재 큐에서 제거
                cardInQueue[i] = null;

                // (필요하다면 UI도 비활성화)
                card.GetCardObject()?.SetActive(false);
            }
        }
        deckManager.GoToNextTurn();
    }
}
