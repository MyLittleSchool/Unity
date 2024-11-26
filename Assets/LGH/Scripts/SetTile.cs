using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Photon.Pun;
using SW;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;
using static GH.DataManager;
using static Item;

namespace GH
{
    [System.Serializable]
    public class ObjectInfo
    {
        public GameObject obj; // 오브젝트를 저장할 변수
        public Vector3 position; // 위치를 저장할 변수
        public int id; // DB id

        // 생성자
        public ObjectInfo(GameObject obj, Vector3 position)
        {
            this.id = -1;
            this.obj = obj;
            this.position = position;
        }
    }
    public class SetTile : MonoBehaviourPun
    {
        public static SetTile instance;
        public Tilemap tilemap;
        public Transform playerFrontTileTransform;
        private Vector3Int tilePosition;
        private Grid grid;
        public GameObject setGameObject;
        public ItemType setItemType;
        public TileBase emptyTilebase;
        public GameObject tileLine;

        public bool setMode;

        public List<ObjectInfo> objectList = new List<ObjectInfo>();

        public bool tileObjCheck;
        public int setObjectId;

        public PlayerMove playerMove;

        void Start()
        {
            if (photonView.IsMine) instance = this;
            setMode = false;
            tileLine.SetActive(false);
            SuchGrid();
            playerMove =GetComponent<PlayerMove>();
        }

        void Update()
        {
            tileLine.SetActive(setMode);
            //쓰레기 코드 베타에 수정
            if (DataManager.instance.setTileObj != null)
            {
                setGameObject = DataManager.instance.setTileObj;
            }
            setObjectId = DataManager.instance.setTileObjId;


            if (setMode)
            {

                if (playerMove.stingDir == -transform.up)
                {
                    playerFrontTileTransform.localPosition = new Vector3(0, 0, 0);
                }
                else if (playerMove.stingDir == transform.up)
                {
                    playerFrontTileTransform.localPosition = new Vector3(0, 2, 0);

                }
                else if (playerMove.stingDir == transform.right)
                {
                    playerFrontTileTransform.localPosition = new Vector3(1, 1, 0);

                }
                else if (playerMove.stingDir == -transform.right)
                {

                    playerFrontTileTransform.localPosition = new Vector3(-1, 1, 0);

                }

                tilePosition = grid.WorldToCell(playerFrontTileTransform.position);
                tileLine.transform.position = tilePosition;
                if (Input.GetKeyDown(KeyCode.Q))
                {
                    OnTile();
                }
                if (Input.GetKeyDown(KeyCode.E))
                {
                    RPC_DeleteTile();
                }
                tileObjCheck = tilemap.HasTile(tilePosition);
                //print(tileObjCheck);

            }
        }
        public void TileMapSetTile(Vector3Int pos, bool remove = false)
        {
            if (remove) tilemap.SetTile(pos, null);
            else tilemap.SetTile(pos, emptyTilebase);
        }
        public void OnTile()
        {
            if (!tilemap.HasTile(tilePosition) && InventorySystem.GetInstance().CheckItem())
            {
                tilemap.SetTile(tilePosition, emptyTilebase);
                GameObject setObject = PhotonNetwork.Instantiate("Furnitures/" + setGameObject.name, tilePosition, Quaternion.identity);

                InventorySystem.GetInstance().PatchItemData(InventorySystem.GetInstance().GetCurItemType(), setGameObject.name, -1);
                //setObject.transform.position = tilePosition;
                AddObject(setObject);
                
                
                print(tilemap.HasTile(tilePosition));
                // 통신
                ObjectInfo obj = objectList.Last();
                PlaceManager.ObjectInfo objectInfo = new PlaceManager.ObjectInfo();
                objectInfo.objId = setObjectId;
                objectInfo.x = tilePosition.x;
                objectInfo.y = tilePosition.y;
                //objectInfo.rot =
                objectInfo.mapId = DataManager.instance.mapId;
                objectInfo.mapType = DataManager.instance.MapTypeState;
                PlaceManager.GetInstance().CreatePlace(objectInfo, (PlaceManager.PlaceInfo callBack) =>
                {
                    obj.id = callBack.id;
                });
            }
        }

        public void CopyTile(Vector3Int _tilePosition, int objId)
        {
            tilemap.SetTile(_tilePosition, emptyTilebase);
            GameObject setObject = PhotonNetwork.Instantiate("Furnitures/" + setGameObject.name, _tilePosition, Quaternion.identity);
            //setObject.transform.position = tilePosition;
            AddObject(setObject);

            print(tilemap.HasTile(_tilePosition));
            // 통신
            ObjectInfo obj = objectList.Last();
            PlaceManager.ObjectInfo objectInfo = new PlaceManager.ObjectInfo();
            objectInfo.objId = objId;
            objectInfo.x = _tilePosition.x;
            objectInfo.y = _tilePosition.y;
            //objectInfo.rot =
            objectInfo.mapId = DataManager.instance.mapId;
            objectInfo.mapType = MapType.MyClassroom;
            PlaceManager.GetInstance().CreatePlace(objectInfo, (PlaceManager.PlaceInfo callBack) =>
            {
                obj.id = callBack.id;
            });
        }

        public void RPC_DeleteTile()
        {
            if (tilemap.HasTile(tilePosition))
            {
                foreach (ObjectInfo obj in objectList)
                {
                    if (obj.position == tilePosition)
                    {
                        photonView.RPC(nameof(ReqDeleteTile), obj.obj.GetPhotonView().Owner, tilePosition.x, tilePosition.y, tilePosition.z);
                    }
                }
            }
        }
        [PunRPC]
        public void ReqDeleteTile(int x, int y, int z)
        {
            Vector3Int pos = new Vector3Int(x, y, z);
            DataManager.instance.player.GetComponent<SetTile>().DeleteTile(pos);
        }
        public void DeleteTile(Vector3Int pos)
        {
            if (tilemap.HasTile(pos))
            {
                tilemap.SetTile(pos, null);
                foreach (ObjectInfo obj in objectList)
                {
                    if (obj.position == pos)
                    {
                        PhotonNetwork.Destroy(obj.obj);
                        PlaceManager.GetInstance().DeletePlace(obj.id); // 통신
                    }
                }
            }
        }
        public void SuchGrid()
        {
            grid = GameObject.Find("Grid").GetComponent<Grid>();
            tilemap = grid.transform.GetChild(0).GetComponent<Tilemap>();
        }

        public void AddObject(GameObject obj)
        {
            Vector3 position = obj.transform.position; // 오브젝트의 현재 위치를 가져옴
            ObjectInfo newObjectInfo = obj.GetComponent<Furnitures>().objectInfo; // 새 ObjectInfo 객체 생성
            objectList.Add(newObjectInfo); // 리스트에 추가
        }
        public void LoadData(Vector3Int pos, GameObject obj, int id)
        {
            tilePosition = pos;
            setGameObject = obj;
            if (!tilemap.HasTile(tilePosition))
            {
                tilemap.SetTile(tilePosition, emptyTilebase);
                GameObject setObject = PhotonNetwork.Instantiate("Furnitures/" + setGameObject.name, tilePosition, Quaternion.identity);
                //setObject.transform.position = tilePosition;
                AddObject(setObject);
                objectList.Last().id = id;
                //89print(tilemap.HasTile(tilePosition));
            }
        }
    }

}