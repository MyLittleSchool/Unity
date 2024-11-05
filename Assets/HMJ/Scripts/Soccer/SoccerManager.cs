using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoccerManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        CoSpawnSoccerObject();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void CoSpawnSoccerObject()
    {
        StartCoroutine(SpawnSoccerObject());
    }

    IEnumerator SpawnSoccerObject()
    {
        Vector3 initPosition = new Vector3(16.5f, -22.5f, 0.0f);
        //룸에 입장이 완료될 때까지 기다린다.
        yield return new WaitUntil(() => { return PhotonNetwork.InRoom; });
        if (PhotonNetwork.IsMasterClient)
            PhotonNetwork.Instantiate("Soccer", initPosition, Quaternion.identity);
    }
}
