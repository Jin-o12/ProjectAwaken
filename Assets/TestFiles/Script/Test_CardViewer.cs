using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Test_CardViewer : MonoBehaviour
{
    [Header("참조 스크립트")]
    public Test_CardUIController cardUIController;
    public Test_CardDataManager cardDataManager;

    public Image backImage;
    public Image frontImage;
    public Image artworkImage;
    public TextMeshProUGUI nameText;
    public TextMeshProUGUI effectText;
    public TextMeshProUGUI DamageNumText;
    public TextMeshProUGUI ChainNumText;

    private bool isDragging = false;
    private Vector3 offset;
    private Vector3 lastPos;        // 드래그 되기 직전의 위치치
    private float snapRange = 5.0f;

    private Card cardData;

    public void Start()
    {
        // 필수 컴포넌트 불러오기
        cardUIController = GetComponentInParent<Test_CardUIController>();
        cardDataManager = GameObject.Find("GameSystem").GetComponent<Test_CardDataManager>();
        cardUIController = GetComponentInParent<Test_CardUIController>();
    }

    public void SetupCardData(Card data)
    {
        cardData = data.Clone();
        Debug.Log($"[SetupCardData] cardData name: {cardData?.GetName()}");

        nameText.text = data.GetName();
        effectText.text = data.GetExplan();
        DamageNumText.text = data.GetValue().ToString();
        ChainNumText.text = data.GetChain().ToString();
        Debug.Log("cardDataManager: " + cardDataManager);
        int cardIndex = cardDataManager.cardList.IndexOf(data);
        artworkImage.sprite = cardDataManager.artworkImages[cardIndex];
    }

    public void SetArtwork(Sprite img)
    {
        artworkImage.sprite = img;
    }

    void Update()
    {
        // 카드 클릭시 드래그
        if (isDragging)
        {
            Vector3 mouseScreenPos = Input.mousePosition;
            mouseScreenPos.z = 0.0f;
            Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(mouseScreenPos);

            transform.position = mouseWorldPos + offset;
        }
    }

    void OnMouseDown()
    {
        isDragging = true;
        lastPos = GetComponent<RectTransform>().anchoredPosition;   //클릭 직전 카드 위치 저장
        cardUIController.cilkedCard = cardData;                     // 클릭 된 카드 등록
        
        // 마우스 위치에 따른 드래그
        Vector3 mouseScreenPos = Input.mousePosition;
        mouseScreenPos.z = 0.0f;
        Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(mouseScreenPos);
        offset = transform.position - mouseWorldPos;

        //스냅 위치 보여주기 혹은 자동 트래킹 등
    }

    void OnMouseUp()
    {
        isDragging = false;

        RectTransform rectTransform = GetComponent<RectTransform>();
        // 1. 마우스 위치를 월드 좌표로 변환
        Vector3 mouseScreenPos = Input.mousePosition;
        mouseScreenPos.z = 0f; // 또는 카메라에서의 거리값 (UI라면 0이면 충분)

        Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(mouseScreenPos);
        mouseWorldPos.z = 0; // UI에서는 z=0 고정

        // 2. 스냅 위치 계산
        Debug.Log("cardData: "+ cardData);
        RectTransform slot = cardUIController.GetNearestSlotPosition(mouseWorldPos, snapRange, cardData);
        if (slot == null)   //슬롯과 카드의 거리가 너무 멀 때, 슬롯이 비는(null판정) 경우가 생겨 중간에 예외처리함(코드 더 깔끔하게 손볼 것)//
        {
            rectTransform.anchoredPosition = lastPos;
            return;
        }
        Vector2 nearest = slot.position;

        // 3. 스냅 여부 판단
        if (nearest != (Vector2)mouseWorldPos)
        {
            rectTransform.position = nearest; // 스냅 성공
        }
        else
        {
            rectTransform.anchoredPosition = lastPos; // 스냅 실패 → 원위치
        }
        return;
    }
}
