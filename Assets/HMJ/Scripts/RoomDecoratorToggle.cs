using System.Collections;
using System.Collections.Generic;
using GH;
using MJ;
using Photon.Pun;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI.Table;

public class RoomDecoratorToggle : MonoBehaviour
{
    public enum DecorationPermission
    {
        Allowed,   // 꾸미기 허용
        NotAllowed // 꾸미기 비허용
    }

    public DecorationPermission currentPermission = DecorationPermission.NotAllowed;



    public void ToggleDecorationPermission()
    {
        currentPermission = currentPermission == DecorationPermission.Allowed
            ? DecorationPermission.NotAllowed
            : DecorationPermission.Allowed;

        Debug.Log($"현재 꾸미기 상태: {currentPermission}");
        OnPermissionChanged(currentPermission);
    }

    // 허용 상태 변경
    [PunRPC]
    public void EnableDecoration()
    {
        Debug.Log("꾸미기가 허용되었습니다.");
        SceneUIManager.GetInstance().OnInventoryUI();
    }

    // 비허용 상태 변경
    [PunRPC]
    public void DisableDecoration()
    {
        Debug.Log("꾸미기가 비허용되었습니다.");
        SceneUIManager.GetInstance().OffInventoryUI();
    }

    public void RPC_SettingDecoration(bool bToggle)
    {
        PhotonView pv = GetComponent<PhotonView>();
        if (null == pv)
            return;

        if (bToggle)
            pv.RPC("EnableDecoration", RpcTarget.OthersBuffered);
        else
            pv.RPC("DisableDecoration", RpcTarget.OthersBuffered);
    }

    // 상태 변경 시 호출
    public void OnPermissionChanged(DecorationPermission newPermission)
    {
        RPC_SettingDecoration(newPermission == DecorationPermission.Allowed);
    }

}
