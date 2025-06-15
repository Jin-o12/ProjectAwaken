using System.Collections.Generic;
using DG.Tweening;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Splines;
using System.Linq;

public class BattleFieldUI : MonoBehaviour
{
    [Header("컴포넌트")]

    [Header("Canvas 속성")]
    [SerializeField] public Canvas mainCanvas;
    [SerializeField] public UnityEngine.UI.Image backgroundImage;

    [Header("UI_CardInfo 속성")]
    [SerializeField] public RectTransform CardInfoUI;
    [SerializeField] public RectTransform DeckQueueUI;
    [SerializeField] public RectTransform DeckTrashUI;
    [SerializeField] public RectTransform playerReadyQueueUI;
    [SerializeField] public RectTransform enemyReadyQueueUI;
    [SerializeField] public RectTransform HandListUI;

    [Header("UI_EntityField 속성")]
    [SerializeField] public RectTransform EntityFieldUI;

    [Header("Prefaps")]
    [SerializeField] GameObject entityPrefab;
    [SerializeField] GameObject cardPrefab;
    [SerializeField] GameObject snapSlotPrefab;

    [SerializeField] private SplineContainer splineContainer;
    [SerializeField] private Transform spawnPoint;
    private List<GameObject> handCardObj = new();      // 손패 카드 리스트
    private List<CardInfo> DrawCardPile = new();       // 드로우 카드 리스트
    private List<CardInfo> TrashCardPile = new();      // 버린 카드 리스트

    private PlayerStatus player;
    private EnemyStatus enemy;

    private GameObject playerObj;
    private GameObject enemyObj;
    private List<Card> cardInQueue = new List<Card>() { null };              // Ready 슬롯 내 카드 목록
    private List<RectTransform> slotsTrans = new List<RectTransform>();      // Ready 슬롯 내 각 스냅 슬롯의 Transform 정보

    /* Entity 위치에 대한 고정 상수 값 (초기 배치) */
    private const float PLAYER_X_POS = 200.0f;
    private const float PLAYER_Y_POS = 500.0f;
    private const float ENEMY_X_POS = 1080.0f;
    private const float ENEMY_Y_POS = 500.0f;

    void Start()
    {
        BattleUISetting_Entity();
        BattleUISetting_Card();
    }

    private void Update()
    { 
        
    }

    /* (프리팹, 판넬UI)를 바탕으로 오브젝트 생성 및 위치 지정 */
    private void BattleUISetting_Entity()
    {
        // 플레이어
        playerObj = Instantiate(entityPrefab, EntityFieldUI);
        RectTransform playerRt = playerObj.GetComponent<RectTransform>();
        playerRt.anchoredPosition = new Vector2(PLAYER_X_POS, PLAYER_Y_POS);

        // 상대
        enemy = EntityDataLoader.GetEnemyStatusById(GameConstants.nowBattleEnemy);
        if (GameConstants.nowBattleEnemy == 0)
            Debug.Log("Battle Enemy Code is null: " + GameConstants.nowBattleEnemy);
        enemyObj = Instantiate(entityPrefab, EntityFieldUI);
        RectTransform enemyRt = enemyObj.GetComponent<RectTransform>();
        enemyRt.anchoredPosition = new Vector2(ENEMY_X_POS, ENEMY_Y_POS);
    }

    public void BattleUISetting_Card()
    {
        ShuffleDrawCardPile();
        DrawCard();
    }

    private void UpdateCardPositions()
    {
        if (handCardObj.Count == 0) return;

        float cardSpacing = 1.0f / GameConstants.handSize;   // 카드간의 거리 (스플라인 길이는 0~1 값)
        float firstCardPosition = 0.5f - (handCardObj.Count - 1) * cardSpacing / 2; //드로우 카드 갯수에 따라 첫 카드의 위치가 밀림
        Spline spline = splineContainer.Spline;
        for (int i = 0; i < handCardObj.Count; i++)
        {
            float p = firstCardPosition + i * cardSpacing;

            //카드를 정렬하고 Spline 묶음을 살짝 회전 (손패가 원형이 되도록)
            Vector3 splinePosition = spline.EvaluatePosition(p);
            Vector3 forward = spline.EvaluateTangent(p);
            Vector3 up = spline.EvaluateUpVector(p);
            Quaternion rotation = Quaternion.LookRotation(up, Vector3.Cross(up, forward).normalized);
            handCardObj[i].transform.DOMove(splinePosition, 0.25f);
            handCardObj[i].transform.DOLocalRotateQuaternion(rotation, 0.25f);
        }
    }

    /* 드로우 덱이 비었을 때 처음 한번 셔플 */
    private void ShuffleDrawCardPile()
    {
        int shuffleCardNum;
        shuffleCardNum = GameConstants.DeckList.Count;

        int[] order = Enumerable.Range(0, shuffleCardNum).ToArray();
        for (int i = 0; i < shuffleCardNum; i++)
        {
            int random = Random.Range(0, shuffleCardNum);
            (order[i], order[random]) = (order[random], order[i]);
        }
    
        for (int i = 0; i < shuffleCardNum; i++)
        {
            DrawCardPile.Add(GameConstants.DeckList[order[i]]);
        }
    }

    private void DrawCard()
    {
        if (handCardObj.Count >= GameConstants.handSize) return;
        else if (DrawCardPile.Count <= 0) return;

        foreach (CardInfo cardInfo in DrawCardPile)
        {
            GameObject cardPre = Instantiate(cardPrefab, spawnPoint.position, spawnPoint.rotation);
            cardPre.transform.SetParent(splineContainer.transform, worldPositionStays: false);
            CardInfomationViewer cardInfoCom = cardPre.GetComponent<CardInfomationViewer>();
            cardInfoCom.SetupCardData(cardInfo);
            handCardObj.Add(cardPre);
            UpdateCardPositions();
        }
    }
    
    /* 현재 카드 위치에 따라 가장 가까운 snap 슬롯을 찾고, 슬롯 위치를 반환 */
    public RectTransform GetNearestSlotPosition(Vector2 cardPos, float snapThreshold, Card card)
    {
        float minDistance = float.MaxValue;
        RectTransform slot = null;
        int slotIndex = -1;

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

        // 스냅 가능한 슬롯이 없으면 리턴
        if (slotIndex == -1)
            return null;

        // 선택된 슬롯 또는 앞쪽 슬롯에 카드가 없다면 가장 앞쪽 빈 슬롯으로 이동
        while (slotIndex > 0 && cardInQueue[slotIndex - 1] == null)
        {
            slot = slotsTrans[slotIndex - 1];
            slotIndex--;
        }

        // 최종 슬롯이 이미 차 있으면 스냅 실패
        if (cardInQueue[slotIndex] != null)
            return null;

        cardInQueue[slotIndex] = card;
        Debug.Log(cardInQueue[slotIndex].GetName());
        return slot;
    }
}
