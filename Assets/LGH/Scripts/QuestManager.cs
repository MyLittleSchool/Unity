using GH;
using SW;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static HttpManager;
using UnityEngine.Networking;

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
    public int id;
    public string content;
    public int count;
    public string questType;
    public int gold;
    public int exp;
}


[System.Serializable]
public struct UserQuestList
{
    public List<UserQuest> data;
}
#endregion
public class QuestManager : MonoBehaviour
{
    public QuestManager instance;

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

    void Start()
    {

    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Y))
        {
            UserQuestListGet();
        }

        if (Input.GetKeyDown(KeyCode.U))
        {
            QuestPatch(3);
        }
    }


    public void QuestPatch(int questId)
    {
        HttpInfo info = new HttpInfo();
        info.url = HttpManager.GetInstance().SERVER_ADRESS + "/user-quest/" + questId + "/" + AuthManager.GetInstance().userAuthData.userInfo.id;
        info.onComplete = (DownloadHandler downloadHandler) =>
        {
            string jsonData = downloadHandler.text;
            userQuestData = JsonUtility.FromJson<UserQuest>(jsonData);
            print("get : " + jsonData);

            QuestPatch questPatch = new QuestPatch();
            questPatch.userId = AuthManager.GetInstance().userAuthData.userInfo.id;
            print(AuthManager.GetInstance().userAuthData.userInfo.id);
            questPatch.questId = questId;
            print(questId);
            questPatch.userQuestId = userQuestData.userQuestId;
            print(userQuestData.userQuestId);
            questPatch.count = userQuestData.count--;
            print(userQuestData.count--);
            questPatch.isComplete = true;

            HttpInfo info = new HttpInfo();
            info.url = HttpManager.GetInstance().SERVER_ADRESS + "/user-quest";
            info.body = JsonUtility.ToJson(questPatch);
            info.contentType = "application/json";
            info.onComplete = (DownloadHandler downloadHandler) =>
            {
                print("Patch : " + downloadHandler.text);
            };
            StartCoroutine(HttpManager.GetInstance().Patch(info));

        };
        StartCoroutine(HttpManager.GetInstance().Get(info));





       
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
}
