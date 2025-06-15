using System.Collections.Generic;
using UnityEngine;

// 엔티티 데이터 저장 객체 기본형
[System.Serializable]
public class EntityStatus
{
    public string name = null;
    public int hp;
    public EntityCord code;
    public GameObject obj;

    /* Set */
    public void SetName(string name) { this.name = name; }  
    public void SetHP(int hp) { this.hp = hp; }
    public void SetObjcet(GameObject obj) { this.obj = obj; }
    /* Get */
    public string GetName() { return name; }
    public int GetHP() { return hp; }
    public EntityCord GetEntityCode() { return code; }
    public GameObject GetObjcet() { return obj; }

    public void appHP(int hp) { this.hp -= hp; }

    public void ApplyDamage(int dmg)
    {
        hp -= dmg;
    }

    public void AddHeal(int _amount) { hp += _amount; }

    public EntityStatus() { }
}

[System.Serializable]
public class PlayerStatus : EntityStatus
{
    public PlayerStatus(EntityStatus data)
    {
        hp = data.GetHP();
        obj = null;
    }
}

[System.Serializable]
public class EnemyStatus : EntityStatus
{
    public EnemyStatus(EntityStatus data)
    {
        name = data.GetName();
        hp = data.GetHP();
        obj = null;
    }
    public void AddHP(int hp) { this.hp += hp; }
    public void GetBattleAction(EnemyStatus enemy)
    {

    }
}

// 배열 형태 JSON을 읽기 위한 래퍼 클래스
[System.Serializable]
public class EntityInfoList
{
    public List<EntityStatus> entityList;
}

public enum EntityCord
{
    Entity_NULL = 000,      // static 변수를 위한 null 대체 형식식
    Entity_Awaker = 1,

    Entity_Dummy_1 = 101,
}

/// <summary>
/// 개체 데이터 로드 및 관리.
/// <para>
/// GetPlayerStatusById 혹은 GetEnemyStatusById 를 호출해 사용합니다.
/// </para>
/// </summary>
public static class EntityDataLoader
{
    // 개체 데이터 탐색을 위한 Dictionary형 데이터 맵 생성
    private static Dictionary<EntityCord, EntityStatus> entityDataMap;

    static EntityDataLoader()
    {
        LoadEntityStatus();
    }

    public static void LoadEntityStatus()
    {
        // Resources 폴더의 JSON 파일 불러오기
        TextAsset jsonFile = Resources.Load<TextAsset>("Data/entityData");

        if (jsonFile == null)
        {
            entityDataMap = new Dictionary<EntityCord, EntityStatus>();
            return;
        }
        string wrapped = "{\"entityList\":" + jsonFile.text + "}";
        EntityInfoList entities = JsonUtility.FromJson<EntityInfoList>(wrapped);
        entityDataMap = new Dictionary<EntityCord, EntityStatus>();
        foreach (var entity in entities.entityList)
        {
            entityDataMap[entity.GetEntityCode()] = entity;
        }
        Debug.Log($"EntityDataLoader: 개체 {entityDataMap.Count}개 로딩됨");
    }

    /* 플레이어 객체 데이터 호출 */
    public static PlayerStatus GetPlayerStatusById(EntityCord codeId)
    {
        if (entityDataMap.TryGetValue(codeId, out EntityStatus data))
        {
            PlayerStatus player = new PlayerStatus(data);
            return player;
        }
        Debug.LogWarning($"엔티티 ID '{codeId}' 를 찾을 수 없습니다.");
        return null;
    }
    /* 적 객체 데이터 호출 */
    public static EnemyStatus GetEnemyStatusById(EntityCord codeId)
    {
        if (entityDataMap.TryGetValue(codeId, out EntityStatus data))
        {
            EnemyStatus enemy = new EnemyStatus(data);
            return enemy;
        }
        Debug.LogWarning($"엔티티 ID '{codeId}' 를 찾을 수 없습니다.");
        return null;
    }

    
}

