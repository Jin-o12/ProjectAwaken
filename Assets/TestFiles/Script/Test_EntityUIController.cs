using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test_EntityUIController : MonoBehaviour
{
    [Header("참조 스크립트")]
    [SerializeField] Test_EntityDataManager entityDataManager;

    [Header("카드 UI")]
    [SerializeField] public Transform fieldPanel;
    [SerializeField] public GameObject entityPrefab;

    /* Entity 위치에 대한 고정 상수 값 (초기 배치) */
    private const float PLAYER_X_POS = 200.0f;
    private const float PLAYER_Y_POS = 500.0f;
    private const float ENEMY_X_POS = 1080.0f;
    private const float ENEMY_Y_POS = 500.0f;

    public void PlaceEntity(Status entity)
    {
        // (프리팹, 판넬UI)를 바탕으로 오브젝트 생성 및 위치 지정정
        GameObject obj = Instantiate(entityPrefab, fieldPanel);
        RectTransform Rt = obj.GetComponent<RectTransform>();
        
        // Player와 Enemy일 경우에 따라 좌우 배치 구분
        if (entity is tPlayerStatus)
        {
            Rt.anchoredPosition = new Vector2(PLAYER_X_POS, PLAYER_Y_POS);
        }
        else if (entity is tEnemyStatus)
        {
            Rt.anchoredPosition = new Vector2(ENEMY_X_POS, ENEMY_Y_POS); ;
        }
        else
        {
            Debug.LogError("EntityUIController: worng type Entity create (error: E0001)");
        }

        // 기본 설정을 위한 컴포넌트 생성 및 Setup
        Test_EntityStatusViewer view = obj.GetComponent<Test_EntityStatusViewer>();
        view.Setup(entity);
    }


    void Start()
    {
        
    }

    void Update()
    {
        
    }
}
