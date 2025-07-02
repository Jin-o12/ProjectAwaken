using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine.UIElements;

public class Draggable : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    private Canvas canvas;                          // 메인 캔버스
    public BattleCardPileUI battleCardPileUI;
    public float snapThreshold = 1f;                // 스냅 허용 거리
    public RectTransform lastRt;                    // 이전 위치 정보 값 (기록용)
    private Vector3 offset;

    // 오브젝트 기본 컴포넌트
    private RectTransform rectTransform;            
    private CanvasGroup canvasGroup;                
    private Transform originalParent;               // 드래그 이전 부모 (기록용)
    public bool wasDropped = false;                 // 드롭 여부


    void Start()
    {
        battleCardPileUI = GetComponentInParent<BattleCardPileUI>();
        rectTransform = GetComponent<RectTransform>();
        canvasGroup = GetComponent<CanvasGroup>();
        canvas = GetComponentInParent<Canvas>();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        originalParent = transform.parent;                      // 원래 부모 기억
        transform.SetParent(canvas.transform);                  // 드래그 중에만 최상위로 올림
        lastRt = gameObject.GetComponent<RectTransform>();      // 이전 위치 기록
        canvasGroup.blocksRaycasts = false;                     // Drop 감지를 위해 끔

        // BattleCardPileUI 에 드래그 중인 카드 손패에서 제외
    }

    public void OnDrag(PointerEventData eventData)
    {
        rectTransform.anchoredPosition += eventData.delta / canvas.scaleFactor;
        // Vector3 worldPos = Camera.main.ScreenToWorldPoint(eventData.position) + offset;
        // worldPos.z = 0f;
        // transform.position = worldPos;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        canvasGroup.blocksRaycasts = true;
        StartCoroutine(CheckDropSuccessLater());
    }

    private System.Collections.IEnumerator CheckDropSuccessLater()
    {
        yield return null; // 1프레임 대기

        if (!wasDropped)
        {
            //스냅 실패시 원래 자리로 복귀
            transform.SetParent(originalParent);
            GetComponent<RectTransform>().anchoredPosition = lastRt.anchoredPosition;
        }
        wasDropped = false;
    }
}
