using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBattleData : MonoBehaviour
{
    List<Card> enemyPattern1 = new List<Card> { };
    
    public List<Card> ReturnBattleAction()
    {
        return enemyPattern1;
    }
}
