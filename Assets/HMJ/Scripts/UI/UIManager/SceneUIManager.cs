using GH;
using Photon.Pun;
using SW;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem.EnhancedTouch;
using UnityEngine.Networking;
using UnityEngine.UI;
using static HttpManager;

namespace MJ
{
    public class SceneUIManager : MonoBehaviour
    {
        #region Button
        [Header("닫기 버튼")]
        public UnityEngine.UI.Button MapRegisterCloseButton;
        public UnityEngine.UI.Button MapContestCloseButton;

        [Header("아바타 꾸미기 On 버튼")]
        public Button avatarDecoOpenButton;

        [Header("아바타 꾸미기 Off 버튼")]
        public Button avatarDecoOffButton;

        [Header("꾸미기 패널 - Skin, Face, Hair, Cloth")]
        public UnityEngine.UI.Button[] decorationEnumButton;

        [Header("꾸미기 패널 - 선택창(0, 1, 2, 3)")]
        public UnityEngine.UI.Button[] decorationChoiceButton;

        [Header("맵 등록 패널 - 맵 등록 패널 버튼")]
        public UnityEngine.UI.Button mapRegisterButton;

        [Header("맵 등록 패널 - 맵 콘테스트 패널 버튼")]
        public UnityEngine.UI.Button mapContestButton;

        [Header("맵 등록 패널 - 맵 등록 여부 패널 버튼")]
        public UnityEngine.UI.Button MapConfirmYesButton;
        public UnityEngine.UI.Button MapConfirmNoButton;

        [Header("맵 등록 성공 패널 끄기")]
        public UnityEngine.UI.Button MapRegisterSuccessCloseButton;

        [Header("맵 인벤토리 패널 - 맵 인벤토리 패널 버튼")]
        public UnityEngine.UI.Button InventoryButton;

        [Header("맵 인벤토리 패널 - 맵 인벤토리 패널 끄기 버튼")]
        public UnityEngine.UI.Button InventoryCloseButton;

        [Header("맵 인벤토리 에러 패널 - 맵 인벤토리 에러 패널 끄기 버튼")]
        public UnityEngine.UI.Button InventoryErrorCloseButton;

        [Header("메뉴 버튼")]
        public UnityEngine.UI.Button topMenuButton;

        [Header("친구창 버튼")]
        public UnityEngine.UI.Button friendsButton;

        [Header("나의 프로필 버튼")]
        public Button myProfileButton;

        [Header("나의 프로필 편집 버튼")]
        public Button myProfileEditButton;

        [Header("나의 프로필 편집 저장 버튼")]
        public Button myProfileSaveButton;

        [Header("첫 로그인 학생 선택 버튼")]
        public Button studentCheckButton;

        [Header("첫 로그인 학생 아님 선택 버튼")]
        public Button noStudentCheckButton;

        [Header("학교 선택 완료 버튼")]
        public Button schoolSaveButton;

        #endregion

        #region Panel
        [Header("꾸미기 패널")]
        public GameObject DecorationPanel;

        [Header("플레이어 꾸미기 오브젝트")]
        public GameObject PlayerObject;

        [Header("방명록 패널")]
        public GameObject guestbookPanel;

        [Header("게시판 패널")]
        public GameObject boardPanel;

        [Header("맵 등록 패널")]
        public GameObject mapRegisterPanel;

        [Header("맵 등록 여부 패널")]
        public GameObject mapConfirmPanel;

        [Header("맵 콘테스트 패널")]
        public GameObject mapContestPanel;

        [Header("맵 등록 성공 패널")]
        public GameObject mapSuccessRegisterPanel;

        [Header("맵 인벤토리 패널")]
        public GameObject mapInventoryPanel;

        [Header("맵 인벤토리 에러 패널 - 아이템 개수 부족")]
        public GameObject mapInventoryErrorPanel;

        [Header("메뉴 패널")]
        public GameObject menuPanel;

        [Header("친구창 패널")]
        public GameObject friendsPanel;

        [Header("채팅 패널")]
        public GameObject ChatPanel;

        [Header("나의 프로필 패널")]
        public GameObject myProfilePanel;

        [Header("나의 프로필 편집 패널")]
        public GameObject myProfileEditPanel;

        [Header("다른 플레이어 프로필 패널")]
        public GameObject othersProfilePanel;

        [Header("플레이어 목록 패널")]
        public GameObject playerList;

        [Header("첫 로그인 학생 선택 패널")]
        public GameObject firstLoginPanel;

        [Header("첫 로그인 학교 선택 패널")]
        public GameObject firstSchoolPanel;

        [Header("학교 방문 패널")]
        public GameObject visitOtherSchoolPanel;
        [Header("프라이빗 룸 패널")]
        public GameObject privateRoomPanel;

        [Header("보이스 패널")]
        public GameObject voicePanel;
        #endregion

        #region SingleTone
        private static SceneUIManager instance;
        public static SceneUIManager GetInstance()
        {
            if (instance == null)
            {
                GameObject go = new GameObject();
                go.name = "SceneUIManager";
                go.AddComponent<SceneUIManager>();
            }
            return instance;
        }

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
        #endregion

        #region 규현
        [Header("프로필 편집 인풋 박스들")]
        public TMP_InputField nickNameInputField;
        public TMP_InputField interestInputField;
        public TMP_InputField myMessageInputField;

        [Header("프로필 편집 텍스트")]
        public TMP_Text nickNameText;
        public TMP_Text interestText;
        public TMP_Text myMessageText;

        [Header("관심사 딕셔너리")]
        private Dictionary<string, GameObject> buttonList = new Dictionary<string, GameObject>();

        [Header("관심사 버튼 프리팹")]
        public GameObject interestButtonPrefab;

        [Header("관심사 버튼 생성위치")]
        public RectTransform interestButtonTransform;

        [Header("선택된 관심사 리스트")]
        public List<string> selectedInterest;

        private Color32 selectColor = new Color32(242, 136, 75, 255);
        private Color32 noneSelectColor = new Color32(242, 242, 242, 255);

        public TMP_Text profileLvNick;
        public TMP_Text profileInterest;
        public TMP_Text profileMyMessage;

        public UserInfo currentuserInfo;

        [Header("학교 검색 인풋필드")]
        public TMP_InputField schoolSuch;

        [Header("학교 통신 겟 리스트")]
        public SchoolData schooldata;

        [Header("학교 통신 드롭다운")]
        public TMP_Dropdown schoolDropDown;

        [Header("학년 드롭다운")]
        public TMP_Dropdown schoolGradeDropDown;

        public List<string> schoolName;
        #endregion

        private void Start()
        {
            myProfileButton.onClick.AddListener(OnOffMyProfile);
            myProfileEditButton.onClick.AddListener(OnMyProfileEdit);
            myProfileSaveButton.onClick.AddListener(OffMyProfileEdit);
            studentCheckButton.onClick.AddListener(ClickStudent);
            noStudentCheckButton.onClick.AddListener(ClickNoStudent);
            schoolSaveButton.onClick.AddListener(SchoolSave);

            firstLoginPanel.SetActive(false);
            firstSchoolPanel.SetActive(false);
            myProfileEditPanel.SetActive(false);
            myProfilePanel.SetActive(false);

            if (mapRegisterButton)
            {
                mapRegisterButton.onClick.AddListener(OnMapRegisterPanel);
            }
            if (MapRegisterCloseButton)
            {
                MapRegisterCloseButton.onClick.AddListener(CloseMapRegisterPanel);
                MapRegisterCloseButton.onClick.AddListener(OnMapContestPanel);
            }

            if (MapContestCloseButton)
                MapContestCloseButton.onClick.AddListener(CloseMapContestPanel);

            // StartCoroutine(coMapInventoryGameObject());
            InventoryButton.onClick.AddListener(OnMapInventoryPanel);
            InventoryCloseButton.onClick.AddListener(CloseMapInventoryPanel);

            if (mapContestButton)
                mapContestButton.onClick.AddListener(OnMapConfirmPanel);

            if (MapConfirmYesButton)
                MapConfirmYesButton.onClick.AddListener(OnMapRegisterPanel);
            if (MapConfirmYesButton)
                MapConfirmYesButton.onClick.AddListener(OffMapConfirmPanel);
            if (MapConfirmNoButton)
                MapConfirmNoButton.onClick.AddListener(OffMapConfirmPanel);

            if (MapRegisterSuccessCloseButton)
                MapRegisterSuccessCloseButton.onClick.AddListener(OffMapSuccessRegisterPanel);

            if (InventoryErrorCloseButton)
                InventoryErrorCloseButton.onClick.AddListener(OffMapInventoryErrorPanel);

            if (avatarDecoOpenButton)
                avatarDecoOpenButton.onClick.AddListener(OnDecorationPanel);

            if (avatarDecoOffButton)
                avatarDecoOffButton.onClick.AddListener(OffDecorationPanel);


            if (AuthManager.GetInstance().userAuthData.userInfo.school.schoolName == "")
            {
                RegisterAvatar();

                firstLoginPanel.SetActive(true);
            }
            else
                GetAvatar();

            InterestButtonCreate();

            SetProfile();
            initDecorationPanel();
            InitOtherPlayerPanel();
            initDecorationPanel();
        }
        private void Update()
        {
            //ProfileEditCount();

            if (firstSchoolPanel.activeSelf)
            {
                if (Input.GetKeyDown(KeyCode.Return))
                {
                    print("엔터");
                    SchoolGet();
                }
                schoolDropDown.onValueChanged.AddListener(delegate { SetSchoolName(schoolDropDown.value); });

            }
            TouchPlayer();
        }
        public void RegisterAvatar()
        {
            PlayerAnimation.GetInstance().PostAvatarData();
        }

        public void GetAvatar()
        {
            PlayerAnimation.GetInstance().GetAvatarData();
        }

        public void initDecorationPanel()
        {
            avatarDecoOpenButton.onClick.AddListener(OnDecorationPanel);
            avatarDecoOffButton.onClick.AddListener(OffDecorationPanel);

            PlayerDecoration DecorationDT = PlayerDecoration.GetInstance();
            DecorationDT.LoadDecorationData();

            PlayerAnimation AnimationDT = PlayerAnimation.GetInstance();


            for (int i = 0; i < decorationEnumButton.Length; i++)
            {
                int data = i;
                decorationEnumButton[i].onClick.AddListener(() => DecorationDT.SetPlayerDecorationData((DecorationEnum.DECORATION_DATA)data));
            }

            for (int i = 0; i < decorationChoiceButton.Length; i++)
            {
                int data = i;
                decorationChoiceButton[i].onClick.AddListener(() =>
                {
                    if (!DecorationDT.SetPlayerSelectDecorationData(DecorationDT.CurDecorationPanel, data))
                        return;
                    AnimationDT.SetDecorationAnimData(DecorationDT.CurDecorationPanel, data);
                });
            }

            // OffDecorationPanel();
        }
        public void OnGuestbookPanel()
        {
            guestbookPanel.SetActive(true);
            guestbookPanel.GetComponent<Guestbook>().LoadGuestbookData();
        }
        public void OnBoardPanel()
        {
            boardPanel.SetActive(true);
            boardPanel.GetComponent<Board>().LoadBoardData();
        }
        public void OnMapRegisterPanel()
        {
            mapRegisterPanel.SetActive(true);
            mapContestPanel.SetActive(false);
        }

        public void OnMapContestPanel()
        {
            mapRegisterPanel.SetActive(false);
            mapContestPanel.SetActive(true);
            mapContestButton.gameObject.SetActive(false);
        }

        IEnumerator coMapInventoryGameObject()
        {
            yield return new WaitUntil(() => mapInventoryPanel != null);
        }

        public void OnMapInventoryPanel()
        {
            mapInventoryPanel.SetActive(true);
            InventoryCloseButton.gameObject.SetActive(true);
            InventoryButton.gameObject.SetActive(false);
            ChatPanel.gameObject.SetActive(false);

            if (DataManager.instance.player != null)
            {
                DataManager.instance.player.GetComponent<SetTile>().setMode = true;
            }
        }

        public void CloseMapRegisterPanel()
        {
            mapRegisterPanel.SetActive(false);
        }

        public void CloseMapContestPanel()
        {
            mapContestPanel.SetActive(false);
            mapContestButton.gameObject.SetActive(true);
        }

        public void CloseMapInventoryPanel()
        {
            mapInventoryPanel.SetActive(false);
            InventoryCloseButton.gameObject.SetActive(false);
            InventoryButton.gameObject.SetActive(true);
            ChatPanel.gameObject.SetActive(true);
            if (DataManager.instance.player != null)
            {
                DataManager.instance.player.GetComponent<SetTile>().setMode = false;
            }
        }

        public void OnMapConfirmPanel()
        {
            mapConfirmPanel.SetActive(true);
        }

        public void OffMapConfirmPanel()
        {
            mapConfirmPanel.SetActive(false);
        }

        public void OnMapSuccessRegisterPanel()
        {
            // mapSuccessRegisterPanel.SetActive(true);
        }

        public void OffMapSuccessRegisterPanel()
        {
            mapSuccessRegisterPanel.SetActive(false);
        }

        public void OnMenuButtonClick()
        {
            if (menuPanel.activeSelf)
            {
                iTween.Stop(menuPanel);
                menuPanel.transform.position = new Vector3(267, menuPanel.transform.position.y, 0);
                iTween.MoveTo(menuPanel, iTween.Hash(
                    "x", 0,
                    "time", 0.6f,
                    "easetype", iTween.EaseType.easeOutBounce,
                    "oncomplete", nameof(SetMenuPanel),
                    "oncompletetarget", gameObject,
                    "oncompleteparams", false
                ));
            }
            else
            {
                iTween.Stop(menuPanel);
                menuPanel.transform.position = new Vector3(0, menuPanel.transform.position.y, 0);
                SetMenuPanel(true);
                iTween.MoveTo(menuPanel, iTween.Hash(
                    "x", 267,
                    "time", 0.6f,
                    "easetype", iTween.EaseType.easeOutBounce
                ));
            }
        }
        public void SetMenuPanel(bool value)
        {
            menuPanel.SetActive(value);
        }

        public void OnFriendsPanel()
        {
            friendsPanel.SetActive(true);
            friendsPanel.GetComponent<FriendsUI>().RefreshFriends();
        }

        public void OffAllMapPanel()
        {
            if (mapContestPanel)
                mapContestPanel.SetActive(false);
            if (mapRegisterPanel)
                mapRegisterPanel.SetActive(false);
            if (mapConfirmPanel)
                mapConfirmPanel.SetActive(false);
            if (mapSuccessRegisterPanel)
                mapSuccessRegisterPanel.SetActive(false);
        }

        public void OnOffMyProfile()
        {
            //버튼으로 키고 끄기
            myProfilePanel.SetActive(!myProfilePanel.activeSelf);

            Image myProfileImage = myProfileButton.GetComponentInChildren<Image>();
            Color32 myprofileColor = myProfilePanel.activeSelf ? new Color32(242, 136, 75, 255) : new Color32(29, 27, 32, 255);
            myProfileImage.color = myprofileColor;

        }

        public void OnDecorationPanel()
        {
            DecorationPanel.SetActive(true);
        }

        public void OffDecorationPanel()
        {
            PlayerAnimation.GetInstance().PatchAvatarData();
            DecorationPanel.SetActive(false);
        }
        

        public void OffMapInventoryErrorPanel()
        {
            mapInventoryErrorPanel.SetActive(false);
        }

        public void OnMapInventoryErrorPanel()
        {
            mapInventoryErrorPanel.SetActive(true);
        }

        public void OnMyProfileEdit()
        {
            myProfileEditPanel.SetActive(true);
            myProfilePanel.SetActive(false);
        }

        public void OffMyProfileEdit()
        {
            myProfileEditPanel.SetActive(false);
            myProfilePanel.SetActive(true);
            ProfileEditSave();
        }
        public void OnVisitOtherSchoolPanel()
        {
            visitOtherSchoolPanel.SetActive(true);
        }

        public void OnVoicePanel()
        {
            voicePanel.SetActive(true);
        }

        public void OffVoicePanel()
        {
            voicePanel.SetActive(false);
        }

        public void ProfileEditCount()
        {
            nickNameText.text = nickNameInputField.text.Length + "/10";
            interestText.text = selectedInterest.Count + "/5";
            myMessageText.text = myMessageInputField.text.Length + "/30";

            if (interestInputField.isFocused)
            {
                interestButtonTransform.gameObject.SetActive(true);
            }
            if (Input.touchCount == 1 || Input.GetMouseButtonDown(0))
            {
                Vector2 localPointPos = interestButtonTransform.InverseTransformPoint(Input.mousePosition);
                if (!interestButtonTransform.rect.Contains(localPointPos))
                {
                    interestButtonTransform.gameObject.SetActive(false);
                }
            }
        }
        public void InterestButtonOnOff()
        {
            bool onInterest = false;
            onInterest = interestButtonTransform.gameObject.activeSelf ? false : true;
            interestButtonTransform.gameObject.SetActive(onInterest);
        }

        private void InterestButtonCreate()
        {
            for (int i = 0; i < DataManager.instance.interests.Count; i++)
            {
                GameObject interestButton = Instantiate(interestButtonPrefab, interestButtonTransform);
                interestButton.GetComponentInChildren<TMP_Text>().text = DataManager.instance.interests[i];
                buttonList.Add(DataManager.instance.interests[i], interestButton);
            }
        }

        public void InterestSlect(string key, Image image)
        {
            bool test = false;
            interestInputField.text = "";

            if (selectedInterest.Count > 0)
            {

                // 중복 체크
                for (int i = 0; i < selectedInterest.Count; i++)
                {
                    if (selectedInterest[i] == key)
                    {
                        test = true;
                        print("11");

                        image.color = noneSelectColor;
                        selectedInterest.RemoveAt(i);
                        break;
                    }
                    else
                    {
                        test = false;
                        print("22");

                    }
                    // interestText.text += "#" + selectedInterest[i] + " ";
                }
                if (!test)
                {
                    image.color = selectColor;

                    if (selectedInterest.Count < 5)
                    {
                        selectedInterest.Add(key);
                    }
                    else
                    {
                        buttonList[selectedInterest[0]].GetComponent<Image>().color = noneSelectColor;
                        selectedInterest.RemoveAt(0);
                        selectedInterest.Add(key);
                    }
                }

                for (int i = 0; i < selectedInterest.Count; i++)
                {
                    interestInputField.text += "#" + selectedInterest[i] + " ";

                }
            }
            else
            {
                image.color = new Color32(242, 136, 75, 255);
                selectedInterest.Add(key);
                interestInputField.text += "#" + key + " ";
            }

        }

        private void ProfileEditSave()
        {
            UserInfo joinInfo = AuthManager.GetInstance().userAuthData.userInfo;
            joinInfo.name = nickNameInputField.text;
            joinInfo.interest = selectedInterest;
            joinInfo.statusMesasge = myMessageInputField.text;
            joinInfo.schoolId = 1;

            HttpInfo info = new HttpInfo();
            info.url = HttpManager.GetInstance().SERVER_ADRESS + "/user/profile";
            info.body = JsonUtility.ToJson(joinInfo);
            info.contentType = "application/json";
            info.onComplete = (DownloadHandler downloadHandler) =>
            {
                print(downloadHandler.text);
            };
            StartCoroutine(HttpManager.GetInstance().Patch(info));

            currentuserInfo = AuthManager.GetInstance().userAuthData.userInfo;
            currentuserInfo.name = nickNameInputField.text;
            currentuserInfo.interest = selectedInterest;
            currentuserInfo.statusMesasge = myMessageInputField.text;

            AuthManager.GetInstance().userAuthData = new AuthManager.AuthData(currentuserInfo);
            SetProfile();
        }

        private void SetProfile()
        {
            profileInterest.text = "";
            profileMyMessage.text = "";
            UserInfo userInfo = AuthManager.GetInstance().userAuthData.userInfo;
            profileLvNick.text = userInfo.level + " | " + userInfo.name;
            for (int i = 0; i < userInfo.interest.Count; i++)
            {
                profileInterest.text += "#" + userInfo.interest[i] + " ";
            }

            //메시지
            profileMyMessage.text = userInfo.statusMesasge;
        }

        //첫 로그인 핵생 선택 버튼
        private void ClickStudent()
        {
            firstLoginPanel.SetActive(false);
            firstSchoolPanel.SetActive(true);
        }
        private void ClickNoStudent()
        {
            firstLoginPanel.SetActive(false);

        }
        private void SchoolSave()
        {
            HttpInfo info = new HttpInfo();
            info.url = HttpManager.GetInstance().SERVER_ADRESS + "/school/add-user?schoolId="
                + schooldata.data[schoolDropDown.value].id + "&userId=" + AuthManager.GetInstance().userAuthData.userInfo.id
                + "&user_grade=" + schoolGradeDropDown.value;
            info.onComplete = (DownloadHandler downloadHandler) =>
            {
                print(downloadHandler.text);
            };
            StartCoroutine(HttpManager.GetInstance().Post(info));

            AuthManager.GetInstance().userAuthData.userInfo.school.schoolName = schooldata.data[schoolDropDown.value].schoolName;
            DataManager.instance.playerSchool = schooldata.data[schoolDropDown.value].schoolName;
            firstSchoolPanel.SetActive(false);

            StartCoroutine(CoUserGet());

        }

        private void SchoolGet()
        {
            schooldata.data.Clear();

            schoolName.Clear();

            HttpInfo info = new HttpInfo();
            info.url = HttpManager.GetInstance().SERVER_ADRESS + "/school/" + schoolSuch.text;
            info.onComplete = (DownloadHandler downloadHandler) =>
            {
                string jsonData = "{ \"data\" : " + downloadHandler.text + "}";
                print(jsonData);
                //jsonData를 PostInfoArray 형으로 바꾸자.
                schooldata = JsonUtility.FromJson<SchoolData>(jsonData);
            };
            StartCoroutine(HttpManager.GetInstance().Get(info));

            StartCoroutine(CoSchoolGet());


            print("학교 정보 불러오기");



        }
        private void SetSchoolName(int option)
        {
            schoolSuch.text = schoolDropDown.options[option].text;
        }
        IEnumerator CoSchoolGet()
        {
            yield return new WaitUntil(() => schooldata.data.Count > 0);

            for (int i = 0; i < schooldata.data.Count; i++)
            {
                schoolName.Add(schooldata.data[i].schoolName);
            }

            schoolDropDown.ClearOptions();
            schoolDropDown.AddOptions(schoolName);

            schoolDropDown.Show();


        }

        IEnumerator CoUserGet()
        {
            yield return new WaitForSeconds(0.5f);



            HttpInfo info2 = new HttpInfo();
            info2.url = HttpManager.GetInstance().SERVER_ADRESS + "/user/email/" + AuthManager.GetInstance().userAuthData.userInfo.email;
            Debug.Log("로그인 url: " + info2.url);
            info2.onComplete = (DownloadHandler downloadHandler) =>
            {
                string jsonData = "{ \"data\" : " + downloadHandler.text + "}";
                print(jsonData);
                //jsonData를 데이터에 다시 저장 바꾸자.
                AuthManager.GetInstance().userAuthData = new AuthManager.AuthData(JsonUtility.FromJson<UserInfoData>(jsonData).data);
            };
            StartCoroutine(HttpManager.GetInstance().Get(info2));
        }
        private void TouchPlayer()
        {
            // 터치 입력이 발생했을 때만 처리
            if (Input.touchCount > 0)
            {
                UnityEngine.Touch touch = Input.GetTouch(0);

                // 터치가 시작되었을 때 처리
                if (touch.phase == TouchPhase.Began)
                {
                    // UI가 터치를 가로챘는지 확인
                    if (IsPointerOverUIObject(touch.position))
                    {
                        return; // UI가 터치를 가로챘으므로 월드 오브젝트와의 상호작용을 중지
                    }
                    // UI가 아닌 2D 콜라이더 오브젝트 터치 처리
                    Vector2 touchPosition = Camera.main.ScreenToWorldPoint(touch.position);
                    HandleInteraction(touchPosition);
                }
            }
            else if (Input.GetMouseButtonDown(0))
            {
                // UI가 터치를 가로챘는지 확인
                if (IsPointerOverUIObject(Input.mousePosition))
                {
                    return; // UI가 터치를 가로챘으므로 월드 오브젝트와의 상호작용을 중지
                }
                Vector2 clickPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                HandleInteraction(clickPosition);
            }
        }
        private void HandleInteraction(Vector2 touchPosition)
        {
            RaycastHit2D hit = Physics2D.Raycast(touchPosition, Vector2.zero, Mathf.Infinity, 1 << LayerMask.NameToLayer("Player"));

            if (hit.collider != null)
            {
                //Debug.Log("2D 오브젝트를 터치했습니다: " + hit.collider.gameObject.name);
                // 터치한 오브젝트에 대한 처리 수행
                if (hit.collider.gameObject.GetComponent<PhotonView>().IsMine == false)
                {
                    othersProfilePanel.SetActive(true);
                    UserInfo userInfo = hit.collider.gameObject.GetComponent<UserRPC>().userInfo;
                    FriendPanel comp = othersProfilePanel.GetComponent<FriendPanel>();
                    comp.id = userInfo.id;
                    comp.NickNameText.text = userInfo.name;
                    comp.InterestText.text = "#" + String.Join(" #", userInfo.interest);
                    comp.MessageText.text = userInfo.statusMesasge;
                }
            }
            else othersProfilePanel.SetActive(false);
        }
        private bool IsPointerOverUIObject(Vector2 touchPosition)
        {
            // PointerEventData 생성 및 터치 위치 설정
            PointerEventData eventData = new PointerEventData(EventSystem.current) { position = touchPosition };

            // Raycast 결과를 저장할 리스트 생성
            var results = new List<RaycastResult>();

            // EventSystem을 통해 Raycast 수행
            EventSystem.current.RaycastAll(eventData, results);

            // Raycast 결과 검사하여 Default 레이어가 아닌 UI 요소만 확인
            foreach (var result in results)
            {
                if (result.gameObject.layer != LayerMask.NameToLayer("Default"))
                {
                    return true; // Default 레이어가 아닌 UI 요소가 있으면 true 반환
                }
            }

            // Default 레이어만 감지되었거나, UI가 없으면 false 반환
            return false;
        }
        private void InitOtherPlayerPanel()
        {
            FriendPanel comp = othersProfilePanel.GetComponent<FriendPanel>();
            comp.PassButton.onClick.AddListener(() =>
            {
                othersProfilePanel.SetActive(false);
            });
            comp.RequestButton.onClick.AddListener(() =>
            {
                WebSocketManager.GetInstance().RequestFriend(comp.id);
            });
        }
    }
}