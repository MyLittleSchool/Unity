using GH;
using MJ;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CaptureManager : MonoBehaviour
{
    public static CaptureManager instance;
    // Start is called before the first frame update

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);

        }
        else
        {
            Destroy(gameObject);
        }

    }

    public static CaptureManager GetInstance()
    {
        if (instance == null)
        {
            GameObject go = new GameObject();
            go.name = "CaptureManager";
            go.AddComponent<CaptureManager>();
        }
        return instance;
    }

    public RenderTexture m_CaptureRenderTexture;

    private string m_CurrentCaptureTexturePath;

    public Sprite CaptureRenderTexture()
    {
        // 렌더 텍스쳐 크기만큼 2d 텍스쳐 생성
        Texture2D texture = new Texture2D(m_CaptureRenderTexture.width, m_CaptureRenderTexture.height);

        // 해당 렌더 텍스쳐를 활성화
        RenderTexture.active = m_CaptureRenderTexture;

        texture.ReadPixels(new Rect(0, 0, m_CaptureRenderTexture.width, texture.height), 0, 0);
        texture.Apply();

        
        byte[] bytes = texture.EncodeToPNG();
        m_CurrentCaptureTexturePath = "MapImage/" + DateTime.Now.ToString(("yyyy_MM_dd_HH_mm_ss_")) + DataManager.instance.playerName + ".png";
        System.IO.File.WriteAllBytes(Application.dataPath + "/Resources/" + m_CurrentCaptureTexturePath, bytes);

        Texture2D textureData = new Texture2D(2, 2);
        textureData.LoadImage(bytes); // PNG -> Texture로 로드

        RenderTexture.active = null;

        return Sprite.Create(textureData, new Rect(0, 0, textureData.width, textureData.height), new Vector2(0.5f, 0.5f));

    }
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
    }

    public string GetCapturePath()
    {
        return m_CurrentCaptureTexturePath;
    }


}
