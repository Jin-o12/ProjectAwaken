using Unity.Mathematics;
using UnityEngine;
/// <summary>
/// 배틀의 흐름을 관리 및 제어
/// </summary>
public class BattleManager : MonoBehaviour
{
    [Header("참조 컴포넌트")]
    [SerializeField] public BattleFieldUI battleFieldUI;
    [SerializeField] public BattleCardPileUI battleCardPileUI;

    [Header("Canvas 속성")]
    [SerializeField] public Canvas mainCanvas;
    [SerializeField] public UnityEngine.UI.Image backgroundImage;

    public static EntityCode nowEnemyCode = GameConstants.nowBattleEnemyCode;
    public PlayerStatus player = GameConstants.playerData;
    public EnemyStatus enemy = EntityDataLoader.GetEnemyStatusById(nowEnemyCode);

    /* 한 턴 혹은 전투동안 사용되는 일회성 값 */
    private int chainStack;                 // 연계 가능 스텍 수치
    private int nextTurnDraw;               // 다음 차례 드로우 예정 카드

    void Start()
    {
        battleFieldUI.BattleUISetting_Entity();
        battleCardPileUI.BattleUISetting_Card();
    }

    void Update()
    {
        
    }
}
