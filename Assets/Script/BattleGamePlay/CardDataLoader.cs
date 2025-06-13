using System.Collections.Generic;
using UnityEngine;

// 카드 데이터 저장 객체 기본형
[System.Serializable]
public class CardInfo
{
    public string name;
    public int value;
    public int chain;
    public int artworkIndex;
    public string target;
    public int cord;
}

// 배열 형태 JSON을 읽기 위한 래퍼 클래스
[System.Serializable]
public class CardInfoList
{
    public List<CardInfo> cards;
}

// 고유 카드 코드 구분을 위한 Enum
public enum CardCord
{
    Card_Test_Attack_1 = 001,

    Card_char1_Slash = 101,
    Card_char1_QuickBash = 102,
    Card_char1_Counter = 103,
    Card_char1_Spill = 104
}

// 카드 효과 발동을 위한 함수 모음 클래스
static public class CardEffects
{
    public static void eff_def_attack(EntityStatus target, int dmg)
    {
        target.ApplyDamage(dmg);
    }
}

/// <summary>
/// 카드 데이터 로드 및 관리.
/// <para>
/// static List CardList 를 호출해 사용합니다.
/// </para>
/// </summary>
public static class CardDataLoader
{
    // 카드 데이터 캐싱, CardList에 모든 카드 데이터 저장 (데이터 필요시 해당 리스트만 호출)
    public static List<CardInfo> CardList;

    static CardDataLoader()
    {
        if (CardList == null)
            LoadCards();
    }

    static void LoadCards()
    {
        // Resources 폴더의 JSON 파일 불러오기
        TextAsset jsonFile = Resources.Load<TextAsset>("Data/cardData");

        if (jsonFile == null)
        {
            Debug.LogError("CardDataLoader: cardData.json 파일을 찾을 수 없습니다!");
            CardList = new List<CardInfo>();
            return;
        }
        string wrapped = "{\"cardData\":" + jsonFile.text + "}";
        CardInfoList Cards = JsonUtility.FromJson<CardInfoList>(wrapped);
        CardList = Cards.cards;
        Debug.Log($"CardDatabase: 카드 {CardList.Count}장 로딩됨");
    }
}
