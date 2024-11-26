using GH;
using SW;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static HttpManager;
using UnityEngine.Networking;
using TMPro;
using UnityEngine.UI;
using static System.Net.Mime.MediaTypeNames;

#region DTO
// 튜토리얼 
[System.Serializable]
public struct Tutorial
{
    public string title;
    public string content;
    public int count;
    public int exp;
    public int money;
}

// 퀘스트 정보 Patch
public struct QuestPatch
{
    public int userQuestId;
    public int userId;
    public int questId;
    public int count;
    public bool isComplete;
}

// 플레이어 퀘스트 리스트
[System.Serializable]
public struct UserQuest
{
    public int userQuestId;
    public QuestInfo quest;
    public int count;
    public bool isComplete;

}

[System.Serializable]
public struct QuestInfo
{
    public int questId;
    public string title;
    public string content;
    public int count;
    public string questType;
    public int gold;
    public int exp;
    public List<QuestRewardItem> rewardInfo;
}


[System.Serializable]
public struct UserQuestList
{
    public List<UserQuest> data;
}

[System.Serializable]
public struct QuestRewardItem
{
    public int itemIdx;
    public int count;
}


#endregion
public class QuestManager : MonoBehaviour
{
    public static QuestManager instance;

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
    public List<Tutorial> tutorialQuestList;
    public UserQuestList userQuestList;
    public UserQuest userQuestData;

    [Header("퀘스트UI")]
    public List<GameObject> questItemList;
    public GameObject questPanel;
    public TMP_Text expText;
    public TMP_Text gemText;
    public RectTransform questImageTransform;
    public GameObject questItemPrefabs;
    public TMP_Text questTitle;



    [Header("미션 버튼")]
    public Button missonOnButton;
    public Button dailyMissonButton;
    public Button allMissonOnButton;
    public Button MissonOffButton;

    [Header("미션 아이템 프리펩")]
    public GameObject missionPrefabs;

    public RectTransform missionListTransform;

    public List<GameObject> missionList;

    public GameObject missionPanel;

    private void Start()
    {
        missonOnButton.onClick.AddListener(MissionPanelOn);
        dailyMissonButton.onClick.AddListener(MissionGet);
        allMissonOnButton.onClick.AddListener(MissionGet);
        MissonOffButton.onClick.AddListener(MissionPanelOff);
    }
    private void Update()
    {
        if (Input.touchCount == 1 || Input.GetMouseButtonDown(0))
        {
            questPanel.SetActive(false);
        }
    }
    //퀘스트 달성 보내기    
    public void QuestPatch(int questId)
    {
        print("퀘스트 체크!!");
        HttpInfo info = new HttpInfo();
        info.url = HttpManager.GetInstance().SERVER_ADRESS + "/user-quest/" + questId + "/" + AuthManager.GetInstance().userAuthData.userInfo.id;
        info.onComplete = (DownloadHandler downloadHandler) =>
        {
            string jsonData = downloadHandler.text;
            userQuestData = JsonUtility.FromJson<UserQuest>(jsonData);
            print("get : " + jsonData);
            // 퀘스트 상태가 false면
            if (userQuestData.isComplete == false)
            {
                QuestPatch questPatch = new QuestPatch();
                questPatch.userId = AuthManager.GetInstance().userAuthData.userInfo.id;
                questPatch.questId = questId;
                questPatch.userQuestId = userQuestData.userQuestId;
                questPatch.count = userQuestData.count--;
                questPatch.isComplete = true;

                HttpInfo info = new HttpInfo();
                info.url = HttpManager.GetInstance().SERVER_ADRESS + "/user-quest";
                info.body = JsonUtility.ToJson(questPatch);
                info.contentType = "application/json";
                info.onComplete = (DownloadHandler downloadHandler) =>
                {
                    print("Patch : " + downloadHandler.text);
                    //퀘스트 완료 창 띄우기
                    QuestSuccess(userQuestData);
                };
                StartCoroutine(HttpManager.GetInstance().Patch(info));
            }
        };
        StartCoroutine(HttpManager.GetInstance().Get(info));
    }
    private void QuestSuccess(UserQuest userQuest)
    {
        questPanel.SetActive(true);
        for (int i = 0; i < questItemList.Count; i++)
        {
            Destroy(questItemList[i]);
        }
        questItemList.Clear();
        // 퀘스트 아이텔 프리펩 생성하기
        for (int i = 0; i < userQuest.quest.rewardInfo.Count; i++)
        {
            GameObject rewardItem = Instantiate(questItemPrefabs, questImageTransform);
            questItemList.Add(rewardItem);
            //아이템 인덱스로 이름 받아오기! ==== 규할일 ====

            //rewardItem.GetComponent<Image>().sprite
            rewardItem.GetComponent<TMP_Text>().text = "이름" + " x " + userQuest.quest.rewardInfo[i].count;
        }
        // 잼이랑 경험치 수치 수정하기
        gemText.text = "잼 +" + userQuest.quest.gold;
        expText.text = "경험치 +" + userQuest.quest.exp;
        questTitle.text = userQuest.quest.title;



    }
    public void UserQuestListGet()
    {

        HttpInfo info = new HttpInfo();
        info.url = HttpManager.GetInstance().SERVER_ADRESS + "/user-quest/list/" + AuthManager.GetInstance().userAuthData.userInfo.id;
        info.onComplete = (DownloadHandler downloadHandler) =>
        {
            string jsonData = "{ \"data\" : " + downloadHandler.text + "}";
            print(jsonData);
            //jsonData를 PostInfoArray 형으로 바꾸자.
            userQuestList = JsonUtility.FromJson<UserQuestList>(jsonData);
            //print("get : " + userQuestList);
        };
        StartCoroutine(HttpManager.GetInstance().Get(info));
    }

    public void MissionPanelOn()
    {
        missionPanel.SetActive(true);
        MissionGet();
    } 
    public void MissionPanelOff()
    {
        missionPanel.SetActive(false);
    }
    private void MissionGet()
    {
        for(int i = 0; i < missionList.Count; i++)
        {
            Destroy(missionList[i]);
        }
        missionList.Clear();


        HttpInfo info = new HttpInfo();
        info.url = HttpManager.GetInstance().SERVER_ADRESS + "/user-quest/list/not-completed/" + AuthManager.GetInstance().userAuthData.userInfo.id;
        info.onComplete = (DownloadHandler downloadHandler) =>
        {
            print(downloadHandler.text);
            string jsonData = "{ \"data\" : " + downloadHandler.text + "}";

            userQuestList = JsonUtility.FromJson<UserQuestList>(jsonData);

            for (int i = 0; i < userQuestList.data.Count; i++)
            {
                MissionItemInfo missionItemInfo = Instantiate(missionPrefabs, missionListTransform).GetComponent<MissionItemInfo>();
                missionList.Add(missionItemInfo.gameObject);

                missionItemInfo.title.text = userQuestList.data[i].quest.title;
                missionItemInfo.content.text = "- "+userQuestList.data[i].quest.content;
                missionItemInfo.rewardEXP.text = userQuestList.data[i].quest.exp.ToString();
                missionItemInfo.rewardGem.text = userQuestList.data[i].quest.gold.ToString();
                //아이템 인덱스로 이름 받아오기! ==== 규할일 ====
               
            }
        };
        StartCoroutine(HttpManager.GetInstance().Get(info));
    }

}