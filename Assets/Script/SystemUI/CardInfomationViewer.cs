using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CardInfomationViewer : MonoBehaviour
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

    private Card cardData;

    public void Start()
    {
    }

    public void SetupCardData(CardInfo data)
    {
        nameText.text = data.GetName();
        DamageNumText.text = data.GetValue().ToString();
        ChainNumText.text = data.GetChain().ToString();
        artworkImage.sprite = data.GetArtwork();
    }

    public void SetArtwork(Sprite img)
    {
        artworkImage.sprite = img;
    }
}
