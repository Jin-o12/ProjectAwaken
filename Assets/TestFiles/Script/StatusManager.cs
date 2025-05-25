using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;

public class Status
{
    protected string name = null;
    protected int hp;
    protected int shield;

    /* Set */
    public void SetName(string name) { this.name = name; }  
    public void SetHP(int hp) { this.hp = hp; }
    public void SetShield(int shield) { this.shield = shield; }
    /* Get */
    public string GetName() { return name; }
    public int GetHP() { return hp; }
    public int GetShield() { return shield; }

    public void appHP(int hp) { this.hp -= hp; }
    public void appShield(int shield) { this.shield -= shield; }

    public void ApplyDamage(int dmg)
    {
        int net = Mathf.Max(0, dmg - shield);
        shield = Mathf.Max(0, shield - dmg);
        hp -= net;
    }

    public void ApplyHeal(int amount)
    {
        hp += amount;
    }

    public void AddShield(int amount)
    {
        shield += amount;
    }
    

    public Status() { }
}

public class PlayerStatus : Status
{
    int handSize = 5;
    public List<cardCord> handList = new List<cardCord>();
    public PlayerStatus(int hp)
    {
        this.hp = hp;
        this.shield = 0;
    }

    public void AddCardToHand(cardCord code)
    {
        handList.Add(code);
    }
}

public class EnemyStatus : Status
{
    public EnemyStatus(string name, int hp, int shield)
    {
        this.name = name;
        this.hp = hp;
        this.shield = shield;
    }
    public void AddHP(int hp) { this.hp += hp; }
}

public class StatusManager : MonoBehaviour
{
    [SerializeField] CardUIController cardUIController;
    [SerializeField] GameObject entityPrefab;                 // 개체 프리팹
    [SerializeField] public Transform fieldPanel;            // 필드

    public PlayerStatus player = new PlayerStatus(50);
    public EnemyStatus enemy = new EnemyStatus("creature", 50, 0);
    void Start()
    {
        player.AddCardToHand(cardCord.Card_Test_Attack_1);
        player.AddCardToHand(cardCord.Card_Test_Defend_1);

        // 현재는 필드의 오브젝트를 일일이 생성 및 배치했지만, 이후 메소드로 기능을 분리할 것것
        GameObject playerObj = Instantiate(entityPrefab, fieldPanel);
        RectTransform pRt = playerObj.GetComponent<RectTransform>();
        pRt.anchoredPosition = new Vector2(300.0f, 340.0f);;
        StatusController pview = playerObj.GetComponent<StatusController>();
        pview.Setup(player);

        GameObject enemyObj = Instantiate(entityPrefab, fieldPanel);
        RectTransform eRt = enemyObj.GetComponent<RectTransform>();
        eRt.anchoredPosition = new Vector2(900.0f, 340.0f);;
        StatusController eview = enemyObj.GetComponent<StatusController>();
        eview.Setup(enemy);

        cardUIController.DrawHand(player.handList);
    }

    void Update()
    {

    }

    public void ExecuteCardEffect(Card card, Status target)
    {
        //임시 이펙트 확인용 코드
        switch (card.GetTarget())
        {
            case TargetType.Enemy:
                Debug.Log("Card Activated: " + card.GetName() + " to " + target.GetName());
                card?.Activate(target, card.GetValue());
                break;
            case TargetType.Self:
                break;
            default:
                break;
        }
    }
}
