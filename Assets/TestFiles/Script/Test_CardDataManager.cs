/// <summary>
/// 게임 플레이시 사용되는 모든 종류의 카드를 저장
///  └ Class (Main class 제외)
///    ├ Card           // 카드 객체의 모든 정보 보유
///    └ CardEffects    // 카드 효과 메소드
///  └ Enum
///    ├ TargetType     // 카드 효과 대상 구분
///    └ cardCord       // 카드 종류에 따른 고유번호
///      └ 00X: 테스트용 혹은 디버깅용 카드, 실제 인게임에는 존재하지 않음
///      └ 10X: 1번 캐릭터가 사용하는 카드
///      └ 20X: 2번 캐릭터가 사용하는 카드
///      └ 30X: 3번 캐릭터가 사용하는 카드
///      └ 50X: 적이 사용하는 카드
/// </summary>
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum TargetType
{
    Self,
    Enemy,
    Ally,
    None
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

    public Card() { }
    /* 이름, 타입, 효과, 연계, 코드 */

    public Card(string name, string explan, int value, int chain, TargetType target, cardCord cord, Action<Status, int> effect)
    {
        this.name = name;
        this.explan = explan;
        this.value = value;
        this.chain = chain;
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

    public Card Clone()
    {   
        return new Card(this.name, this.explan, this.value, this.chain, this.target, this.cord, this.effect);
    }
}
static public class CardEffect
{
    public static void eff_def_attack(Status target, int dmg)
    {
        Debug.Log("eff_def_attack to " + target);
        target.ApplyDamage(dmg);
    }
}

public enum cardCord
{
    Card_Test_Attack_1 = 001,

    Card_char1_Slash = 101,
    Card_char1_QuickBash = 102,
    Card_char1_Counter = 103,
    Card_char1_Spill = 104
}

public class Test_CardDataManager : MonoBehaviour
{
    [SerializeField] public List<Sprite> artworkImages = new List<Sprite>();    // 카드 리스트와 같은 순번 사용용
    public List<Card> cardList = new List<Card>();

    public void Start()
    {
        cardList.Add(new Card("Attack", "피해를 줍니다", 5, 7, TargetType.Enemy, cardCord.Card_Test_Attack_1, (target, val) => CardEffect.eff_def_attack(target, val)));

        cardList.Add(new Card("Slash", " ", 3, 4, TargetType.Enemy, cardCord.Card_char1_Slash, (target, val) => CardEffect.eff_def_attack(target, val)));
        cardList.Add(new Card("QuickBash", " ", 1, 5, TargetType.Enemy, cardCord.Card_char1_QuickBash, (target, val) => CardEffect.eff_def_attack(target, val)));
        cardList.Add(new Card("Counter", " ", 5, 2, TargetType.Enemy, cardCord.Card_char1_Counter, (target, val) => CardEffect.eff_def_attack(target, val)));
        cardList.Add(new Card("Spill", " ", 0, 7, TargetType.Enemy, cardCord.Card_char1_Spill, (target, val) => CardEffect.eff_def_attack(target, val)));

        cardList.Add(new Card("Scratch", " ", 5, 7, TargetType.Enemy, cardCord.Card_char1_Spill, (target, val) => CardEffect.eff_def_attack(target, val)));
        cardList.Add(new Card("Spill", " ", 0, 7, TargetType.Enemy, cardCord.Card_char1_Spill, (target, val) => CardEffect.eff_def_attack(target, val)));
        cardList.Add(new Card("Spill", " ", 0, 7, TargetType.Enemy, cardCord.Card_char1_Spill, (target, val) => CardEffect.eff_def_attack(target, val)));

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


