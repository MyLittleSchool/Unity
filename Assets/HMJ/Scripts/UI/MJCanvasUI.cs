using MJ;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MJCanvasUI : MonoBehaviour
{
    [Header("닫기 버튼")]
    public UnityEngine.UI.Button MapRegisterCloseButton;
    public UnityEngine.UI.Button MapContestCloseButton;

    [Header("맵 등록 패널 - 맵 등록 패널 버튼")]
    public UnityEngine.UI.Button mapRegisterButton;

    [Header("맵 등록 패널 - 맵 콘테스트 패널 버튼")]
    public UnityEngine.UI.Button mapContestButton;

    [Header("맵 인벤토리 패널 - 맵 인벤토리 패널 버튼")]
    public UnityEngine.UI.Button InventoryButton;

    [Header("맵 인벤토리 패널 - 맵 인벤토리 패널 끄기 버튼")]
    public UnityEngine.UI.Button InventoryCloseButton;

    [Header("맵 등록 패널")]
    public GameObject mapRegisterPanel;

    [Header("맵 콘테스트 패널")]
    public GameObject mapContestPanel;

    // Start is called before the first frame update
    void Start()
    {
        SceneUIManager.GetInstance().RestartSetting(MapContestCloseButton, MapRegisterCloseButton, mapRegisterButton, InventoryButton, InventoryCloseButton, mapContestButton, mapContestPanel, mapRegisterPanel);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
