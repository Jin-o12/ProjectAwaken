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
    public PlayerStatus(int hp)
    {
        this.hp = hp;
    }
}

public class EnemyStatus : Status
{
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
    }

    void Update()
    {

    }

    /* 전투 시작시 기본 셋팅 */
    public PlayerStatus BattleBegin_PlayerSetting()
    {
        if (player == null)
            Debug.LogError("EntityDataManager: player obj Does not exist");

        // 기본적인 데이터 베이스를 구성하고 그곳에서 가져오는 형식으로 바꿀 것
        if (entityUIController == null)
            Debug.LogError("EntityDataManager: entityUIController Component Does not exist");
        entityUIController.PlaceEntity(player);

        return player;
    }
    public EnemyStatus BattleBegin_EnemySetting(enemyCode code)
    {
        if (enemy == null)
            Debug.LogError("EntityDataManager: enemy obj Does not exist");

        // 기본적인 데이터 베이스를 구성하고 그곳에서 가져오는 형식으로 바꿀 것
        if (entityUIController == null)
            Debug.LogError("EntityDataManager: entityUIController Component Does not exist");
        entityUIController.PlaceEntity(enemy);

        return enemy;
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
