using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class EntityStatusViewer : MonoBehaviour
{
    [Header("개체 UI 이미지")]
    public Image characterImage;
    public Image hpBar;

    [Header("개체 UI 텍스트")]
    public TextMeshProUGUI nameText;
    public TextMeshProUGUI hpText;
    

    public EntityStatus statusData;
    private int maxHP;

    public void Start()
    {
    }

    public void Setup(EntityStatus obj)
    {
        // // ※참조복사 주의
        statusData = obj;
        
        maxHP = statusData.GetHP();

        if (statusData.GetName() == null)
            nameText.text = $"";
        else
            nameText.text = statusData.GetName();

        hpText.text = $"{statusData.GetHP()}/{maxHP}";
    }

    public void RewritingStatusUI()
    {
        // 체력
        hpText.text = $"{statusData.GetHP()}/{maxHP}";
        float fillAmountHP = (float)statusData.GetHP() / (float)maxHP;
        hpBar.fillAmount = fillAmountHP;
    }
}
