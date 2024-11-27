using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{

    public enum EMainEftType
    {
    }
    public enum EButtonEftType
    {
        BUTTON_CLICK,
        ICON_UP
    }
    public enum EInteractEftType
    {
        MISSION_CLEAR,
        BUBBLE_UP,
        WRITE_DATA
    }

    public enum EBgmType
    {
        BGM_DATA
    }

    public static SoundManager instance;

    //audioSource
    public AudioSource eftAudio;
    public AudioSource bgmAudio;

    //effect audio clip을 여러게 담아 놓을 변수
    public AudioClip[] mainEftAudios;
    public AudioClip[] buttonEftAudios;
    public AudioClip[] InteractEftAudios;
    public AudioClip[] bgmAudios;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            //씬이 전환이 돼도 게임오브젝트를 바괴하고 싶지 않다
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    void Start()
    {
        
    }
    void Update()
    {
        
    }
    public void PlayMainEftSound(EMainEftType idx)
    {
        int audioIdx = (int)idx;
        eftAudio.PlayOneShot(mainEftAudios[audioIdx]);
    }

    public void ButtonMainEftSound(EButtonEftType idx)
    {
        int audioIdx = (int)idx;
        eftAudio.PlayOneShot(buttonEftAudios[audioIdx]);
    }

    public void InteractMainEftSound(EInteractEftType idx)
    {
        int audioIdx = (int)idx;
        eftAudio.PlayOneShot(InteractEftAudios[audioIdx]);
    }

    
    public void PlayBgmSound(EBgmType idx)
    {
        int audioIdx = (int)idx;

        bgmAudio.clip = bgmAudios[audioIdx];

        bgmAudio.Play();
    }

    public void StopBgmSound()
    {
        bgmAudio.Stop();

    }
}
