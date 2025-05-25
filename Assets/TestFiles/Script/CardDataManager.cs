using System;
using System.Collections;
using System.Collections.Generic;
using Microsoft.Unity.VisualStudio.Editor;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public enum TargetType
{
    Self,
    Enemy,
    AllEnemies,
    Ally,
    None
}

public enum cardCord
{
    Card_Test_Attack_1 = 101,
    Card_Test_Defend_1 = 201

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
    TargetType target;
    cardType type;
    Action<Status, int> effect;
    cardType[] chain;
    cardCord cord;

    /* 이름, 타입, 효과, 연계, 코드 */
    public Card(string name, string explan, int value, TargetType target, cardType type, Action<Status, int> effect, cardType[] chain, cardCord cord)
    {
        this.name = name;
        this.explan = explan;
        this.value = value;
        this.target = target;
        this.type = type;
        this.effect = effect;
        this.chain = chain;
        this.cord = cord;
    }
    public Text t;  //변환용 임시변수
    public string GetName() { return name; }
    public string GetExplan() { return explan; }
    public TargetType GetTarget() { return target; }
    public int GetValue() { return value; }
    public cardCord GetCode() { return cord; }

    public void Activate(Status target, int value)
    {
        effect?.Invoke(target, value);
    }
}

public class CardEffects
{
    public static void eff_def_attack(Status target, int damage)
    {
        target.ApplyDamage(damage);
    }
    public static void eff_def_shield(Status target, int shield)
    {
        target.AddShield(shield);
    }
}


public class CardDataManager : MonoBehaviour
{
    public List<Card> cardList = new List<Card>()
    {
        // 설명에 수치 대입 후에 추가가
        /* 이름, 타입, 효과, 연계, 코드 */
        new Card("slash", "피해를 줍니다", 5, TargetType.Enemy, cardType.attack, (target, damage)=>CardEffects.eff_def_attack(target, 10),   new cardType[1]{cardType.attack},  cardCord.Card_Test_Attack_1),
        new Card("shield",  "방어력을 얻습니다", 5, TargetType.Self, cardType.defend, (target, damage)=>CardEffects.eff_def_shield(target, 10),   new cardType[1]{cardType.defend},  cardCord.Card_Test_Defend_1)
    };

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


