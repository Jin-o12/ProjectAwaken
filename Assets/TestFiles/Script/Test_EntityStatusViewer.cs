using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Unity.VisualScripting;

public class Test_EntityStatusViewer : MonoBehaviour
{
    [Header("참조 스크립트")]
    public Test_CardUIController cardUIController;
    public Test_EntityDataManager statusManager;

    [Header("UI")]
    public TextMeshProUGUI nameText;
    public TextMeshProUGUI hpText;
    public Image hpBar;
    public Image Artwork;

    public Status statusData;
    private int maxHP;

    public void Start()
    {
        cardUIController = GameObject.Find("UI_CardInfo").GetComponent<Test_CardUIController>();
        statusManager = GameObject.Find("GameSystem").GetComponent<Test_EntityDataManager>();
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
