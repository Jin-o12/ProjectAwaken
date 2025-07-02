using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StageInteractiveManager : MonoBehaviour
{
    [Header("참조 컴포넌트")]
    [SerializeField] PlayerDeckManager playerDeckManager;

    [SerializeField] GameObject entityPrefab;
    [SerializeField] RectTransform StageFieldUI;
    
    void Start()
    {
        /// 스테이지가 존재하지 않으므로 바로 배틀씬으로 넘어가게 함, 기능 추가에 따라 수정해 나갈 것 ///

        // 플레이어블 캐릭터 설정
        GameConstants.playerData = EntityDataLoader.GetPlayerStatusById(EntityCode.Entity_Awaker);

        // 플레이어 덱에 카드 설정
        playerDeckManager.AddCardInDeckList(CardCode.Card_char1_Slash);
        playerDeckManager.AddCardInDeckList(CardCode.Card_char1_QuickBash);
        playerDeckManager.AddCardInDeckList(CardCode.Card_char1_Counter);
        playerDeckManager.AddCardInDeckList(CardCode.Card_char1_Spill);        

        // 스테이지에 보일 플레이어 오브젝트
            GameObject playerObj = Instantiate(entityPrefab, StageFieldUI);
        GameConstants.playerData.SetObjcet(playerObj);

        // 적 설정
        BattleBegin(EntityCode.Entity_Dummy_1);
    }

    void Update()
    {

    }

    void BattleBegin(EntityCode code)
    {
        Debug.Log("Send Enemy Code: " + code);
        GameConstants.nowBattleEnemyCode = code;
        // 씬 바뀌면서 삭제될 것이기 때문에 백업하고 삭제
        UIStaticConfig.playerObjectInStage = GameConstants.playerData.GetObjcet();
        GameConstants.playerData.SetObjcet(null);
        SceneManager.LoadScene("BattleField");
    }
}
