using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;

public class Status
{
    protected string name = null;
    protected int hp;

    /* Set */
    public void SetName(string name) { this.name = name; }  
    public void SetHP(int hp) { this.hp = hp; }
    /* Get */
    public string GetName() { return name; }
    public int GetHP() { return hp; }

    public void appHP(int hp) { this.hp -= hp; }

    public void ApplyDamage(int dmg)
    {
        hp -= dmg;
    }

    public void AddHeal(int _amount) { hp += _amount; }

    public Status() { }
}

public class PlayerStatus : Status
{
    int handSize = 5;
    /// <summary>
    /// 카드 각각의 강화 수치를 표현하기 위해 Card 클래스로 바꿀 것 (혹은 덱 리스트를 저장하는 다른방식 사용)
    /// </summary>
    public List<cardCord> handList = new List<cardCord>();
    public List<cardCord> discardPile = new List<cardCord>();

    public PlayerStatus(int hp)
    {
        this.hp = hp;
    }

    public void AddCardToHand(cardCord code)
    {
        handList.Add(code);
    }
}

public class EnemyStatus : Status
{
    List<Card> actionList;
    public EnemyStatus(string name, int hp)
    {
        this.name = name;
        this.hp = hp;
    }
    public void AddHP(int hp) { this.hp += hp; }
    public void GetBattleAction(EnemyStatus enemy)
    {
        
    }
}

public class EntityDataManager : MonoBehaviour
{
    [Header("참조 스크립트")]
    [SerializeField] EntityUIController entityUIController;
    [SerializeField] CardUIController cardUIController;

    public PlayerStatus player = new PlayerStatus(30);
    public EnemyStatus enemy = new EnemyStatus("creature", 30);
    void Start()
    {
        BattleBegin(player, enemy);
        cardUIController.DrawHand(player.handList);
    }

    void Update()
    {

    }

    /* 전투 시작시 기본 셋팅 */
    private void BattleBegin(Status player, Status enemy)
    {
        if (player == null)
            Debug.LogError("EntityDataManager: player obj Does not exist (error: E0002)");
        if (player == null)
            Debug.LogError("EntityDataManager: enemy obj Does not exist (error: E0003)");

        // 플레이어와 적에 대해 카드처럼 기본적인 데이터 베이스를 구성하고 그곳에서 가져오는 형식으로 바꿀 것
        if (entityUIController == null)
            Debug.LogError("EntityDataManager: entityUIController Component Does not exist (error: E0004)");
        entityUIController.PlaceEntity(player);
        entityUIController.PlaceEntity(enemy);

        PlayerStatus playerPS = (PlayerStatus)player;
        
        // 손패 데이터를 받아오고 그만큼 카드 생성으로 바꿀 것
        playerPS.AddCardToHand(cardCord.Card_Test_Attack_1);
        playerPS.AddCardToHand(cardCord.Card_Test_Attack_1);
    }

    public void ExecuteCardEffect(Card card, TargetType target)
    {
        switch (target)
        {
            case TargetType.Enemy:
                card?.Activate(enemy, card.GetValue());
                break;
            case TargetType.Self:
                card?.Activate(player, card.GetValue());
                break;
            default:
                break;
        }

        cardUIController.connectable += card.GetChain() - card.GetValue();
    }
}
