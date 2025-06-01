using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CardUIController : MonoBehaviour
{
    [Header("참조 스크립트")]
    [SerializeField] CardDataManager cardDataManager;

    [Header("카드 UI")]
    [SerializeField] public Transform handPanel;         // 카드가 나열될 패널
    [SerializeField] public Transform ReadyQueuePanel;   // 카드 대기 패널
    [SerializeField] public GameObject cardPrefab;       // 카드 프리팹 (UI용)

    private CardSnapSlot[] handSnapSlot;
    private CardSnapSlot[] readySnapSlot;
    public Card cilkedCard = null;

    void Update()
    {
    }

    public void DrawHand(List<cardCord> handList)
    {
        // 기존 카드 제거
        //foreach (Transform child in handPanel)
        //{
        //    Destroy(child.gameObject);
        //}

        // 새 카드 생성
        for (int i = 0; i < handList.Count; i++)
        {
            // 카드 데이터 가져옴 및 배치치
            cardCord code = handList[i];
            GameObject cardObj = Instantiate(cardPrefab, handPanel);
            HandCardPos(cardObj, i, handList.Count);

            CardViewer view = cardObj.GetComponent<CardViewer>();
            view.Setup(cardDataManager.GetCardDataByNum(code));
        }
    }

    void HandCardPos(GameObject cardObj, int order, int handSize)
    {
        float xPos = 0;
        float yPos = 0;
        RectTransform rt = cardObj.GetComponent<RectTransform>();

        //위치 계산
        xPos = (order - (handSize / 2)) * 50;

        rt.anchoredPosition = new Vector2(xPos, yPos);
    }

    void StockCardInQueue()
    {

    }


}
