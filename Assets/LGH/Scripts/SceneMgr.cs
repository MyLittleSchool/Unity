using GH;
using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneMgr : MonoBehaviour
{
    public static SceneMgr instance;
    [Header("·Î±×ÀÎ ¾À")]
    public TMP_InputField nameInputField;
    public TMP_InputField schoolInputField;
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
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {

    }
    public void Login()
    {
        //¾À ³Ñ¾î°¡±â
        DataManager.instance.playerName = nameInputField.text;
        DataManager.instance.playerSchool = schoolInputField.text;

        SceneManager.LoadScene(2);
    }
    public void ClassIn()
    {
        DataManager.instance.playerCurrChannel = DataManager.instance.playerName;
        PhotonNetMgr.instance.roomName = DataManager.instance.playerName;

        //DataManager.instance.player = null;
        PhotonNetwork.LeaveRoom();
        PhotonNetMgr.instance.sceneNum = 2;
        //SceneManager.LoadScene(2);
        //PhotonNetMgr.instance.CreateRoom();
    }

    public void SquareIn()
    {
        DataManager.instance.playerCurrChannel = "¸¸³²ÀÇ ±¤Àå";
        PhotonNetMgr.instance.roomName = "¸¸³²ÀÇ ±¤Àå";

        PhotonNetwork.LeaveRoom();
        PhotonNetMgr.instance.sceneNum = 3;
    }

    public void SchoolIn()
    {
        DataManager.instance.playerCurrChannel = DataManager.instance.playerSchool;
        PhotonNetMgr.instance.roomName = DataManager.instance.playerSchool;

        PhotonNetwork.LeaveRoom();
        PhotonNetMgr.instance.sceneNum = 1;
    }

    public void QuizIn()
    {
        DataManager.instance.playerCurrChannel = "ÄûÁî";
        PhotonNetMgr.instance.roomName = "ÄûÁî";

        PhotonNetwork.LeaveRoom();
        PhotonNetMgr.instance.sceneNum = 4;
    }

    public void QuizSquareIn()
    {
        DataManager.instance.playerCurrChannel = "ÄûÁî ±¤Àå";
        PhotonNetMgr.instance.roomName = "ÄûÁî ±¤Àå";

        PhotonNetwork.LeaveRoom();
        PhotonNetMgr.instance.sceneNum = 5;
    }

    public void MapContestMapIn(string roomName)
    {
        PhotonNetMgr.instance.roomName = "¸Ê ÄÜÅ×½ºÆ®: " + roomName;

        PhotonNetwork.LeaveRoom();
        PhotonNetMgr.instance.sceneNum = 6;
    }
}
