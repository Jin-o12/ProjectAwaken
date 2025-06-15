using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StageInteractiveManager : MonoBehaviour
{
    [SerializeField] GameObject entityPrefab;
    [SerializeField] RectTransform StageFieldUI;
    
    void Start()
    {
        /// 스테이지가 존재하지 않으므로 바로 배틀씬으로 넘어가게 함, 기능 추가에 따라 수정해 나갈 것 ///

        // 플레이어블 캐릭터 설정
        GameConstants.playerData = EntityDataLoader.GetPlayerStatusById(EntityCord.Entity_Awaker);

        // 플레이어 덱에 카드 설정
        GameConstants.DeckList[0] = CardDataLoader.GetCardInfoByCode(CardCode.Card_char1_Slash);
        GameConstants.DeckList[1] = CardDataLoader.GetCardInfoByCode(CardCode.Card_char1_QuickBash);
        GameConstants.DeckList[2] = CardDataLoader.GetCardInfoByCode(CardCode.Card_char1_Counter);
        GameConstants.DeckList[3] = CardDataLoader.GetCardInfoByCode(CardCode.Card_char1_Spill);

        // 스테이지에 보일 플레이어 오브젝트
        GameObject playerObj = Instantiate(entityPrefab, StageFieldUI);
        GameConstants.playerData.SetObjcet(playerObj);

        // 적 설정
        BattleBegin(EntityCord.Entity_Dummy_1);
    }

    void Update()
    {

    }

    void BattleBegin(EntityCord cord)
    {
        Debug.Log("Send Enemy Code: " + cord);
        GameConstants.nowBattleEnemy = cord;
        // 씬 바뀌면서 삭제될 것이기 때문에 백업하고 삭제
        UIStaticConfig.playerObjectInStage = GameConstants.playerData.GetObjcet();
        GameConstants.playerData.SetObjcet(null);
        SceneManager.LoadScene("BattleField");
    }
}
