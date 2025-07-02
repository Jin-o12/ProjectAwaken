using System.Collections.Generic;
using UnityEngine;
using System;
using System.Text.RegularExpressions;
using Unity.VisualScripting;

// 카드 데이터 저장 객체 기본형
[Serializable]
public class CardInfo
{
    public string name;
    public int value;
    public int chain;
    public string artworkPath;
    public string explan;
    public string target;
    public CardCode code;
    public string effectCode;

    public int ID;

    [NonSerialized]
    public Sprite artwork;
    public Action<EntityStatus, int> effect;

    public CardInfo() { }
    public CardInfo(CardInfo card)
    {
        name = card.name;
        value = card.value;
        chain = card.chain;
        artwork = card.artwork;
        explan = card.explan;
        target = card.target;
        code = card.code;
    }

    public void SetID(int id) { ID = id; }

    public string GetName() { return name; }
    public Sprite GetArtwork() { return artwork; }
    public int GetValue() { return value; }
    public int GetChain() { return chain; }
    public CardCode GetCode() { return code; }
    public string GetTarget() { return target; }
    public string GetExplan() { return explan; }
    public int GetID() { return ID; }

    // 설명 내 변수나 수식 치환
    public string GetExplanFormatted()
    {
        // {} 범위 안의 문자열에 대해 파싱을 수행
        return Regex.Replace(explan, @"\{([^}]+)\}", match =>
        {
            string expr = match.Groups[1].Value;
            int result = EvaluateExpression(expr);
            return result.ToString();
        });
    }

    private int EvaluateExpression(string expr)
    {
        expr = expr.Replace(" ", ""); // 공백 제거

        string[] parts = expr.Split('+');
        int total = 0;

        foreach (var part in parts)
        {
            if (int.TryParse(part, out int val))
            {
                total += val; // 숫자면 그대로 더함
            }
            else
            {
                // 변수명 처리
                switch (part)
                {
                    case "value":
                        total += value;
                        break;
                    case "chain":
                        total += chain;
                        break;
                    default:
                        Debug.LogWarning($"Unknown variable '{part}' in expression.");
                        break;
                }
            }
        }
        return total;
    }

    public void Activate(EntityStatus target, int val)
    {
        effect?.Invoke(target, val);
    }
}

// 배열 형태 JSON을 읽기 위한 래퍼 클래스
[Serializable]
public class CardInfoList
{
    public List<CardInfo> cardList;
}

// 고유 카드 코드 구분을 위한 Enum
public enum CardCode
{
    Card_Test_Attack_1 = 001,

    Card_char1_Slash = 101,
    Card_char1_QuickBash = 102,
    Card_char1_Counter = 103,
    Card_char1_Spill = 104
}

// 카드 효과 발동을 위한 함수 모음 클래스
public static class CardEffects
{
    public static Dictionary<string, Action<EntityStatus, int>> effectMap = new Dictionary<string, Action<EntityStatus, int>>()
    {
        { "eff_attack", eff_attack },
        { "eff_drawCard", eff_drawCard}
    };
    public static void eff_attack(EntityStatus target, int dmg)
    {
        target.ApplyDamage(dmg);
    }
    public static void eff_drawCard(EntityStatus target, int dmg)
    {

    }
}

static public class CharCardInit
{
    static public int[,] cardInit = new int[,]
    {
        {101, 102, 103, 104, 105},
        {201, 202, 203, 204, 205},
        {301, 302, 303, 304, 305}
    };
}


/// <summary>
/// 카드 데이터 로드 및 관리.
/// <para>
/// GetCardInfoByCode(CardCode) 를 호출해 사용합니다.
/// </para>
/// </summary>
public static class CardDataLoader
{
    // 카드 데이터 캐싱, CardList에 모든 카드 데이터 저장 (데이터 필요시 해당 리스트만 호출)
    public static Dictionary<CardCode, CardInfo> CardDataMap;

    static public void LoadCards()
    {
        // Resources 폴더의 JSON 파일 불러오기
        TextAsset jsonFile = Resources.Load<TextAsset>("Data/cardData");

        if (jsonFile == null)
        {
            Debug.LogError("CardDataLoader: cardData.json 파일을 찾을 수 없습니다!");
            CardDataMap = new Dictionary<CardCode, CardInfo>();
            return;
        }
        string wrapped = "{\"cardList\":" + jsonFile.text + "}";
        CardInfoList Cards = JsonUtility.FromJson<CardInfoList>(wrapped);
        CardDataMap = new Dictionary<CardCode, CardInfo>();
        foreach (var card in Cards.cardList)
        {
            card.artwork = Resources.Load<Sprite>(card.artworkPath);
            Debug.Log($"카드 생성: {card.name}, 아트워크 null?: {card.artwork == null}");
            if (card.artwork == null)
                Debug.LogWarning($"이미지 로드 실패: {card.artworkPath}");
            card.effect = CardEffects.effectMap[card.effectCode];
            CardDataMap[card.code] = card;
        }
        Debug.Log($"CardDatabase: 카드 {CardDataMap.Count}장 로딩됨");
    }

    static public CardInfo GetCardInfoByCode(CardCode code)
    {
        if (CardDataMap.TryGetValue(code, out CardInfo data))
        {
            return new CardInfo(data);
        }
        Debug.LogWarning($"엔티티 ID '{code}' 를 찾을 수 없습니다.");
        return null;
    }
}
