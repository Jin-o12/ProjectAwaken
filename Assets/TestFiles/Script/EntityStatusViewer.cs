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
    public Image hpBar;
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

        if (statusData.GetName() == null)
            nameText.text = $"";
        else
            nameText.text = statusData.GetName();

        hpText.text = $"{statusData.GetHP()}/{maxHP}";
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
    }
}
