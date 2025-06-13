/// <summary>
/// 적의 공격 패턴 데이터 저장 및 정럴 하는 스크립트트
/// > 패턴 데이터 저장 방식
///   └ Pattern_[이름]_enemy
/// </summary>
using UnityEngine;

public enum enemyCode
{
    Test_enemy_1 = 01
};

public class Test_EnemyBattleData : MonoBehaviour
{
    cardCord[][] Pattern_Test_enemy_1 = new cardCord[][]
    {
        new cardCord[] { },
        new cardCord[] { },
        new cardCord[] { }
    };

    public cardCord[][] GetBattleAction(enemyCode code)
    {
        return Pattern_Test_enemy_1;
    }
}
