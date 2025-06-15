/// <summary>
/// 메인화면 UI 상호작용
/// </summary>
using UnityEngine.EventSystems;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class MainPageController : MonoBehaviour
{
    [ Header("메인화면") ]
    [SerializeField] public Button startButton;
    [SerializeField] public Button settingButton;
    [SerializeField] public Button exitButton;

    public GraphicRaycaster raycaster;
    public EventSystem eventSystem;
    private GameObject lastHovered = null;

    void Update()
    {
        // 마우스 호버 된 오브젝트 존재시에 대한 판정
        PointerEventData pointer = new PointerEventData(eventSystem);   // 마우스 동작+위치 정보 저장
        pointer.position = Input.mousePosition;
        List<RaycastResult> results = new List<RaycastResult>();        // RayCast 결과 저장
        raycaster.Raycast(pointer, results);
        if (results.Count > 0)
        {
            GameObject hover = results[0].gameObject;
            if (hover.CompareTag("Button"))
                MouseHoverOnButton(hover);
        }
    }

    private void MouseHoverOnButton(GameObject hoverObj)
    {
        /// 호버시 색 변경 코드 문제 있음, 디버깅 필요 ///
        TextMeshProUGUI tmpText = null;
        if (lastHovered != hoverObj && lastHovered != null)
        {
            tmpText = hoverObj.GetComponentInChildren<TextMeshProUGUI>();
            tmpText.fontMaterial.SetColor(ShaderUtilities.ID_FaceColor, Color.black);
        }
        tmpText = hoverObj.GetComponentInChildren<TextMeshProUGUI>();
        tmpText.fontMaterial.SetColor(ShaderUtilities.ID_FaceColor, Color.white);
        lastHovered = hoverObj;
    }

    public void ButtonGameStart()
    {
        SceneManager.LoadScene("StageField");
    }

    public void ButtonSetting()
    {
        /// 세팅 UI 실행 ///
    }

    public void ButtonExitGame()
    { 
        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
        #else
        Application.Quit();
        #endif
    }
}
