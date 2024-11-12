using GH;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json;
using Photon.Pun;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static SW.PlaceManager;
using UnityEngine.Networking;
using UnityEditor.U2D.Aseprite;
using System.IO;
using static HttpManager;
using UnityEngine.Analytics;
using static MapRegisterDataUI;
using System.Net;
using static SW.FriendsUI;
using SW;
using System.Security.Cryptography;
using ExitGames.Client.Photon;
using System.Text;
using UnityEngine.Rendering;
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
        public int objId; // 오브젝트 아이디
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
            info.url = httpManager.SERVER_ADRESS + "/map-contest/upload-image";
            info.contentType = "multipart/form-data";
            info.body = Application.dataPath + "/Resources/" + mapRoute; // 파일 경로
            info.onComplete = (DownloadHandler downloadHandler) =>
            {
                string fileName = downloadHandler.text.ToString().Substring(39);
                SendMapData(mapRegisterData, fileName);

            };
            StartCoroutine(httpManager.UploadFileByFormData(info, mapRoute));

        }

        public void SendMapData(MapRegisterData mapRegisterData, string url)
        {
            MapContestData mapContestInfo = new MapContestData();

            mapContestInfo.title = mapRegisterData.title;
            mapContestInfo.description = mapRegisterData.Description;
            mapContestInfo.userId = 1;
            mapContestInfo.previewImageUrl = url;
            HttpInfo info = new HttpInfo();
            info.url = httpManager.SERVER_ADRESS + "/map-contest";
            info.body = JsonUtility.ToJson(mapContestInfo);
            info.contentType = "application/json";
            info.onComplete = (DownloadHandler downloadHandler) =>
            {
                print(downloadHandler.text);
            };

            StartCoroutine(httpManager.Post(info));
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
            StartCoroutine(httpManager.DownloadSprite(info));
        }
        //public string title;
        //public string description;
        //public int mapId;
        //public string imageUrl;

        public void LoadMapData()
        {
            HttpInfo info = new HttpInfo();
            info.url = httpManager.SERVER_ADRESS + "/map-contest/list";
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
            StartCoroutine(httpManager.Get(info));
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
                setTile.LoadData(new Vector3Int(info.x, info.y, 0), InventorySystem.GetInstance().items[info.objId].prefab, info.id);
            }
        }

        public void MapContestEditSave(MapContestData mapContestInfo)
        {
            HttpInfo info = new HttpInfo();
            info.url = httpManager.SERVER_ADRESS + "/map-contest";
            info.body = JsonUtility.ToJson(mapContestInfo);
            info.contentType = "application/json";
            info.onComplete = (DownloadHandler downloadHandler) =>
            {
                print(downloadHandler.text);
            };
            StartCoroutine(httpManager.Patch(info));
        }

        public void MapContestDeleteAllFurniture()
        {
            HttpManager.HttpInfo info = new HttpManager.HttpInfo();
            info.url = httpManager.SERVER_ADRESS + "/furniture/list/" + DataManager.instance.mapId;
            info.onComplete = (DownloadHandler res) =>
            {
                deleteItemDataLists = JsonUtility.FromJson<DeleteItemDataList>(res.text);
                print("제거완료");
            };
            StartCoroutine(httpManager.Delete(info));
            // no -> 다시 깔고

            // yes -> (인벤토리 개수 + 이전에 깔았던 아이템 개수)

        } 


        /*
         *  MapContestData mapContestInfo = new MapContestData();

            mapContestInfo.title = mapRegisterData.title;
            mapContestInfo.description = mapRegisterData.Description;
            mapContestInfo.userId = 1;
            mapContestInfo.previewImageUrl = url;
            HttpInfo info = new HttpInfo();
            info.url = httpManager.SERVER_ADRESS + "/map-contest";
            info.body = JsonUtility.ToJson(mapContestInfo);
            info.contentType = "application/json";
            info.onComplete = (DownloadHandler downloadHandler) =>
            {
                print(downloadHandler.text);
            };

            StartCoroutine(httpManager.Post(info));
         */
    }
}

/*
 * ArgumentException: JSON must represent an object type.
UnityEngine.JsonUtility.FromJson (System.String json, System.Type type) (at <5ee00f1c11864dcd8992e826540f733b>:0)
UnityEngine.JsonUtility.FromJson[T] (System.String json) (at <5ee00f1c11864dcd8992e826540f733b>:0)
 */