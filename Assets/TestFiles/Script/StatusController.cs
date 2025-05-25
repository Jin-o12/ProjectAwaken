using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Unity.VisualScripting;

public class StatusController : MonoBehaviour
{
    [Header("참조 스크립트")]
    public CardUIController cardUIController;
    public StatusManager statusManager;

    [Header("UI")]
    public TextMeshProUGUI nameText;
    public TextMeshProUGUI hpText;
    public Image hpBar;
    public Image getShield;
    public Image Artwork;
    
    public Status statusData;

    public void Start()
    { 
        cardUIController = GameObject.Find("UI_play").GetComponent<CardUIController>();
        statusManager = GameObject.Find("GameSystem").GetComponent<StatusManager>();
    }

    public void Setup(Status obj)
    {
        // ※참조복사 주의
        statusData = obj;

        if (statusData.GetName() == null)
            nameText.text = $"";
        else
            nameText.text = statusData.GetName();

        hpText.text = $"{statusData.GetHP()}/50";
    }

    public void OnMouseDown()
    {
        if (cardUIController.cilkedCard != null)
        {
            statusManager.ExecuteCardEffect(cardUIController.cilkedCard, statusData);
            
        }
    }
}
