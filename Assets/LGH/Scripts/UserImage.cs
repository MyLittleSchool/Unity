using SW;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static HttpManager;
using UnityEngine.Networking;
using MJ;
using UnityEngine.UI;

public class UserImage : MonoBehaviour
{
    public List<Sprite> skin;
    public List<Sprite> clothes;
    public List<Sprite> face;
    public List<Sprite> hair;

    AvatarIndexData avatarIndexData;

    public Image skinSpriteRenderer;
    public Image clothesSpriteRenderer;
    public Image faceSpriteRenderer;
    public Image hairSpriteRenderer;

    void Start()
    {

    }

    void Update()
    {

    }

    public void AvatarGet(int userIdex)
    {

        HttpInfo info = new HttpInfo();
        info.url = HttpManager.GetInstance().SERVER_ADRESS + "/avatar?userId=" + userIdex;
        info.onComplete = (DownloadHandler downloadHandler) =>
        {
            avatarIndexData = JsonUtility.FromJson<AvatarIndexData>(downloadHandler.text);
            AvatarSetting(avatarIndexData.infoList[0], avatarIndexData.infoList[1], avatarIndexData.infoList[2], avatarIndexData.infoList[3]);
        };
        HttpManager.GetInstance().GetMethod(info);
    }
    void AvatarSetting(int skinId, int faceId, int hairId, int clothId)
    {
        skinSpriteRenderer.sprite = skin[skinId];
        faceSpriteRenderer.sprite = face[faceId];
        hairSpriteRenderer.sprite = hair[hairId];
        clothesSpriteRenderer.sprite = clothes[clothId];
    }


}
