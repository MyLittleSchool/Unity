using GH;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json;
using Photon.Pun;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Networking;
using UnityEngine.UI;
using static SW.PlaceManager;
using static UnityEngine.Rendering.DebugUI;
using Button = UnityEngine.UI.Button;
namespace SW
{
    public class Guestbook : MonoBehaviour
    {
        [Header("방명록")]
        public Button closeButton;
        public Button createPanel_onButton;
        public List<RectTransform> contentsRaws = new List<RectTransform>();
        public RectTransform contents2;
        public GameObject contentPrefab;

        [Header("탭")]
        public Button[] tabButtons;
        public GameObject[] tabPanels;


        [Header("방명록 글쓰기")]
        public RectTransform createPanel;
        public Image createPanelImage;
        public Button createPanel_closeButton;
        public Button[] colorButtons;
        public Button saveButton;
        public TMP_InputField inputField;
        private Color selectedColor = Color.white;
        private int selectedColorIdx = 0;

        [Header("방명록 삭제")]
        public RectTransform deletePanel;
        public Button cancelDeleteButton;
        public Button confirmDeleteButton;
        private ContentData deleteSelected;

        // 방명록 리스트
        private List<ContentData> contentsList = new List<ContentData>();
        // 쪽지함 리스트
        private List<ContentData> notesList = new List<ContentData>();
        private void Start()
        {
            closeButton.onClick.AddListener(() => { OffPanel(); });
            createPanel_onButton.onClick.AddListener(() => { SetActiveCreatePanel(true); });
            createPanel_closeButton.onClick.AddListener(() => { SetActiveCreatePanel(false); });
            foreach (Button button in colorButtons)
            {
                button.onClick.AddListener(() => { SetColor(button); });
            }
            cancelDeleteButton.onClick.AddListener(() => { OnCancelButtonClick(); });
            confirmDeleteButton.onClick.AddListener(() => { OnConfirmButtonClick(); });
            for (int i = 0; i < tabButtons.Length; i++)
            {
                int idx = i;
                tabButtons[i].onClick.AddListener(() => ChangeTab(idx));
            }
        }
        private void OffPanel()
        {
            gameObject.SetActive(false);
            createPanel.gameObject.SetActive(false);
        }
        private void SetActiveCreatePanel(bool value)
        {
            createPanel.gameObject.SetActive(value);
        }
        private void SetColor(Button _button)
        {
            for (int i = 0; i < colorButtons.Length; i++)
            {
                if (colorButtons[i] == _button)
                {
                    colorButtons[i].transform.GetChild(0).gameObject.SetActive(true);
                    selectedColorIdx = i;
                }
                else colorButtons[i].transform.GetChild(0).gameObject.SetActive(false);
            }
            Color buttonColor = _button.GetComponent<Image>().color;
            if (buttonColor == Color.white) createPanelImage.color = new Color(0.9529412f, 0.9607844f, 0.9764706f);
            else createPanelImage.color = buttonColor;
            selectedColor = buttonColor;
        }
        public void OnValueChanged(string value)
        {
            if (value.Length == 0) saveButton.interactable = false;
            else saveButton.interactable = true;
        }
        // 방명록 생성
        public void OnSaveButtonClick()
        {
            HttpManager httpManager = HttpManager.GetInstance();
            PostInfo postInfo = new PostInfo();
            postInfo.content = inputField.text;
            postInfo.backgroundColor = selectedColorIdx;
            HttpManager.HttpInfo info = new HttpManager.HttpInfo();
            info.url = httpManager.SERVER_ADRESS + "/guest-book";
            info.body = JsonConvert.SerializeObject(postInfo, new JsonSerializerSettings { Converters = { new StringEnumConverter() } });
            info.contentType = "application/json";
            info.onComplete = (DownloadHandler res) =>
            {
                ToastMessage.OnMessage("작성을 완료하였습니다");
                LoadGuestbookData();
            };
            StartCoroutine(httpManager.Post(info));

            //// 생성
            //ContentData newContent = new ContentData();
            //newContent.nickname = "이규현";
            //newContent.content = inputField.text;
            //newContent.rgb = ColorToHexRGB(selectedColor);
            //newContent.registDate = DateTime.Now.ToString("O");
            //contentsList.Insert(0, newContent);
            //StartCoroutine(RefreshList());
            // 닫기
            inputField.text = "";
            SetActiveCreatePanel(false);
        }
        // 불러오기 통신
        public void LoadGuestbookData()
        {
            int mapId = DataManager.instance.mapId;
            DataManager.MapType mapType = DataManager.instance.MapTypeState;
            bool isMyClassroom = false;
            print(mapId);
            print(mapType);
            // 쪽지함은 내 교실일 때
            if (mapId == AuthManager.GetInstance().userAuthData.userInfo.id && mapType == DataManager.MapType.MyClassroom)
            {
                tabButtons[1].gameObject.SetActive(true);
                isMyClassroom = true;
            }
            else
            {
                tabButtons[1].gameObject.SetActive(false);
                ChangeTab(0);
            }
            HttpManager.HttpInfo info = new HttpManager.HttpInfo();
            info.url = HttpManager.GetInstance().SERVER_ADRESS + "/guest-book/list/" + mapType.ToString() + "/" + mapId;
            info.onComplete = (DownloadHandler res) =>
            {
                PostList list = JsonUtility.FromJson<PostList>("{\"data\" : " + res.text + "}");
                contentsList = new List<ContentData>();
                foreach (PostInfo post in list.data)
                {
                    ContentData newContent = new ContentData();
                    newContent.id = post.id;
                    newContent.userId = post.user.id;
                    newContent.nickname = post.user.nickname;
                    newContent.content = post.content;
                    switch (post.backgroundColor)
                    {
                        case 0:
                            newContent.rgb = "#FFFFFF";
                            break;
                        case 1:
                            newContent.rgb = "#D7ED7F";
                            break;
                        case 2:
                            newContent.rgb = "#BAEBD9";
                            break;
                        case 3:
                            newContent.rgb = "#BADEFF";
                            break;
                        case 4:
                            newContent.rgb = "#C6C5FF";
                            break;
                    }
                    //newContent.registDate = 
                    contentsList.Insert(0, newContent);
                }
                if (isMyClassroom)
                {
                    HttpManager.HttpInfo info2 = new HttpManager.HttpInfo();
                    info2.url = HttpManager.GetInstance().SERVER_ADRESS + "/guest-book/list/" + DataManager.MapType.Note.ToString() + "/" + mapId;
                    info2.onComplete = (DownloadHandler res2) =>
                    {
                        PostList list = JsonUtility.FromJson<PostList>("{\"data\" : " + res2.text + "}");
                        notesList = new List<ContentData>();
                        foreach (PostInfo post in list.data)
                        {
                            ContentData newContent = new ContentData();
                            newContent.id = post.id;
                            newContent.userId = post.user.id;
                            newContent.nickname = post.user.nickname;
                            newContent.content = post.content;
                            switch (post.backgroundColor)
                            {
                                case 0:
                                    newContent.rgb = "#FFFFFF";
                                    break;
                                case 1:
                                    newContent.rgb = "#D7ED7F";
                                    break;
                                case 2:
                                    newContent.rgb = "#BAEBD9";
                                    break;
                                case 3:
                                    newContent.rgb = "#BADEFF";
                                    break;
                                case 4:
                                    newContent.rgb = "#C6C5FF";
                                    break;
                            }
                            notesList.Insert(0, newContent);
                        }
                    StartCoroutine(RefreshList());
                    };
                    StartCoroutine(HttpManager.GetInstance().Get(info2));
                }
                else StartCoroutine(RefreshList());
            };
            StartCoroutine(HttpManager.GetInstance().Get(info));
        }
        IEnumerator RefreshList()
        {
            // 쪽지함
            // 삭제
            for (int i = 0; i < contents2.childCount; i++)
            {
                Destroy(contents2.GetChild(i).gameObject);
            }
            // 추가
            for (int i = 0; i < notesList.Count; i++)
            {
                CreateNote(notesList[i]);
            }
            // 방명록
            // 삭제
            foreach (RectTransform raw in contentsRaws)
            {
                for (int i = raw.childCount - 1; i >= 0; i--)
                {
                    Destroy(raw.GetChild(i).gameObject);
                }
            }
            yield return null;
            // 추가
            for (int i = 0; i < contentsList.Count; i++)
            {
                CreateContent(contentsList[i]);
                yield return null;
            }
        }
        // 방명록
        private void CreateContent(ContentData contentData)
        {
            RectTransform raw = contentsRaws.OrderBy(rt => rt.rect.height).FirstOrDefault();
            GameObject newContent = Instantiate(contentPrefab, raw);
            // 삭제 버튼
            Button delBtn = newContent.transform.GetChild(1).GetChild(0).GetComponent<Button>();
            // 작성자 이거나
            if (contentData.userId == AuthManager.GetInstance().userAuthData.userInfo.id || 
            // 교실의 주인일 때
                (DataManager.instance.MapTypeState == DataManager.MapType.MyClassroom && DataManager.instance.mapId == AuthManager.GetInstance().userAuthData.userInfo.id))
            // 삭제버튼 On
            {
                delBtn.onClick.AddListener(() =>
                {
                    deleteSelected = contentData;
                    deletePanel.gameObject.SetActive(true);
                });
            }
            else
            {
                delBtn.gameObject.SetActive(false);
            }
            newContent.GetComponent<Image>().color = HexToColor(contentData.rgb);
            TMP_Text contentText = newContent.transform.GetChild(0).GetComponent<TMP_Text>();
            contentText.text = contentData.content;
            TMP_Text infoText = newContent.transform.GetChild(1).GetComponent<TMP_Text>();
            infoText.text = "<b>" + contentData.nickname + "</b> <color=#A4A4A4>";// + GetElapsedTime(contentData.registDate) + "</color>";
            LayoutRebuilder.ForceRebuildLayoutImmediate(contentText.rectTransform);
            LayoutRebuilder.ForceRebuildLayoutImmediate(raw);
        }
        // 쪽지함
        private void CreateNote(ContentData contentData)
        {
            GameObject newContent = Instantiate(contentPrefab, contents2.transform);
            // 삭제 버튼
            Button delBtn = newContent.transform.GetChild(1).GetChild(0).GetComponent<Button>();
            delBtn.onClick.AddListener(() =>
            {
                deleteSelected = contentData;
                deletePanel.gameObject.SetActive(true);
            });
            newContent.GetComponent<Image>().color = HexToColor(contentData.rgb);
            TMP_Text contentText = newContent.transform.GetChild(0).GetComponent<TMP_Text>();
            contentText.text = contentData.content;
            TMP_Text infoText = newContent.transform.GetChild(1).GetComponent<TMP_Text>();
            infoText.text = "<b>" + contentData.nickname + "</b> <color=#A4A4A4>";// + GetElapsedTime(contentData.registDate) + "</color>";
            LayoutRebuilder.ForceRebuildLayoutImmediate(contentText.rectTransform);
            LayoutRebuilder.ForceRebuildLayoutImmediate(contents2);
        }

        private string ColorToHexRGB(Color color)
        {
            int r = Mathf.RoundToInt(color.r * 255);
            int g = Mathf.RoundToInt(color.g * 255);
            int b = Mathf.RoundToInt(color.b * 255);

            return $"#{r:X2}{g:X2}{b:X2}";
        }
        private Color HexToColor(string hex)
        {
            // '#' 문자가 있으면 제거
            hex = hex.Replace("#", "");

            // 16진수 문자열을 각각 RGB로 변환
            byte r = byte.Parse(hex.Substring(0, 2), System.Globalization.NumberStyles.HexNumber);
            byte g = byte.Parse(hex.Substring(2, 2), System.Globalization.NumberStyles.HexNumber);
            byte b = byte.Parse(hex.Substring(4, 2), System.Globalization.NumberStyles.HexNumber);

            // 0~255 범위의 값을 0~1로 변환해서 Color로 반환 (알파는 1로 설정)
            return new Color(r / 255f, g / 255f, b / 255f, 1f);
        }
        private string GetElapsedTime(string time)
        {
            DateTime registDate = DateTime.Parse(time);

            // 현재 시간과 비교
            DateTime currentTime = DateTime.Now;
            TimeSpan timeDifference = currentTime - registDate;

            // 경과 시간에 따라 다른 포맷으로 출력
            string timeAgo;
            if (timeDifference.TotalMinutes < 1)
            {
                timeAgo = "0m";
            }
            else if (timeDifference.TotalMinutes < 60)
            {
                timeAgo = $"{Math.Floor(timeDifference.TotalMinutes)}m";  // 분
            }
            else if (timeDifference.TotalHours < 24)
            {
                timeAgo = $"{Math.Floor(timeDifference.TotalHours)}h";  // 시간
            }
            else
            {
                timeAgo = $"{Math.Floor(timeDifference.TotalDays)}d";  // 일
            }

            return timeAgo;
        }
        public void OnDeleteButtonClick(ContentData contentData)
        {
            deleteSelected = contentData;
            deletePanel.gameObject.SetActive(true);
        }
        public void OnCancelButtonClick()
        {
            deleteSelected = null;
            deletePanel.gameObject.SetActive(false);
        }
        public void OnConfirmButtonClick()
        {
            contentsList.Remove(deleteSelected);
            StartCoroutine(RefreshList());

            HttpManager.HttpInfo info = new HttpManager.HttpInfo();
            info.url = HttpManager.GetInstance().SERVER_ADRESS + "/guest-book?guestBookId=" + deleteSelected.id;
            info.onComplete = (DownloadHandler res) =>
            {
                LoadGuestbookData();
            };
            StartCoroutine(HttpManager.GetInstance().Delete(info));
            deleteSelected = null;
            deletePanel.gameObject.SetActive(false);
        }
        private int tab;
        public void ChangeTab(int num)
        {
            tab = num;
            for (int i = 0; i < tabButtons.Length; i++)
            {
                if (i == num)
                {
                    tabPanels[i].gameObject.SetActive(true);
                    tabButtons[i].transform.GetChild(0).gameObject.SetActive(true);
                }
                else
                {
                    tabPanels[i].gameObject.SetActive(false);
                    tabButtons[i].transform.GetChild(0).gameObject.SetActive(false);
                }
            }
        }
    }
    [Serializable]
    public class ContentData
    {
        public int id;
        public int userId;
        public string nickname;
        public string content;
        public string rgb;
        public string registDate;
        public int likeCount;
    }
    [Serializable]
    public class PostInfo
    {
        public int id;
        public string content;
        public UserInfo user;
        public int mapId;
        public DataManager.MapType mapType;
        public int backgroundColor;
        public PostInfo()
        {
            user = AuthManager.GetInstance().userAuthData.userInfo;
            mapId = DataManager.instance.mapId;
            mapType = DataManager.instance.MapTypeState;
        }
    }
    [Serializable]
    public class PostList
    {
        public List<PostInfo> data;
    }
}