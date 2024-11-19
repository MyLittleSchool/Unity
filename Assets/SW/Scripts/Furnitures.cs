using GH;
using Photon.Pun;
using Photon.Realtime;
using SW;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Furnitures : MonoBehaviourPunCallbacks
{
    public ObjectInfo objectInfo;
    private void Awake()
    {
        objectInfo = new ObjectInfo(gameObject, transform.position);   
    }
    private void Start()
    {
        if (!photonView.IsMine) photonView.RPC(nameof(ReqSync), photonView.Owner, PhotonNetwork.LocalPlayer.ActorNumber);
    }
    [PunRPC]
    public void ReqSync(int actorNumber)
    {
        StartCoroutine(WaitLoad(actorNumber));
    }
    private IEnumerator WaitLoad(int actorNumber)
    {
        yield return new WaitUntil(() => { return !FurnitureLoader.instance.isLoading && objectInfo.id != -1; });
        photonView.RPC(nameof(ResSync), PhotonNetwork.CurrentRoom.GetPlayer(actorNumber), objectInfo.id);
    }
    [PunRPC]
    public void ResSync(int objId)
    {
        SetTile.instance.TileMapSetTile(new Vector3Int((int)transform.position.x, (int)transform.position.y, (int)transform.position.z));
        objectInfo.id = objId;
        SetTile.instance.objectList.Add(objectInfo);
    }
    private void OnDestroy()
    {
        SetTile.instance.TileMapSetTile(new Vector3Int((int)objectInfo.position.x, (int)objectInfo.position.y, (int)objectInfo.position.z), true);
        SetTile.instance.objectList.Remove(objectInfo);
    }
}
