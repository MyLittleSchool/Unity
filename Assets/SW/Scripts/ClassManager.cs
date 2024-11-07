using GH;
using Photon.Pun;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static SW.PlaceManager;
namespace SW
{
    public class ClassManager : MonoBehaviour
    {
        // Start is called before the first frame update
        void Start()
        {
            StartCoroutine(LoadFurniture());
        }
        private IEnumerator LoadFurniture()
        {
            while (!PhotonNetwork.InRoom)  // 룸입장대기
            {
                yield return null;
            }
            if (!PhotonNetwork.IsMasterClient) yield break; // 방장일 때만
            DataManager dataManager = DataManager.instance;
            while (dataManager.player == null)  // 플레이어 생성 대기
            {
                print(dataManager.player == null);
                yield return null;
            }
            SetTile setTile = dataManager.player.GetComponent<SetTile>();
            PlaceManager placeManager = GetInstance();
            // 배치 로딩
            placeManager.ReadPlace(DataManager.instance.mapId, DataManager.instance.mapType, (GetPlaceResInfo res) =>
            {
                foreach (PlaceInfo info in res.response)
                {
                    setTile.LoadData(new Vector3Int(info.x, info.y, 0), InventorySystem.GetInstance().items[info.objId].prefab, info.id);
                }
            });
        }
    }
    [Serializable]
    public class School
    {
        public int schoolId;
        public string schoolName;
        public string location;
        public int backgroundColorId;
        public List<PlaceManager.ObjectInfo> furnitureList;
    }
}