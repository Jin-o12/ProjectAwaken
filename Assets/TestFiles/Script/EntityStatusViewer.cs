using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Unity.VisualScripting;

public class StatusViewer : MonoBehaviour
{
    [Header("참조 스크립트")]
    public CardUIController cardUIController;
    public EntityDataManager statusManager;

    [Header("UI")]
    public TextMeshProUGUI nameText;
    public TextMeshProUGUI hpText;
    public TextMeshProUGUI staminaText;
    public Image hpBar;
    public Image staminaBar;
    public Image Artwork;

    public Status statusData;
    private int maxHP;
    private int maxStamina;

    public void Start()
    {
        cardUIController = GameObject.Find("UI_CardInfo").GetComponent<CardUIController>();
        statusManager = GameObject.Find("GameSystem").GetComponent<EntityDataManager>();
    }

    public void Setup(Status obj)
    {
        // ※참조복사 주의
        statusData = obj;
        maxHP = statusData.GetHP();
        maxStamina = statusData.GetStamina();

        if (statusData.GetName() == null)
            nameText.text = $"";
        else
            nameText.text = statusData.GetName();

        hpText.text = $"{statusData.GetHP()}/{maxHP}";
        staminaText.text = $"{statusData.GetStamina()}/{maxStamina}";
    }

    public void OnMouseDown()
    {
        if (cardUIController.cilkedCard != null)
        {
            statusManager.ExecuteCardEffect(cardUIController.cilkedCard, statusData);
        }
    }

    void Update()
    {
        RewritingStatusUI();
    }

    public void RewritingStatusUI()
    {
        // 체력
        hpText.text = $"{statusData.GetHP()}/{maxHP}";
        float fillAmountHP = (float)statusData.GetHP() / (float)maxHP;
        hpBar.fillAmount = fillAmountHP;
        // 기력
        staminaText.text = $"{statusData.GetStamina()}/{maxStamina}";
        float fillAmountStamina = (float)statusData.GetStamina() / (float)maxStamina;
        staminaBar.fillAmount = fillAmountStamina;
    }
}
