using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IntoClass : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.layer == LayerMask.NameToLayer("Player") && collision.gameObject.GetComponent<PhotonView>().IsMine)
        {
            SceneMgr.instance.ClassIn();
        }
    }
}
