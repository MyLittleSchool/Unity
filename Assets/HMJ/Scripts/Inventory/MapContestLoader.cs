using GH;
using SW;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Resources;
using UnityEngine;
using UnityEngine.Networking;
using static HttpManager;
using static MapRegisterDataUI;
namespace MJ
{
    [System.Serializable]
    public class ImageData
    {
        public string url;
        public string publicId;
    }

    [Serializable]
    public class ObjectContestInfo
    {
        public int id;
        public int objId; // 오브젝트 아이디
        public int x, y; // 좌표
        public int rot; // 회전
        public bool flip; // 반전
        public int mapId; // 맵아이디
        public DataManager.MapType mapType;
    }

    [Serializable]
    public struct MapContestData
    {
        public int id;
        public string title;
        public string description;
        public List<ObjectContestInfo> furnitureList;
        public string previewImageUrl;
        public int likeCount;
        public int viewCount;
        public int userId;
        public string publicId;
    }

    [Serializable]
    public class MapContestDataList
    {
        public List<MapContestData> response;
    }

    [Serializable]
    public struct DeleteItemData
    {
        public int objectId; // 오브젝트 아이디
        public int removedCount; // 제거한 오브젝트 개수
    }

    [Serializable]
    public class DeleteItemDataList
    {
        public List<DeleteItemData> response;
    }

    public class MapContestLoader : MonoBehaviour
    {
        public static MapContestLoader instance;
        // Start is called before the first frame update

        private DataManager dataManager;
        private HttpManager httpManager;
        public MapContestDataList mapDatas;
        public MapContestScrollUI mapContestScrollUIComponent;

        public Texture2D[] sprites;
        //public List<Texture2D> sprites = new List<Texture2D>();

        public List<ObjectContestInfo> loadfurnitureList;


        public DeleteItemDataList deleteItemDataLists;
        private void Awake()
        {
            if (instance == null)
            {
                instance = this;
                DontDestroyOnLoad(gameObject);
                sprites = new Texture2D[100];
            }
            else
            {
                Destroy(gameObject);
            }

        }

        public static MapContestLoader GetInstance()
        {
            if (instance == null)
            {
                GameObject go = new GameObject();
                go.name = "MapContestLoader";
                go.AddComponent<MapContestLoader>();
            }
            return instance;
        }

        void Start()
        {
            dataManager = DataManager.instance;
            httpManager = HttpManager.GetInstance();
        }

        public void SendMapContestData(string mapRoute, MapRegisterData mapRegisterData)
        {
            HttpInfo info = new HttpInfo();
            info.url = HttpManager.GetInstance().SERVER_ADRESS + "/map-contest/upload-image";
            info.contentType = "multipart/form-data";
            info.body = mapRoute; // 파일 이름
            info.onComplete = (DownloadHandler downloadHandler) =>
            {
                string jsonResponse = downloadHandler.text;

                try
                {
                    // JSON 파싱을 위한 임시 클래스
                    var jsonData = JsonUtility.FromJson<ImageData>(jsonResponse);

                    // URL과 publicId 추출
                    string url = jsonData.url;
                    string publicId = jsonData.publicId;

                    // 추출된 데이터 디버깅 출력
                    Debug.Log("URL: " + url);
                    Debug.Log("Public ID: " + publicId);

                    // 파일 이름 전달 (publicId를 기준으로 전달)
                    SendMapData(mapRegisterData, url, publicId);
                    Debug.Log("맵 콘테스트 이미지 보내기 성공");
                }
                catch (System.Exception ex)
                {
                    Debug.LogError("JSON 파싱 실패: " + ex.Message);
                }
            };
            StartCoroutine(HttpManager.GetInstance().UploadFileByFormData(info, mapRoute));

        }

        public void SendMapData(MapRegisterData mapRegisterData, string url, string PublicId)
        {
            MapContestData mapContestInfo = new MapContestData();

            mapContestInfo.title = mapRegisterData.title;
            mapContestInfo.description = mapRegisterData.Description;
            mapContestInfo.userId = DataManager.instance.mapId;
            mapContestInfo.previewImageUrl = url;
            mapContestInfo.publicId = PublicId;

            HttpInfo info = new HttpInfo();
            info.url = HttpManager.GetInstance().SERVER_ADRESS + "/map-contest";
            info.body = JsonUtility.ToJson(mapContestInfo);
            info.contentType = "application/json";
            info.onComplete = (DownloadHandler downloadHandler) =>
            {
                Debug.Log("맵 콘테스트 데이터 보내기 성공");
                print(downloadHandler.text);
            };

            StartCoroutine(HttpManager.GetInstance().Post(info));
        }

        public void ReceiveMapImage(string ImageUrl, int idx)
        {
            HttpInfo info = new HttpInfo();
            info.url = ImageUrl;
            info.onComplete = (DownloadHandler downloadHandler) =>
            {
                DownloadHandlerTexture handler = downloadHandler as DownloadHandlerTexture;
                sprites[idx] = handler.texture;
                //sprites[idx] = sprite;
            };
            StartCoroutine(HttpManager.GetInstance().DownloadSprite(info));
        }
        //public string title;
        //public string description;
        //public int mapId;
        //public string imageUrl;

        public void LoadMapData()
        {
            HttpInfo info = new HttpInfo();
            info.url = HttpManager.GetInstance().SERVER_ADRESS + "/map-contest/list";
            info.onComplete = (DownloadHandler downloadHandler) =>
            {
                mapDatas = JsonUtility.FromJson<MapContestDataList>(downloadHandler.text);
                for (int i = 0; i < sprites.Length; i++)
                    sprites[i] = null;
                Debug.Log("--------------------------------------------------------------------------------");
                for (int i = 0; i < mapDatas.response.Count; i++)
                    ReceiveMapImage(mapDatas.response[i].previewImageUrl, i);
                mapContestScrollUIComponent.LoadMapData();
                Debug.Log("--------------------------------------------------------------------------------");
            };
            StartCoroutine(HttpManager.GetInstance().Get(info));
        }

        public bool LoadSpriteComplete()
        {
            if (mapDatas.response.Count <= 0)
                return false;

            int count = 0;
            for(int i = 0; i < sprites.Count(); i++)
            {
                if (sprites[i] != null)
                    count++;
            }
            if (count == mapDatas.response.Count)
                return true;

            return false;
        }

        public void LoadFurniture()
        {
            SetTile setTile = dataManager.player.GetComponent<SetTile>();
            foreach (ObjectContestInfo info in loadfurnitureList)
            {
                setTile.LoadData(new Vector3Int(info.x, info.y, 0), InventorySystem.GetInstance().GetItemIndex(info.id), info.id);
            }
        }

        public void MapContestEditSave(MapContestData mapContestInfo)
        {
            HttpInfo info = new HttpInfo();
            info.url = HttpManager.GetInstance().SERVER_ADRESS + "/map-contest";
            info.body = JsonUtility.ToJson(mapContestInfo);
            info.contentType = "application/json";
            info.onComplete = (DownloadHandler downloadHandler) =>
            {
                print(downloadHandler.text);
            };
            StartCoroutine(HttpManager.GetInstance().Patch(info));
        }

        public void MapContestDeleteAllFurniture()
        {
            HttpManager.HttpInfo info = new HttpManager.HttpInfo();
            info.url = HttpManager.GetInstance().SERVER_ADRESS + "/furniture/list/" + AuthManager.GetInstance().userAuthData.userInfo.id;
            Debug.Log("MapContestDeleteAllFurniture: " + info.url);
            info.contentType = "application/json";
            info.onComplete = (DownloadHandler res) =>
            {
                deleteItemDataLists = JsonUtility.FromJson<DeleteItemDataList>(res.text);
                print("res값: " + res);
                print("제거완료");
            };
            StartCoroutine(HttpManager.GetInstance().Post(info));

        }

        // 맵 콘테스트 가구 현재 나만의 방에 배치
        public void MapCopyFurniture()
        {
            SetTile setTileComponent = DataManager.instance.player.GetComponent<SetTile>();
            if(setTileComponent)
            {
                foreach (ObjectContestInfo info in loadfurnitureList)
                    setTileComponent.CopyTile(new Vector3Int(info.x, info.y, 0), info.objId);
            }
        }
    }
}

