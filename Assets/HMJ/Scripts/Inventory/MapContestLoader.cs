using GH;
using SW;
using System;
using System.Collections.Generic;
using System.Resources;
using UnityEngine;
using UnityEngine.Networking;
using static HttpManager;
using static MapRegisterDataUI;
namespace MJ
{
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

        public List<Texture2D> sprites = new List<Texture2D>();

        public List<ObjectContestInfo> loadfurnitureList;


        public DeleteItemDataList deleteItemDataLists;
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
            info.body = Application.dataPath + "/Resources/" + mapRoute; // 파일 경로
            info.onComplete = (DownloadHandler downloadHandler) =>
            {
                string fileName = downloadHandler.text.ToString().Substring(39);
                SendMapData(mapRegisterData, fileName);

            };
            StartCoroutine(HttpManager.GetInstance().UploadFileByFormData(info, mapRoute));

        }

        public void SendMapData(MapRegisterData mapRegisterData, string url)
        {
            MapContestData mapContestInfo = new MapContestData();

            mapContestInfo.title = mapRegisterData.title;
            mapContestInfo.description = mapRegisterData.Description;
            mapContestInfo.userId = DataManager.instance.mapId;
            mapContestInfo.previewImageUrl = url;
            HttpInfo info = new HttpInfo();
            info.url = HttpManager.GetInstance().SERVER_ADRESS + "/map-contest";
            info.body = JsonUtility.ToJson(mapContestInfo);
            info.contentType = "application/json";
            info.onComplete = (DownloadHandler downloadHandler) =>
            {
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
                sprites.Add(handler.texture);
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
                sprites.Clear();
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

            if (sprites.Count == mapDatas.response.Count)
                return true;

            return false;
        }

        public void LoadFurniture()
        {
            SetTile setTile = dataManager.player.GetComponent<SetTile>();
            foreach (ObjectContestInfo info in loadfurnitureList)
            {
                setTile.LoadData(new Vector3Int(info.x, info.y, 0), InventorySystem.GetInstance().GetItemPrefab(info.id), info.id);
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
            info.url = HttpManager.GetInstance().SERVER_ADRESS + "/furniture/list/" + DataManager.instance.mapId;
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

