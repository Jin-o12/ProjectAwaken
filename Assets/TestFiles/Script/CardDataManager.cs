using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public enum TargetType
{
    Self,
    Enemy,
    Ally,
    None
}

public enum cardCord
{
    Card_Test_Attack_1 = 001,

    Card_char1_Slash = 101,
    Card_char1_QuickBash = 102,
    Card_char1_Counter = 103,
    Card_char1_Spill = 104
    

}
public enum cardType
{
    attack,
    defend,
    skill
}
public class Card
{
    string name;
    string explan;
    int value;
    int chain;
    Sprite artwork;
    TargetType target;
    cardCord cord;
    Action<Status, int> effect;
    GameObject cardObj;

    /* 이름, 타입, 효과, 연계, 코드 */
    public Card(string name, string explan, int value, int chain, Sprite artwork, TargetType target, cardCord cord, Action<Status, int> effect)
    {

        this.name = name;
        this.explan = explan;
        this.value = value;
        this.chain = chain;
        this.artwork = artwork;
        this.target = target;
        this.cord = cord;
        this.effect = effect;
    }
    public Text t;  //변환용 임시변수
    public void SetCardObject(GameObject cardObj) { this.cardObj = cardObj; }
    public string GetName() { return name; }
    public string GetExplan() { return explan; }
    public Sprite GetArtwork() { return artwork; }
    public TargetType GetTarget() { return target; }
    public int GetValue() { return value; }
    public int GetChain() { return chain; }
    public cardCord GetCode() { return cord; }
    public GameObject GetCardObject() { return cardObj; }

    public void Activate(Status target, int val)
    {
        effect?.Invoke(target, val);
    }
}

static public class CardEffects
{
    public static void eff_def_attack(Status target, int dmg)
    {
        Debug.Log("eff_def_attack to " + target);
        target.ApplyDamage(dmg);
    }
}


public class CardDataManager : MonoBehaviour
{
    [SerializeField] public List<Sprite> artworkImages = new List<Sprite>();
    public List<Card> cardList = new List<Card>();

    public void Awake()
    {
        cardList.Add(new Card("Attack", "피해를 줍니다", 5, 7, artworkImages[0], TargetType.Enemy, cardCord.Card_Test_Attack_1, (target, val)=>CardEffects.eff_def_attack(target, val)));
        cardList.Add(new Card("Slash", " ", 3, 4, artworkImages[1], TargetType.Self,cardCord.Card_char1_Slash, (target, val)=>CardEffects.eff_def_attack(target, val)));
        cardList.Add(new Card("QuickBash", " ", 1, 5, artworkImages[2], TargetType.Self, cardCord.Card_char1_QuickBash, (target, val)=>CardEffects.eff_def_attack(target, val)));
        cardList.Add(new Card("Counter", " ", 5, 2, artworkImages[3], TargetType.Self, cardCord.Card_char1_Counter, (target, val)=>CardEffects.eff_def_attack(target, val)));
        cardList.Add(new Card("Spill", " ", 0, 7, artworkImages[4], TargetType.Self, cardCord.Card_char1_Spill, (target, val)=>CardEffects.eff_def_attack(target, val)));
    }
    public Card GetCardDataByNum(cardCord num)
    {
        foreach (Card card in cardList)
        {
            if (card.GetCode() == num)
            {
                return card;
            }
        }
        return null;
    }
}


