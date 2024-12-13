using GH;
using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class SoccerObject : MonoBehaviour/*, IPunObservable*/
{
    private PhotonView pv;
    private Rigidbody2D rigidbody;
    private float forceMagnitude = 5.0f;
    string kickPlayer;

    // 축구 결과창
    public GameObject UIPanel;

    private void Awake()
    {
        rigidbody = GetComponentInChildren<Rigidbody2D>();
        pv = GetComponent<PhotonView>();
    }

    public void KickBall()
    {
        Vector2 joyStickData = new Vector2(GameManager.instance.Joystick.Horizontal, GameManager.instance.Joystick.Vertical).normalized;
        PlayerMove playerMove = DataManager.instance.player.GetComponent<PlayerMove>();

        pv.RPC("CalculateBallDirection", RpcTarget.All, playerMove.joystickDeg, joyStickData);        
    }

    [PunRPC]
    public void CalculateBallDirection(float degree, Vector2 joyStickData)
    {

        float radian = degree * Mathf.Deg2Rad;

        Vector2 direction = new Vector2(Mathf.Cos(radian) * Mathf.Abs(joyStickData.x), Mathf.Sin(radian) * Mathf.Abs(joyStickData.y + 0.05f));

        Vector2 force = direction * forceMagnitude;

        rigidbody.AddForce(force, ForceMode2D.Impulse);
    }

    [PunRPC]
    public void ResetBall(Vector3 vec3Position, string _kickPlayer)
    {
        transform.position = vec3Position;
        rigidbody.velocity = Vector2.zero;
        rigidbody.angularVelocity = 0.0f;
        kickPlayer = _kickPlayer;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        int[] layer = { LayerMask.NameToLayer("NetCollision"), LayerMask.NameToLayer("OutCollision") };

        // 플레이어와 충돌
        if (collision.gameObject.name.Contains("Player") && (collision.gameObject.GetComponent<PhotonView>().IsMine))
        {
            pv.RPC("ResetBall", RpcTarget.All, transform.position, DataManager.instance.playerName);
            KickBall();
        }

        // 특정 레이어와 충돌 - 네트 및 외부 콜리전
        for (int i = 0; i < layer.Length; i++)
            if (collision.gameObject.layer == layer[i])
            {
                if (collision.gameObject.layer == LayerMask.NameToLayer("NetCollision"))
                    SendSoccerWin();
                StartCoroutine(ResetSoccerPosition(0.3f));
            }
    }

    private IEnumerator ResetSoccerPosition(float delayTime)
    {
        yield return new WaitForSeconds(delayTime);

        transform.position = new Vector3(16.5f, -22.5f, 0.0f);
        rigidbody.velocity = Vector2.zero;
        rigidbody.angularVelocity = 0.0f;
        yield return null;
    }

    public void SendSoccerWin()
    {
        pv.RPC("SoccerWinPlayUI", RpcTarget.All);
    }

    [PunRPC]
    public void SoccerWinPlayUI()
    {
        FadeOutUI fadeOutUI = UIPanel.GetComponentInChildren<FadeOutUI>();
        fadeOutUI.GetComponentInChildren<TMP_Text>().text = kickPlayer + " 축구 성공!";
        UIPanel.GetComponentInChildren<FadeOutUI>().FadeInOut(0.0f, 3.0f);
    }
}
/*
         if (gameInteractButton.GetInstance().GetButtonDown())
        {
            Debug.Log("현재 Soccer: " + gameObject.name);
            Debug.Log("버튼 클릭!");
            if (null == dribblePlayer) // 현재 드리블 중인 플레이어 없을 때
                pv.RPC("SyncDribblingPlayer", RpcTarget.All, DataManager.instance.player.GetPhotonView().ViewID);
            //else if (dribblePlayer.GetPhotonView().ViewID == DataManager.instance.player.GetPhotonView().ViewID) // 현재 드리블 중인 플레이어가 있고 본인일 경우
            //    pv.RPC("ResetDribblingPlayer", RpcTarget.All);
        }
        if(dribblePlayer)
        {
            Debug.Log("현재 드리블 중!");
            transform.position = dribblePlayer.transform.position;
        }
 */