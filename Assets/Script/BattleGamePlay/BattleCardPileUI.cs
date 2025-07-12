using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Splines;
using System.Linq;
using DG.Tweening;

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
    [SerializeField] private GameObject handSpline;
    [SerializeField] private GameObject PlayerReadySpline;
    [SerializeField] private SplineContainer handSplineContainer;
    [SerializeField] private SplineContainer PlayerReadySplineContainer;
    [SerializeField] private SplineContainer EnemyReadySplineContainer;
    [SerializeField] private Transform spawnPoint;

    [Header("Prefaps")]
    [SerializeField] GameObject cardPrefab;
    [SerializeField] GameObject snapSlotPrefab;


    /* 플레이어 손패, 덱, 버린덱 */
    private int handSize = GameConstants.handSize;
    private List<CardInfo> DrawCardPile = new();                                // 손패 카드 리스트 CardInfo
    private List<CardInfo> TrashCardPile = new();
    private List<CardViewer> handCardPile = new();                               // 손패 카드 리스트
    private List<CardViewer> ReadyCardQueue = new();                            // Ready 슬롯 내 카드 목록
    private CardViewer handlingCard = null;
    private List<Transform> ReadyslotsTrans = new List<Transform>();          // Ready 슬롯 내 각 스냅 슬롯의 Transform 정보

    /* 적 정보 */
    private cardCord[][] EnemyActionList;

    /* 카드 UI 초기 세팅 */
    public void BattleUISetting_Card()
    {
        InitDrawCardPile();
        UpdateReadySnapSlot();
        /////TurnBeginAnimation();///////
        DrawCard();
    }

    /* Ready Slot Get Function */
    public List<CardViewer> GetCardInQueue()
    {
        return ReadyCardQueue;
    }
    public List<CardViewer> GetCardInHand()
    {
        return handCardPile;
    }

    public void SetHandlingCard(CardViewer card)
    {
        handlingCard = card;
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
            cardPre.transform.SetParent(handSplineContainer.transform, worldPositionStays: false);
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
        Spline spline = handSplineContainer.Spline;
        for (int i = 0; i < handCardPile.Count; i++)
        {
            float p = firstCardPosition + i * cardSpacing;

            //카드 정렬
            Vector3 localPos = spline.EvaluatePosition(p);    // 곡선상 특정 위치(인자값)의 실제 좌표값 반환
            Vector3 worldPosition = HandListUI.transform.TransformPoint(localPos);

            Vector3 forward = spline.EvaluateTangent(p);            // 곡선상 특정 위치(인자값)에서의 접선벡터 방향
            Vector3 up = spline.EvaluateUpVector(p);                // 곡선상 특정 위치(인자값)에서의 위쪽 방향
            Quaternion rotation = Quaternion.LookRotation(up, Vector3.Cross(up, forward).normalized);
            handCardPile[i].transform.DOMove(worldPosition, 0.25f);
            handCardPile[i].transform.DOLocalRotateQuaternion(rotation, 0.25f);
        }
    }

    /* 스냅 슬롯 카드 정렬 */
    private void UpdateReadySnapSlot()
    {
        int slotNum = GameConstants.readyQueueSize;
        float cardSpacing = 1.0f / slotNum;                     // 카드 간격
        Spline spline = PlayerReadySplineContainer.Spline;      // 스플라인 지정

        for (int i = 0; i < ReadyCardQueue.Count; i++)
        {
            // 빈 공간에 대해서는 카드 배치 하지 않음
            if (ReadyCardQueue[i] == null)
                continue;

            float p = 1.0f - i * cardSpacing;

            //카드 정렬
            Vector3 localPos = spline.EvaluatePosition(p);    // 곡선상 특정 위치(인자값)의 실제 좌표값 반환
            Vector3 worldPosition = playerReadyQueueUI.transform.TransformPoint(localPos);

            Vector3 forward = spline.EvaluateTangent(p);            // 곡선상 특정 위치(인자값)에서의 접선벡터 방향
            Vector3 up = spline.EvaluateUpVector(p);                // 곡선상 특정 위치(인자값)에서의 위쪽 방향
            Quaternion rotation = Quaternion.LookRotation(up, Vector3.Cross(up, forward).normalized);
            ReadyCardQueue[i].transform.DOMove(worldPosition, 0.25f);
            ReadyCardQueue[i].transform.DOLocalRotateQuaternion(rotation, 0.25f);
        }


        // float queueWidth = playerReadyQueueUI.rect.width;

        // int slotNum = GameConstants.readyQueueSize;

        // float slotAreaWidth = queueWidth * 0.9f;                // 슬롯이 사용할 전체 가로 영역
        // float slotSpacing = slotAreaWidth / slotNum;            // 슬롯 간의 간격
        // float startX = -slotAreaWidth / 2 + slotSpacing / 2;    // 첫 슬롯의 X 위치

        // for (int i = 0; i < slotNum; i++)
        // {
        //     GameObject newSlot = Instantiate(snapSlotPrefab, playerReadyQueueUI);
        //     RectTransform newSlotRt = newSlot.GetComponent<RectTransform>();

        //     float x = startX + slotSpacing * (slotNum - 1 - i);
        //     newSlotRt.anchoredPosition = new Vector2(x, 0);

        //     ReadyslotsTrans.Add(newSlot);
        // }
    }

    public void PickupHandCard(GameObject cardObject)
    {
        CardInfo cardInfo = cardObject.GetComponent<CardViewer>().GetCardInfo();
        CardViewer findCard = handCardPile.Find(card => card.GetCardInfo().GetID() == cardInfo.GetID());    // 손패에서 동일 카드 찾기
        handlingCard = findCard;
        Debug.Log(findCard);
        handCardPile.Remove(findCard);
    }

    public void PutDownHandCard()
    {
        handCardPile.Add(handlingCard);

        // 카드 배치 및 부모 변경
        handlingCard.transform.SetParent(handSpline.transform);
        UpdateCardPositions();

        handlingCard = null;     // 카드 내려놓음
    }

    public void PickupReadyCard(GameObject cardObject)
    {
        CardInfo cardInfo = cardObject.GetComponent<CardViewer>().GetCardInfo();
        CardViewer findCard = ReadyCardQueue.Find(card => card.GetCardInfo().GetID() == cardInfo.GetID());    // 손패에서 동일 카드 찾기
        handlingCard = findCard;
        ReadyCardQueue.Remove(findCard);
        UpdateReadySnapSlot();
    }

    public void PutDownReadyCard()
    {
        ReadyCardQueue.Add(handlingCard);

        // 카드 배치 및 부모 변경
        handlingCard.transform.SetParent(PlayerReadySpline.transform);
        UpdateReadySnapSlot();

        handlingCard = null;     // 카드 내려놓음
    }
}
