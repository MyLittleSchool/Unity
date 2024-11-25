using ExitGames.Client.Photon.StructWrapping;
using MJ;
using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using static QuizCategory;

public class QuizCategory : MonoBehaviourPunCallbacks
{
    public enum QUIZCATEGORY
    {
        QUIZCATEGORY00,
        QUIZCATEGORY01,
        QUIZCATEGORY02,
        QUIZCATEGORY03,
        QUIZCATEGORY_END
    }

    public Button[] quizCategorys;

    public TMP_Text[] quizPlayerCount;

    public Button enterQuizRoom;
    public Button exitQuizRoom;

    public int[] playerCount = new int[(int)QUIZCATEGORY.QUIZCATEGORY_END];

    QUIZCATEGORY curQuizCategory = QUIZCATEGORY.QUIZCATEGORY_END;
    string[] curQuizRoomName =
    {
    "퀴즈방: 세계 역사와 문화유산",
    "퀴즈방: 한국어와 맞춤법",
    "퀴즈방: 미술과 문화예술",
    "퀴즈방: 자연과 과학 상식"
    };

    // Start is called before the first frame update
    void Start()
    {
        InitButtonCategory();
        InitButtonEnterExit();
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void SelectQuizCategory(QUIZCATEGORY _QuizCategory)
    {
        curQuizCategory = _QuizCategory;
        QuizLogic.GetInstance().SelectQuiz(curQuizCategory);
    }

    public void InitButtonCategory()
    {
        for(int i = 0; i < quizCategorys.Count(); i++)
        {
            int data = i;
            quizCategorys[i].onClick.AddListener(() => SelectQuizCategory(data + QUIZCATEGORY.QUIZCATEGORY00));
        }
    }

    public void InitButtonEnterExit()
    {
        enterQuizRoom.onClick.AddListener(EnterQuizRoom);
        exitQuizRoom.onClick.AddListener(SceneUIManager.GetInstance().OffQuizCategoryPanel);
    }

    public void EnterQuizRoom()
    {
        SceneUIManager.GetInstance().OffQuizCategoryPanel();
        SceneMgr.instance.QuizIn(curQuizRoomName[(int)curQuizCategory]);
        SceneUIManager.GetInstance().OnQuizQuestionPanel();
    }

    public void RoomListUpdate()
    {
        RoomList.GetInstance().GetRoomList
        (
            (roominfos) =>
            {
                for (int i = 0; i < curQuizRoomName.Count(); i++)
                {
                    quizPlayerCount[i].text = "0";
                    Debug.Log("roomList.Count: " + roominfos.Count);
                    foreach (RoomInfo room in roominfos)
                    {
                        if (room.Name == curQuizRoomName[i]) // 방 이름 일치 확인
                        {
                            quizPlayerCount[i].text = room.PlayerCount.ToString();
                            break;
                        }
                    }
                }
            }
        );

    }

    private void OnEnable()
    {
        RoomListUpdate();
    }
    //// 방 목록 업데이트 콜백
    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
    }
}
