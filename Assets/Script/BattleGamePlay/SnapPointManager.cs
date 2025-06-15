using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* 유저 카드 대기열에 사용되는 기능들 모음 (스냅, 액션) */
public class SnapPointManager : MonoBehaviour
{
    public static List<Transform> snapPoints = new List<Transform>();

    private void Awake()
    {
        InitReadyQueue();
    }

    public void InitReadyQueue()
    {
        //스냅 구역에 대한 RectTransform, 가로세로 폭 저장
        RectTransform readyQueue = GetComponent<RectTransform>();
        float width = readyQueue.rect.width;

        // 한 스냅의 위치(크기) = 슬롯의 가로길이 / 적재 가능 카드의 갯수
        float spacing = width / (GameConstants.numberOfReadySlots + 1);

        // 스냅 위치 적재량 만큼 복사 (오브젝트 생성)
        for (int i = 0; i < GameConstants.numberOfReadySlots; i++)
        {
            RectTransform snapSlotRt = GetComponent<RectTransform>();
            RectTransform newSlot = Instantiate(snapSlotRt, GetComponent<RectTransform>());
            newSlot.anchoredPosition = new Vector2(width / 2 - spacing * (i + 1), 0);
            snapPoints.Add(newSlot);

            // Card Queue에도 그만큼의 빈 공간 생성
            snapPoints.Add(null);
        }
    }
}
