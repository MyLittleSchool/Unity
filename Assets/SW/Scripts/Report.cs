using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static HttpManager;
using UnityEngine.Networking;
namespace SW
{
    public class Report : MonoBehaviour
    {
        public static Report instance;
        private void Start()
        {
            instance = this;
        }
        public ReportPanel reportPanel;
        private ReportInfo reportInfo;
        [Serializable]
        private struct ReportInfo
        {
            public int reporterId;
            public int reportedUserId;
            public string contentType;
            public int contentId;
            public string reason;
            public ReportInfo(int reportedUserId, string contentType, int contentId)
            {
                reporterId = AuthManager.GetInstance().userAuthData.userInfo.id;
                this.reportedUserId = reportedUserId;
                this.contentType = contentType;
                this.contentId = contentId;
                reason = "";
            }
        }
        public enum ContentType
        {
            User, Chat, Board, Comment, Guestbook, Note, MapContest, Gallery
        }
        // 유저O, 채팅, 게시판O, 댓글O, 방명록O, 쪽지O, 맵콘테스트O, 아카이빙갤러리O
        public void CreateReportInfo(string targetText, ContentType contentType, int reportedUserId = -1, int contentId = -1)
        {
            reportInfo = new ReportInfo(reportedUserId, contentType.ToString(), contentId);
            reportPanel.SetActivePanel(true);
            reportPanel.SetInfo(targetText);
        }
        public void ConfirmReport(string reason)
        {
            reportInfo.reason = reason;
            HttpInfo info = new HttpInfo();
            info.url = HttpManager.GetInstance().SERVER_ADRESS + "/report";
            info.body = JsonUtility.ToJson(reportInfo);
            info.contentType = "application/json";
            info.onComplete = (DownloadHandler downloadHandler) =>
            {
                ToastMessage.OnMessage("신고가 접수되었습니다.");
                reportPanel.SetActivePanel(false);
            };
            StartCoroutine(HttpManager.GetInstance().Post(info));
        }
    }
}