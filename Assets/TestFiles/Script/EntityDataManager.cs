using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;

public class Status
{
    protected string name = null;
    protected int hp;
    protected int stamina;

    /* Set */
    public void SetName(string name) { this.name = name; }  
    public void SetHP(int hp) { this.hp = hp; }
    public void SetStamina(int stamina) { this.stamina = stamina; }
    /* Get */
    public string GetName() { return name; }
    public int GetHP() { return hp; }
    public int GetStamina() { return stamina; }

    public void appHP(int hp) { this.hp -= hp; }
    public void appStamina(int stamina) { this.stamina -= stamina; }

    public void ApplyDamage(int dmg)
    {
        int net = Mathf.Max(0, dmg - stamina);
        stamina = Mathf.Max(0, stamina - dmg);
        hp -= net;
    }

    public void AddHeal(int _amount) { hp += _amount; }
    public void AddStamina(int _amount) { stamina += _amount; }

    public Status() { }
}

public class PlayerStatus : Status
{
    int handSize = 5;
    public List<cardCord> handList = new List<cardCord>();
    public PlayerStatus(int hp, int stamina)
    {
        this.hp = hp;
        this.stamina = stamina;
    }

    public void AddCardToHand(cardCord code)
    {
        handList.Add(code);
    }
}

public class EnemyStatus : Status
{
    public EnemyStatus(string name, int hp, int stamina)
    {
        this.name = name;
        this.hp = hp;
        this.stamina = stamina;
    }
    public void AddHP(int hp) { this.hp += hp; }
}

public class EntityDataManager : MonoBehaviour
{
    [Header("참조 스크립트")]
    [SerializeField] EntityUIController entityUIController;
    [SerializeField] CardUIController cardUIController;

    public PlayerStatus player = new PlayerStatus(20, 10);
    public EnemyStatus enemy = new EnemyStatus("creature", 10, 5);
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
        playerPS.AddCardToHand(cardCord.Card_Test_Defend_1);
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
