using GH;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.Rendering;
using static HttpManager;
using static MapRegisterDataUI;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ToolTip;
using Photon.Pun;
using SW;
using Photon.Realtime;
using static UnityEngine.InputManagerEntry;

namespace MJ
{
    public class DecorationEnum : MonoBehaviour
    {
        public enum DECORATION_DATA
        {
            SKIN,
            FACE,
            HAIR,
            CLOTH,
            DECORATION_DATA_END
        }
    }

    [Serializable]
    public struct AvatarIndexData
    {
        public int userId;
        public List<int> infoList;
    }

    public class PlayerAnimation : MonoBehaviourPun
    {
        public static PlayerAnimation instance;

        public DecoPlayerUI decoPlayUI;

        private void Awake()
        {
            if (instance == null)
            {
                instance = this;

            }

        }

        public static PlayerAnimation GetInstance()
        {
            if (instance == null)
            {
                GameObject go = new GameObject();
                go.name = "PlayerAnimation";
                go.AddComponent<PlayerAnimation>();
            }
            return instance;
        }

        AvatarIndexData avatarIndexData;

        private void Start()
        {
        }

        private void Update()
        {

        }
        private int[] animatorIndex = new int[(int)DecorationEnum.DECORATION_DATA.DECORATION_DATA_END];
        private int[] animMaxIndexData = { 3, 5, 4, 4 };

        public void SetDecorationAnimData(DecorationEnum.DECORATION_DATA decorationData, int idx)
        {
            if (animMaxIndexData[(int)decorationData] <= idx)
                return;
            animatorIndex[(int)decorationData] = idx;
            //skin, cloth, face, hair

            decoPlayUI.SetAnimationSpriteIndex(animatorIndex[0], animatorIndex[3], animatorIndex[1], animatorIndex[2]);
            for (int i = 0; i < 4; i++)
            {
                if (animMaxIndexData[i] <= animatorIndex[i])
                    return;
            }
        }

        public void InitPlayerAnimation()
        {
            for (int i = 0; i < (int)DecorationEnum.DECORATION_DATA.DECORATION_DATA_END; i++)
            {
                animatorIndex[i] = UnityEngine.Random.Range(0, animMaxIndexData[i]);
            }

        }

        private void OnEnable()
        {
        }
        // 아바타 데이터 전송
        public void PostAvatarData()
        {
            InitPlayerAnimation();

            AvatarIndexData avatarInfoList = new AvatarIndexData();
            avatarInfoList.userId = DataManager.instance.mapId;
            avatarInfoList.infoList = animatorIndex.ToList();

            HttpInfo info = new HttpInfo();
            info.url = HttpManager.GetInstance().SERVER_ADRESS + "/avatar";
            info.body = JsonUtility.ToJson(avatarInfoList);
            info.contentType = "application/json";
            info.onComplete = (DownloadHandler downloadHandler) =>
            {
                decoPlayUI.SetAnimationSpriteIndex(animatorIndex[0], animatorIndex[3], animatorIndex[1], animatorIndex[2]);
                AvatarEdit(animatorIndex[0], animatorIndex[3], animatorIndex[1], animatorIndex[2]);
                Debug.Log("아바타 데이터-------------------");
                Debug.Log(downloadHandler.text);
                Debug.Log("--------------------------------");
            };

            StartCoroutine(HttpManager.GetInstance().Post(info));
        }

        public void PatchAvatarData()
        {
            AvatarIndexData avatarInfoList = new AvatarIndexData();
            avatarInfoList.userId = DataManager.instance.mapId;
            avatarInfoList.infoList = animatorIndex.ToList();

            HttpInfo info = new HttpInfo();
            info.url = HttpManager.GetInstance().SERVER_ADRESS + "/avatar";
            info.body = JsonUtility.ToJson(avatarInfoList);
            info.contentType = "application/json";
            info.onComplete = (DownloadHandler downloadHandler) =>
            {
                decoPlayUI.SetAnimationSpriteIndex(animatorIndex[0], animatorIndex[3], animatorIndex[1], animatorIndex[2]);
                AvatarEdit(animatorIndex[0], animatorIndex[3], animatorIndex[1], animatorIndex[2]);
                Debug.Log("Fetch 성공: " + downloadHandler.text);
            };
            StartCoroutine(HttpManager.GetInstance().Patch(info));
        }

        // 아바타 데이터 받기
        public void GetAvatarData()
        {
            HttpInfo info = new HttpInfo();
            info.url = HttpManager.GetInstance().SERVER_ADRESS + "/avatar?userId=" + DataManager.instance.mapId;
            info.onComplete = (DownloadHandler downloadHandler) =>
            {
                avatarIndexData = JsonUtility.FromJson<AvatarIndexData>(downloadHandler.text);
                animatorIndex = avatarIndexData.infoList.ToArray();
                decoPlayUI.SetAnimationSpriteIndex(animatorIndex[0], animatorIndex[3], animatorIndex[1], animatorIndex[2]);
                AvatarEdit(animatorIndex[0], animatorIndex[3], animatorIndex[1], animatorIndex[2]);
                Debug.Log("--------------------------------------------------------------------------------");
                Debug.Log("아바타 정보 리스트: " + avatarIndexData);
                Debug.Log("--------------------------------------------------------------------------------");
            };
            StartCoroutine(HttpManager.GetInstance().Get(info));
        }

        private void AvatarEdit(int skinId, int clothesId, int faceId, int hairId)
        {
            StartCoroutine(CoAvatarEdit(skinId, clothesId, faceId, hairId));
        }

        public IEnumerator CoAvatarEdit(int skinId, int clothesId, int faceId, int hairId)
        {
            yield return new WaitUntil(() => DataManager.instance.player != null);

            GameObject player = DataManager.instance.player;
            if (player && player.GetPhotonView().IsMine)
            {
                player.GetPhotonView().RPC("SetAvatarPart", RpcTarget.AllBuffered, skinId, clothesId, faceId, hairId);
            }
        }
    }
}
