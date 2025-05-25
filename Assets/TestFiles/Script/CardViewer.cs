using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CardViewer : MonoBehaviour
{
    [Header("참조 스크립트")]
    public CardUIController cardUIController;
    public CardDataManager cardDataManager;
    // 테스트용 임시 호출
    public StatusManager statusManager;

    public Image backImage;
    public Image frontImage;
    public Image artworkImage;
    public TextMeshProUGUI nameText;
    public TextMeshProUGUI effectText;
    public Transform ChanePanel;
    public Image ChaneList;

    private Card cardData;

    public void Start()
    { 
        cardUIController = GetComponentInParent<CardUIController>();
        cardDataManager = GameObject.Find("GameSystem").GetComponent<CardDataManager>();
        statusManager = GameObject.Find("GameSystem").GetComponent<StatusManager>();
    }
    public void Setup(Card data)
    {
        cardData = data;
        nameText.text = data.GetName();
        //artworkImage.sprite = data.Artwork;
    }

    // 해당 카드를 클릭 된 카드로 등록
    public void OnMouseDown()
    {
        cardUIController.cilkedCard = cardData;
    }
}
