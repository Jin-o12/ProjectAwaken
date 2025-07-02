using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Splines;
using System.Linq;
using DG.Tweening;
using UnityEngine.EventSystems;

/// <summary>
/// 한 게임 세션동안 유지되는 모든 카드들의 데이터를 저장 및 수정
/// </summary>
public class BattleCardPileUI : MonoBehaviour
{
    [Header("UI_CardInfo 속성")]
    [SerializeField] public RectTransform CardInfoUI;
    [SerializeField] public RectTransform DeckQueueUI;
    [SerializeField] public RectTransform DeckTrashUI;
    [SerializeField] public RectTransform playerReadyQueueUI;
    [SerializeField] public RectTransform enemyReadyQueueUI;
    [SerializeField] public RectTransform HandListUI;

    [Header("Splines 속성")]
    [SerializeField] private SplineContainer splineContainer;
    [SerializeField] private Transform spawnPoint;

    [Header("Prefaps")]
    [SerializeField] GameObject cardPrefab;
    [SerializeField] GameObject snapSlotPrefab;


    /* 플레이어 손패, 덱, 버린덱 */
    private int handSize = GameConstants.handSize;
    private List<CardViewer> handCardPile = new();                               // 손패 카드 리스트
    private List<CardInfo> DrawCardPile = new();                                // 손패 카드 리스트 CardInfo
    private List<CardInfo> TrashCardPile = new List<CardInfo>();
    private List<CardViewer> ReadyCardQueue = new();                            // Ready 슬롯 내 카드 목록
    private List<GameObject> ReadyslotsTrans = new List<GameObject>();          // Ready 슬롯 내 각 스냅 슬롯의 Transform 정보
    //private float snapThreshold = 0.5f;                                       // 스냅 인식 최소 범위

    /* 적 정보 */
    private cardCord[][] EnemyActionList;

    /* 카드 UI 초기 세팅 */
    public void BattleUISetting_Card()
    {
        InitDrawCardPile();
        SetupReadySnapSlot();
        //TurnBeginAnimation();
        DrawCard();
    }

    /* Ready Slot Get Function */
    public List<CardViewer> GetCardInQueue()
    {
        return ReadyCardQueue;
    }

    /* 손패의 카드를 대기 큐에 적재 */
    public void AddCardInReadyFromHand(GameObject card, int snapZonePos)
    {
        CardViewer moveCardViewer = card.GetComponent<CardViewer>();

        // 손패 내에서 동일 카드 찾아 저장하고 손패에서 제거
        CardViewer handCard = handCardPile.Find(card => card.GetCardData().GetID() == moveCardViewer.GetCardData().GetID());
        handCardPile.Remove(handCard);

        // 카드 배치 및 부모 변경
        RectTransform rtSlot = ReadyslotsTrans[snapZonePos].GetComponent<RectTransform>();
        card.transform.SetParent(playerReadyQueueUI);
        card.GetComponent<RectTransform>().anchoredPosition = ReadyslotsTrans[snapZonePos].GetComponent<RectTransform>().anchoredPosition;
        

        // 카드 정보 저장
        ReadyCardQueue[snapZonePos] = moveCardViewer;
    }

    /* 드로우 덱 초기 세팅: GameContants의 데이터를 바탕으로 받아옴 */
    private void InitDrawCardPile()
    {
        int[] shuffleArr = ShuffleCardPile(GameConstants.DeckList.Count);
        for (int i = 0; i < GameConstants.DeckList.Count; i++)
        {
            DrawCardPile.Add(GameConstants.DeckList[shuffleArr[i]]);
        }
    }

    /* 드로우 카드 없을 시 버린카드를 올리고 다시 셔플 */
    private void ReloadDrawCardPile()
    {
        int[] shuffleArr = ShuffleCardPile(GameConstants.DeckList.Count);
        for (int i = 0; i < GameConstants.DeckList.Count; i++)
        {
            DrawCardPile.Add(TrashCardPile[shuffleArr[i]]);
        }
        TrashCardPile.Clear();
    }

    /* (int)size 크기의 오름차순 배열 셔플 (덱셔플을 위한 기본 함수) */
    private int[] ShuffleCardPile(int size)
    {
        int[] shuffleArr = Enumerable.Range(0, size).ToArray();
        for (int i = 0; i < size; i++)
        {
            int random = Random.Range(0, size);
            (shuffleArr[i], shuffleArr[random]) = (shuffleArr[random], shuffleArr[i]);
        }
        return shuffleArr;
    }

    /* 덱으로부터 카드 한장 드로우 */
    private void DrawCard()
    {
        // 손패 가득 참
        if (handCardPile.Count >= GameConstants.handSize)
        {
            Debug.Log("BattleCardPileUI.DrawCard: Hand was Full");
            return;
        }
        // 모든 카드가 트레시 존에 있음 (== 모든 카드를 사용함)
        else if (DrawCardPile.Count <= 0 && TrashCardPile.Count > 0)
        {
            Debug.Log("BattleCardPileUI.DrawCard: Do not have any Cards");
            ReloadDrawCardPile();
        }
        // 카드가 없음
        else if (DrawCardPile.Count <= 0 && TrashCardPile.Count <= 0)
        {
            Debug.Log("BattleCardPileUI.DrawCard: has no card");
            return;
        }

        // 위의 예외 사항들을 처리한 뒤 한장 드로우
        foreach (CardInfo cardInfo in DrawCardPile)
        {
            //인스턴스 생성
            GameObject cardPre = Instantiate(cardPrefab, spawnPoint.position, spawnPoint.rotation);

            // 부모에 위치값 상속 및 크기 조절
            cardPre.transform.SetParent(splineContainer.transform, worldPositionStays: false);
            cardPre.GetComponent<RectTransform>().localScale *= 4;

            // 카드 인스턴스 생성 및 정보 세팅
            CardViewer cardInfoCom = cardPre.GetComponent<CardViewer>();
            cardInfoCom.SetupCardData(cardInfo, cardPre);
            handCardPile.Add(cardInfoCom);
            UpdateCardPositions();
        }
    }

    /* 손패 카드 정렬 */
    private void UpdateCardPositions()
    {
        if (handCardPile.Count == 0) return;

        float cardSpacing = 1.0f / GameConstants.handSize;   // 카드간의 거리 (스플라인 길이는 0~1 값)
        float firstCardPosition = 0.5f - (handCardPile.Count - 1) * cardSpacing / 2; //드로우 카드 갯수에 따라 첫 카드의 위치가 밀림
        Spline spline = splineContainer.Spline;
        for (int i = 0; i < handCardPile.Count; i++)
        {
            float p = firstCardPosition + i * cardSpacing;

            //카드를 정렬하고 Spline 묶음을 살짝 회전 (손패가 원형이 되도록)
            Vector3 localPos = spline.EvaluatePosition(p);    // 곡선상 특정 위치(인자값)의 실제 좌표값 반환
            Vector3 worldPosition = HandListUI.transform.TransformPoint(localPos);

            Vector3 forward = spline.EvaluateTangent(p);            // 곡선상 특정 위치(인자값)에서의 접선벡터 방향
            Vector3 up = spline.EvaluateUpVector(p);                // 곡선상 특정 위치(인자값)에서의 위쪽 방향
            Quaternion rotation = Quaternion.LookRotation(up, Vector3.Cross(up, forward).normalized);
            handCardPile[i].transform.DOMove(worldPosition, 0.25f);
            handCardPile[i].transform.DOLocalRotateQuaternion(rotation, 0.25f);
        }
    }

    /* 스냅 슬롯 생성 및 정렬 */
    private void SetupReadySnapSlot()
    {
        float queueWidth = playerReadyQueueUI.rect.width;

        int slotNum = GameConstants.readyQueueSize;

        float slotAreaWidth = queueWidth * 0.9f;                // 슬롯이 사용할 전체 가로 영역
        float slotSpacing = slotAreaWidth / slotNum;            // 슬롯 간의 간격
        float startX = -slotAreaWidth / 2 + slotSpacing / 2;    // 첫 슬롯의 X 위치

        for (int i = 0; i < slotNum; i++)
        {
            GameObject newSlot = Instantiate(snapSlotPrefab, playerReadyQueueUI);
            RectTransform newSlotRt = newSlot.GetComponent<RectTransform>();

            //float x = startX + slotSpacing * i;
            float x = startX + slotSpacing * (slotNum - 1 - i); // 플레이어는 정방향 정렬
            newSlotRt.anchoredPosition = new Vector2(x, 0);

            ReadyslotsTrans.Add(newSlot);
            ReadyCardQueue.Add(null);
        }
    }

    // /* 카드 드래그 앤 드롭시 위치 및 데이터 이동 */
    // public void CardPutOnSnapSlot(PointerEventData card, GameObject snapZone)
    // {
    //     GameObject cardObj = card.pointerDrag;          // 카드 오브젝트
    //     Vector2 screenPosition = card.position;         // 드롭시 화면상 위치

    //     RectTransform cardRt = cardObj.GetComponent<RectTransform>();
    //     RectTransform snapZoneRt = snapZone.GetComponent<RectTransform>();

    //     cardRt = snapZoneRt;

    //     if (snapZone.CompareTag("ReadySlot"))
    //     {
    //         int cardIndex = handCardPile.IndexOf(cardObj.GetComponent<CardViewer>());
    //         handCardPile.RemoveAt(cardIndex);
    //         ReadyCardQueue.Add(cardObj.GetComponent<CardViewer>()); //순서대로 적재되게 바꿀 것
            
    //     }
    //     else if (snapZone.CompareTag("HandSlot"))
    //     {
    //         int cardIndex = handCardPile.IndexOf(cardObj.GetComponent<CardViewer>());
    //         ReadyCardQueue.RemoveAt(cardIndex);
    //         handCardPile.Add(cardObj.GetComponent<CardViewer>());
    //     }
            
    //     UpdateCardPositions();
    //     //부모 바꾸는 코드 eventData.pointerDrag.transform.SetParent(transform);
    // }
}
