using GH;
using UnityEngine;
using UnityEngine.Networking;
namespace SW
{
    public class FriendRecommendedPanel : MonoBehaviour
    {
        public void Follow()
        {
            int userId = GetComponent<FriendPanel>().id;
            HttpManager.HttpInfo info = new HttpManager.HttpInfo();
            info.url = HttpManager.GetInstance().SERVER_ADRESS + "/user/" + userId;
            info.onComplete = (DownloadHandler res) =>
            {
                UserInfo userInfo = JsonUtility.FromJson<UserInfo>(res.text);
                if (userInfo.isOnline)
                {
                    if (userInfo.mapType == DataManager.MapType.School.ToString())
                    {
                        HttpManager.HttpInfo info2 = new HttpManager.HttpInfo();
                        info2.url = HttpManager.GetInstance().SERVER_ADRESS + "/school?schoolId=" + userInfo.mapId;
                        info2.onComplete = (DownloadHandler res) =>
                        {
                            School school = JsonUtility.FromJson<School>(res.text);
                            SceneMgr.instance.SchoolIn(school.schoolName, userInfo.mapId);
                            gameObject.SetActive(false);
                        };
                        StartCoroutine(HttpManager.GetInstance().Get(info2));
                    }
                    else if (userInfo.mapType == DataManager.MapType.MyClassroom.ToString())
                    {
                        SceneMgr.instance.ClassIn(userInfo.name, userInfo.mapId);
                        gameObject.SetActive(false);
                    }
                    else if (userInfo.mapType == DataManager.MapType.Square.ToString())
                    {
                        SceneMgr.instance.SquareIn();
                        gameObject.SetActive(false);
                    }
                    else if (userInfo.mapType == DataManager.MapType.Quiz.ToString())
                    {
                        SceneMgr.instance.QuizSquareIn();
                        gameObject.SetActive(false);
                    }
                    else if (userInfo.mapType == DataManager.MapType.QuizSquare.ToString())
                    {
                        SceneMgr.instance.QuizSquareIn();
                        gameObject.SetActive(false);
                    }
                    else if (userInfo.mapType == DataManager.MapType.ContestClassroom.ToString())
                    {
                        ToastMessage.OnMessage("따라갈 수 없는 위치에 있습니다");
                    }
                    else
                    {
                        ToastMessage.OnMessage("따라갈 수 없는 위치에 있습니다");
                    }
                }
                else
                {
                    SceneMgr.instance.ClassIn(userInfo.name, userInfo.mapId);
                    gameObject.SetActive(false);
                }
            };
            StartCoroutine(HttpManager.GetInstance().Get(info));
        }
    }
}