using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CardViewer : MonoBehaviour
{
    [Header("카드 UI: 이미지")]
    public Image backImage;
    public Image frontImage;
    public Image artworkImage;
    public Image DamageNumImage;
    public Image chainNumImage;

    [Header("카드 UI 텍스트")]
    public TextMeshProUGUI nameText;
    public TextMeshProUGUI DamageNumText;
    public TextMeshProUGUI ChainNumText;
    public TextMeshProUGUI explanText;

    /* 게임 오브젝트 */
    private GameObject cardObject = null;
    private CardInfo cardData;
    

    public CardInfo GetCardInfo()
    {
        return cardData;
    }
    public CardInfo GetCardData()
    {
        return cardData;
    }
    public int GetCardID()
    {
        return GetCardData().GetID();
    }

    /* 카드 오브젝트의 기본 값 설정 (카드 정보, 카드 인스턴스) */
    public void SetupCardData(CardInfo data, GameObject obj)
    {
        cardData = data;
        nameText.text = data.GetName();
        DamageNumText.text = data.GetValue().ToString();
        ChainNumText.text = data.GetChain().ToString();
        explanText.text = data.GetExplanFormatted().ToString();
        artworkImage.sprite = data.GetArtwork();
        cardObject = obj;
    }

    public void SetArtwork(Sprite img)
    {
        artworkImage.sprite = img;
    }
    
    void OnDestroy()
    {
        Debug.Log("카드 오브젝트가 파괴");
    }
}
