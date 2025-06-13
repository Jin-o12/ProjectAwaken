using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StageInteractiveManager : MonoBehaviour
{
    void Start()
    {
        /// 스테이지가 존재하지 않으므로 바로 배틀씬으로 넘어가게 함, 스테이지 기능 추가시 변경 ///
        GameConstants.playerData = EntityDataLoader.GetPlayerStatusById(EntityCord.Entity_Awaker);
        BattleBegin(EntityCord.Entity_Dummy_1);
    }

    void Update()
    {

    }

    void BattleBegin(EntityCord cord)
    {
        GameConstants.nowBattleEnemy = cord;
        SceneManager.LoadScene("BattleField");
    }
}
