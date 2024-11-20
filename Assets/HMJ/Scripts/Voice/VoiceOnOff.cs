using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VoiceOnOff : MonoBehaviour
{
    public Button micButton;
    public Image micOffImage;
    public Image micOnImage;

    public enum VoiceState
    {
        VoiceOn,
        VoiceOff,
        VoiceState_End
    }

    VoiceState m_eCurVoiceState = VoiceState.VoiceOff;
    // Start is called before the first frame update
    void Start()
    {
        micButton.onClick.AddListener (() => ChangeVoiceState());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetVoiceState(VoiceState eVoiceState)
    {
        Debug.Log("eVoiceState: " + eVoiceState);
        switch (eVoiceState)
        {
            case VoiceState.VoiceOn:
                micOnImage.enabled = true;
                micOffImage.enabled = false;
                VoiceManager.GetInstance().MicrophoneOnOff(true);
                VoiceManager.GetInstance().HeadSetOnOff(true);
                break;
            case VoiceState.VoiceOff:
                micOffImage.enabled = true;
                micOnImage.enabled = false;
                VoiceManager.GetInstance().MicrophoneOnOff(false);
                VoiceManager.GetInstance().HeadSetOnOff(true);
                break;
        }
    }

    public void ChangeVoiceState()
    {
        Debug.Log("eVoiceState: " + m_eCurVoiceState);
        switch (m_eCurVoiceState)
        {
            case VoiceState.VoiceOn:
                m_eCurVoiceState = VoiceState.VoiceOff;
                break;
            case VoiceState.VoiceOff:
                m_eCurVoiceState = VoiceState.VoiceOn;
                break;
        }
        SetVoiceState(m_eCurVoiceState);
    }

    private void OnEnable()
    {
        m_eCurVoiceState = VoiceState.VoiceOff;
        SetVoiceState(m_eCurVoiceState);
    }
}
