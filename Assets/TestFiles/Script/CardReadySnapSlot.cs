using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardReadySnapSlot : MonoBehaviour
{
    [Header("참조 스크립트")]
    [SerializeField] EntityDataManager entityDataManager;

    public int numberOfReadySlots = 5;                                      // 슬롯 크기
    public RectTransform readySlotUI;                                       // 슬롯 전체 UI
    public RectTransform slotPrefab;                                        // 개별 슬롯
    public List<Card> cardInQueue = new List<Card>() { null };                       // 슬롯 내 카드 목록
    public List<RectTransform> slots = new List<RectTransform>();           // 각 슬롯의 Transform 정보


    void Start()
    {
        GenerateSlots();
    }

    void GenerateSlots()
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
            slots.Add(newSlot);

            //Card에도 그만큼의 빈 공간 생성
            cardInQueue.Add(null);
        }
    }

    /* 현재 카드 위치에 따라 가장 가까운 snap 슬롯을 찾고, 슬롯 위치를 반환 */
    public RectTransform GetNearestSlotPosition(Vector2 cardPos, float snapThreshold)
    {
        float minDistance = float.MaxValue;
        Vector2 nearest = cardPos;
        RectTransform s = null;//위 위치로 수정
        int slotOrder = 0;
        foreach (var slot in slots)
        {
            float dist = Vector2.Distance(cardPos, slot.position);
            if (dist < minDistance && dist < snapThreshold)
            {
                minDistance = dist;
                s = slot;
            }
            slotOrder++;
        }

        while (slotOrder>0)
        {
            if (cardInQueue[slotOrder - 1] != null)
            {
                break;
            }
            else
            {
                s = slots[slotOrder - 1];
                slotOrder--;
            }
        }
        return s;
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

                // 버린 덱에 추가
                List<cardCord> discard = entityDataManager.player.discardPile;
                discard.Add(card.GetCode());

                // 현재 큐에서 제거
                cardInQueue[i] = null;

                // (필요하다면 UI도 비활성화)
                card.GetCardObject()?.SetActive(false);
            }
        }
    }
}
