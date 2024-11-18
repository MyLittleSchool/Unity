using MJ;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MJCanvasUI : MonoBehaviour
{
    [Header("닫기 버튼")]
    public UnityEngine.UI.Button MapRegisterCloseButton;
    public UnityEngine.UI.Button MapContestCloseButton;

    [Header("맵 등록 패널 - 맵 등록 여부 패널 버튼")]
    public UnityEngine.UI.Button MapConfirmYesButton;
    public UnityEngine.UI.Button MapConfirmNoButton;

    [Header("맵 등록 패널 - 맵 등록 패널 버튼")]
    public UnityEngine.UI.Button mapRegisterButton;

    [Header("맵 등록 패널 - 맵 콘테스트 패널 버튼")]
    public UnityEngine.UI.Button mapContestButton;

    [Header("맵 등록 성공 패널 끄기")]
    public UnityEngine.UI.Button MapRegisterSuccessCloseButton;

    [Header("맵 인벤토리 패널 - 맵 인벤토리 패널 버튼")]
    public UnityEngine.UI.Button InventoryButton;

    [Header("맵 인벤토리 패널 - 맵 인벤토리 패널 끄기 버튼")]
    public UnityEngine.UI.Button InventoryCloseButton;

    [Header("맵 인벤토리 에러 패널 - 맵 인벤토리 에러 패널 끄기 버튼")]
    public UnityEngine.UI.Button InventoryErrorCloseButton;

    [Header("사운드 버튼 - 헤드셋")]
    public UnityEngine.UI.Button HeadSetOnOffButton;

    [Header("사운드 버튼 - 마이크")]
    public UnityEngine.UI.Button MicroPhoneOnOffButton;

    [Header("맵 등록 성공 패널")]
    public GameObject mapSuccessRegisterPanel;

    [Header("맵 등록 패널")]
    public GameObject mapRegisterPanel;

    [Header("맵 콘테스트 패널")]
    public GameObject mapContestPanel;

    [Header("맵 등록 여부 패널")]
    public GameObject mapConfirmPanel;

    [Header("헤드셋X 패널")]
    public GameObject HeadSetXPanel;

    [Header("마이크X 패널")]
    public GameObject MirocoPhoneXPanel;

    [Header("맵 인벤토리 에러 패널 - 아이템 개수 부족")]
    public GameObject mapInventoryErrorPanel;

    // Start is called before the first frame update
    void Start()
    {

        SettingSoundPanel();
    }
    
    public void SettingSoundPanel()
    {
        if (!HeadSetOnOffButton || !MicroPhoneOnOffButton)
            return;

        //SoundImageSetting();

        HeadSetOnOffButton.onClick.AddListener(VoiceManager.instance.HeadSetOnOff);
        HeadSetOnOffButton.onClick.AddListener(SoundImageSetting);

        MicroPhoneOnOffButton.onClick.AddListener(VoiceManager.instance.MicrophoneOnOff);
        MicroPhoneOnOffButton.onClick.AddListener(SoundImageSetting);
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void HeadSetImageSetting()
    {
        VoiceManager.instance.SettingPlayerSpeaker();
        HeadSetXPanel.GetComponent<Image>().enabled = !VoiceManager.instance.GetHeadSetOnOff();
    }

    public void MirocoPhoneSetting()
    {
        MirocoPhoneXPanel.GetComponent<Image>().enabled = !VoiceManager.instance.GetMicrophoneOnOff();
    }

    public void SoundImageSetting()
    {
        HeadSetImageSetting();
        MirocoPhoneSetting();
    }
}

