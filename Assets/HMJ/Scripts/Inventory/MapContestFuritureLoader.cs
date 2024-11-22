using GH;
using Photon.Pun;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Playables;
using static SW.PlaceManager;
namespace MJ
{
    public class MapContestFuritureLoader : MonoBehaviour
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
            InventorySystem inventorySystem = InventorySystem.GetInstance();

            SetTile setTile = dataManager.player.GetComponent<SetTile>();
            MapContestLoader mapContestLoader = MapContestLoader.GetInstance();
            // 배치 로딩

            //while (inventorySystem.items.Count == 0)  // 아이템
            //{
            //    yield return null;
            //}

            //foreach (ObjectContestInfo info in mapContestLoader.loadfurnitureList)
            //{
            //    if (inventorySystem.items[info.objId] != null)
            //        setTile.LoadData(new Vector3Int(info.x, info.y, 0), inventorySystem.items[info.objId].prefab, info.id);
            //}
        }
    }

}