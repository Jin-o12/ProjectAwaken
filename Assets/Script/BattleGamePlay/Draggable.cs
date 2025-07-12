using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Collections.Generic;

public class Draggable : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public BattleCardPileUI battleCardPileUI;
    public float snapThreshold = 1f;                // 스냅 허용 거리    
    private Vector3 offset;

    // 오브젝트 기본 컴포넌트
    private RectTransform rectTransform;

    // 드래그 구현              
    private GameObject originalSplineParent;         // 드래그 이전 스플라인 부모 (기록용)
    private GameObject originalSnapZoneParent;       // 드래그 이전 스냅존 (기록용)
    public bool wasDropped = false;                 // 드롭 여부
    public RectTransform lastRt;                    // 이전 위치 정보 값 (기록용)
    public Canvas canvas;                           // 메인 캔버스
    public CanvasGroup canvasGroup;                 // 캔버스 그룹

    public GraphicRaycaster graphicRaycaster;
    public EventSystem eventSystem;


    void Start()
    {
        battleCardPileUI = GetComponentInParent<BattleCardPileUI>();
        rectTransform = GetComponent<RectTransform>();
        canvasGroup = GetComponent<CanvasGroup>();
        canvas = GetComponentInParent<Canvas>();
        eventSystem = GetComponentInParent<EventSystem>();
        graphicRaycaster = FindObjectOfType<GraphicRaycaster>();
    }

    /* 특정 태그의 부모 오브젝트를 찾아 반환 */
    private GameObject FindParentWithTag(string tag)
    {
        Transform current = transform.parent;
        while (current != null)
        {
            Debug.Log("CHECK: " + current.name + " / tag: " + current.tag);
            if (current.CompareTag(tag))
                return current.gameObject;

            current = current.parent;
        }

        return null;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        // 원래 부모 기억
        originalSplineParent = FindParentWithTag("CardSpline");         
        originalSnapZoneParent = FindParentWithTag("SnapZone");
        Debug.Log(originalSplineParent == null);

        if (originalSnapZoneParent.name == "Ready_SnapZoneBox")
            battleCardPileUI.PickupHandCard(eventData.pointerDrag);
        else if (originalSnapZoneParent.name == "Hand_SnapZoneBox")
            battleCardPileUI.PickupReadyCard(eventData.pointerDrag);

        transform.SetParent(canvas.transform);                  // 드래그 중에만 최상위로 올림
        canvasGroup.blocksRaycasts = false;                     // Drop 감지를 위해 끔
        lastRt = gameObject.GetComponent<RectTransform>();      // 이전 위치 기록
        battleCardPileUI.SetHandlingCard(gameObject.GetComponent<CardViewer>());
    }

    public void OnDrag(PointerEventData eventData)
    {
        rectTransform.anchoredPosition += eventData.delta / canvas.scaleFactor;
        // Vector3 worldPos = Camera.main.ScreenToWorldPoint(eventData.position) + offset;
        // worldPos.z = 50f;
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

        GameObject underMouse = GetUIUnderMouse();
        if (underMouse != null)
        {
            underMouse.GetComponent<SnapZone>().CardDropped(gameObject);
        }
        else
        {
            Debug.Log("스냅 실패");
            transform.SetParent(originalSplineParent.transform);
            originalSnapZoneParent.GetComponent<SnapZone>().CardDropped(gameObject);
        }
        wasDropped = false;
    }
    
    public GameObject GetUIUnderMouse()
    {
        PointerEventData pointerData = new PointerEventData(eventSystem);
        pointerData.position = Input.mousePosition;

        List<RaycastResult> results = new List<RaycastResult>();
        graphicRaycaster.Raycast(pointerData, results);

        foreach (RaycastResult result in results)
        {
            if (result.gameObject.CompareTag("SnapZone"))
            {
                return result.gameObject;
            }
        }
        return null;
    }
}
