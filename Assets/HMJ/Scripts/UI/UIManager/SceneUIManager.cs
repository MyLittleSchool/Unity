using GH;
using Photon.Pun;
using Photon.Voice.Unity.Demos;
using SW;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
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
        public UnityEngine.UI.Button onMapRegisterButton;

        [Header("맵 등록 패널 - 맵 콘테스트 업로드 버튼")]
        public UnityEngine.UI.Button contestUpload_Button;

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

        [Header("인벤토리 선택 버튼 - 공통")]
        public Button commonButton;

        [Header("인벤토리 선택 버튼 - 학교")]
        public Button myClassRoomButton;

        [Header("샵 닫기 버튼")]
        public Button shopCloseButton;

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

        [Header("아카이빙갤러리 패널")]
        public GameObject archivePanel;

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
        public RectTransform menuPanelRT;

        [Header("친구창 패널")]
        public GameObject friendsPanel;
        public GameObject requestFriendPanel;

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
        
        [Header("학교 방문 패널")]
        public GameObject visitNearSchoolPanel;


        [Header("프라이빗 룸 패널")]
        public GameObject privateRoomPanel;

        [Header("퀴즈 카테고리 패널")]
        public GameObject quizCategoryPanel;

        //[Header("퀴즈 문제 패널")]
        //public GameObject quizQuestionPanel;

        [Header("보이스 패널")]
        public GameObject voicePanel;

        [Header("맵 튜토리얼 패널")]
        public GameObject tutorialPanel;

        [Header("맵 콘테스트 계산 영수증 - 맵 콘테스트")]
        public GameObject mapContestBill;

        [Header("샵 패널")]
        public GameObject shopPanel;


        [Header("가이드 패널")]
        public GameObject guidePanel;
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

        MapCount mapCount;

        [Header("우측 상단 프로필")]
        public UserImage mineUserImage;

        [Header("프로필 편집 인풋필드 리스트")]
        public List<TMP_InputField> profileInputField;


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


            if (MapRegisterCloseButton)
            {
                MapRegisterCloseButton.onClick.AddListener(CloseMapRegisterPanel);
            }

            if (MapContestCloseButton)
                MapContestCloseButton.onClick.AddListener(CloseMapContestPanel);

            // StartCoroutine(coMapInventoryGameObject());
            InventoryButton.onClick.AddListener(OnMapInventoryPanel);
            InventoryCloseButton.onClick.AddListener(CloseMapInventoryPanel);

            if (mapContestButton)
                mapContestButton.onClick.AddListener(OnMapConfirmPanel);
            if (onMapRegisterButton)
            {
                onMapRegisterButton.onClick.AddListener(OnMapRegisterPanel);
                mapContestButton.onClick.AddListener(OnMapConfirmPanel);
            }

            //if (MapConfirmYesButton)
            //    MapConfirmYesButton.onClick.AddListener(OnMapRegisterPanel);

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

            if (shopCloseButton)
                shopCloseButton.onClick.AddListener(OffShopPanel);
            //if (commonButton)
            //    commonButton.onClick.AddListener(OnInventoryCommon);

            //if (myClassRoomButton)
            //    myClassRoomButton.onClick.AddListener(OnInventoryMyClassRoom);

            if (AuthManager.GetInstance().userAuthData.userInfo.school.schoolName == "")
            {
                RegisterAvatar();

                firstLoginPanel.SetActive(true);
                guidePanel.SetActive(true);
            }
            else
                GetAvatar();

            InterestButtonCreate();

            SetProfile();
            initDecorationPanel();
            InitOtherPlayerPanel();
            initDecorationPanel();
            ProfileSet();
            inventoryRT = mapInventoryPanel.GetComponent<RectTransform>();
            inventoryDataRT = mapInventoryPanel.transform.GetChild(0).GetComponent<RectTransform>();
            mainPanelLayoutGroup = inventoryRT.parent.GetComponent<VerticalLayoutGroup>();
        }
        private void Update()
        {
            if (myProfileEditPanel.activeSelf)
            {
                ProfileEditCount();

            }

            if (firstSchoolPanel.activeSelf)
            {
                //if (Input.GetKeyDown(KeyCode.Return))
                //{
                //    print("엔터");
                //    SchoolGet();
                //}
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
        public void OnArchivePanel()
        {
            archivePanel.SetActive(true);
            archivePanel.GetComponent<Gallery>().LoadData();
        }
        public void OnMapRegisterPanel()
        {
            mapRegisterPanel.SetActive(true);
            mapContestPanel.SetActive(false);

            // 맵 등록 패널에 이미지 업로드
            SettingMapPanel.GetInstance().UploadMapImageData();
            // 여기서 
        }

        public void OnMapContestPanel()
        {
            mapRegisterPanel.SetActive(false);
            mapContestPanel.SetActive(true);
            mapContestButton.gameObject.SetActive(true);
            SettingMapPanel.GetInstance().OnMapContestPanel();
        }

        IEnumerator coMapInventoryGameObject()
        {
            yield return new WaitUntil(() => mapInventoryPanel != null);
        }
        // 인벤토리 애니메이션
        public RectTransform inventoryRT;
        public RectTransform inventoryDataRT;
        public VerticalLayoutGroup mainPanelLayoutGroup;
        public void SetInventoryPanel(bool value)
        {
            mapInventoryPanel.SetActive(value);
            if (!value) mainPanelLayoutGroup.spacing = 0;
        }
        public void MoveInventoryPanel(float newValue)
        {
            //inventoryRT.SetHeight(newValue);
            inventoryDataRT.anchoredPosition = new Vector2(inventoryDataRT.anchoredPosition.x, (newValue - 775) * 1.6f);
            mainPanelLayoutGroup.spacing = -200 -200 + 200 * newValue/ 775;
        }
        public void OnMapInventoryPanel()
        {
            //asdfasdf
            iTween.Stop(mapInventoryPanel);
            iTween.ValueTo(mapInventoryPanel, iTween.Hash(
                "from", 0,
                "to", 775,
                "time", 0.6f,
                "easetype", iTween.EaseType.easeOutCubic,
                "onupdate", nameof(MoveInventoryPanel),
                "onupdatetarget", gameObject
            ));
            SetInventoryPanel(true);
            //mapInventoryPanel.SetActive(true);
            InventoryCloseButton.gameObject.SetActive(true);
            InventoryButton.gameObject.SetActive(false);
            ChatPanel.gameObject.SetActive(false);

            if (DataManager.instance.player != null)
            {
                DataManager.instance.player.GetComponent<SetTile>().setMode = true;
            }
        }
        public void CloseMapInventoryPanel()
        {
            //asdfasdf
            iTween.Stop(mapInventoryPanel);
            iTween.ValueTo(mapInventoryPanel, iTween.Hash(
                "from", 775,
                "to", 0,
                "time", 0.6f,
                "easetype", iTween.EaseType.easeInCubic,
                "onupdate", nameof(MoveInventoryPanel),
                "onupdatetarget", gameObject,
                "oncomplete", nameof(SetInventoryPanel),
                "oncompletetarget", gameObject,
                "oncompleteparams", false
            ));
            //mapInventoryPanel.SetActive(false);
            InventoryCloseButton.gameObject.SetActive(false);
            InventoryButton.gameObject.SetActive(true);
            ChatPanel.gameObject.SetActive(true);
            if (DataManager.instance.player != null)
            {
                DataManager.instance.player.GetComponent<SetTile>().setMode = false;
                QuestManager.instance.QuestPatch(4);
            }
        }

        //샵 온 오프
        public void OnShopPanel()
        {
            shopPanel.SetActive(true);
        }

        public void OffShopPanel()
        {
            shopPanel.SetActive(false);
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


        public void OnInventoryUI()
        {
            mapInventoryPanel.SetActive(false);
            CloseMapInventoryPanel();
            InventoryCloseButton.gameObject.SetActive(false);
            InventoryButton.gameObject.SetActive(true);
        }

        public void OffInventoryUI()
        {
            mapInventoryPanel.SetActive(false);
            CloseMapInventoryPanel();
            InventoryCloseButton.gameObject.SetActive(false);
            InventoryButton.gameObject.SetActive(false);
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
            // mapSuccessRegisterPanel.SetActive(false);
        }

        public void OnMenuButtonClick()
        {
            if (menuPanel.activeSelf)
            {
                //asdfasdf
                iTween.Stop(menuPanel);
                MoveMenuPanel(267);
                iTween.ValueTo(menuPanel, iTween.Hash(
                    "from", 267,
                    "to", 0,
                    "time", 0.6f,
                    "easetype", iTween.EaseType.easeOutBounce,
                    "onupdate", nameof(MoveMenuPanel),
                    "onupdatetarget", gameObject,
                    "oncomplete", nameof(SetMenuPanel),
                    "oncompletetarget", gameObject,
                    "oncompleteparams", false
                ));
            }
            else
            {
                iTween.Stop(menuPanel);
                MoveMenuPanel(0);
                SetMenuPanel(true);
                iTween.ValueTo(menuPanel, iTween.Hash(
                    "from", 0,
                    "to", 267,
                    "time", 0.6f,
                    "easetype", iTween.EaseType.easeOutBounce,
                    "onupdate", nameof(MoveMenuPanel),
                    "onupdatetarget", gameObject
                ));
            }
        }
        public void SetMenuPanel(bool value)
        {
            menuPanel.SetActive(value);
        }
        public void MoveMenuPanel(float newValue)
        {
            menuPanelRT.anchoredPosition = new Vector3(newValue, menuPanelRT.anchoredPosition.y, 0);
        }

        public void OnFriendsPanel()
        {
            friendsPanel.SetActive(true);
            friendsPanel.GetComponent<FriendsUI>().RefreshFriends();
        }
        public void OnReqFriendPanel(int id)
        {
            requestFriendPanel.SetActive(true);
            requestFriendPanel.GetComponent<RequestFriendPanel>().receiverId = id;
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
        bool isProfileOff = true;
        TMP_Text emotionText;
        public void OnOffMyProfile()
        {
            //버튼으로 키고 끄기
            isProfileOff = !isProfileOff;
            myProfilePanel.SetActive(true);
            iTween.Stop(myProfilePanel);
            iTween.ValueTo(myProfilePanel, iTween.Hash(
                "from", isProfileOff ? 0 : -700,
                "to", isProfileOff ? -700 : 0,
                "time", 0.6f,
                "easetype", isProfileOff ? iTween.EaseType.easeInCubic : iTween.EaseType.easeOutCubic,
                "onupdate", nameof(MoveProfilePanel),
                "onupdatetarget", gameObject
                ));
            Image myProfileImage = myProfileButton.transform.GetChild(0).GetComponent<Image>();
            Color32 myprofileColor = myProfilePanel.activeSelf ? new Color32(242, 136, 75, 255) : new Color32(202, 202, 202, 255);
            myProfileImage.color = myprofileColor;
            if (!isProfileOff)
            {
                HttpInfo info = new HttpInfo();
                info.url = HttpManager.GetInstance().SERVER_ADRESS + "/emotion-analysis/" + AuthManager.GetInstance().userAuthData.userInfo.id;
                info.onComplete = (DownloadHandler res) =>
                {
                    EmotionInfo data = JsonUtility.FromJson<EmotionInfo>(res.text);
                    if (data.emotion == "" || data.emotion == "없음")
                    {
                        emotionText.text = "기쁨";
                    }
                    else
                    {
                        emotionText.text = data.emotion;
                    }
                };
                StartCoroutine(HttpManager.GetInstance().Get(info));
            }
        }
        [Serializable]
        private struct EmotionInfo
        {
            public string message;
            public string emotion;
            public int score;
        }

        public void MoveProfilePanel(float value)
        {
            RectTransform rt = myProfilePanel.GetComponent<RectTransform>();
            rt.anchoredPosition = new Vector2(rt.anchoredPosition.x, value);
        }
        //우측상단 이미지 새로 고침
        public void ProfileSet()
        {
            int userId = AuthManager.GetInstance().userAuthData.userInfo.id;
            mineUserImage.AvatarGet(userId);

        }

        public void OnDecorationPanel()
        {
            DecorationPanel.SetActive(true);
            PlayerDecoration.GetInstance().OnEnableDecorationPanel();

        }

        public void OffDecorationPanel()
        {
            PlayerAnimation.GetInstance().PatchAvatarData();
            DecorationPanel.SetActive(false);
            ProfileSet();
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
            ProfileFirstSet();
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
        public void OnVisitNearSchoolPanel()
        {
            visitNearSchoolPanel.SetActive(true);
        }

        public void OnVoicePanel()
        {
            voicePanel.SetActive(true);
        }

        public void OffVoicePanel()
        {
            voicePanel.SetActive(false);
        }

        public void OnQuizCategoryPanel()
        {
            quizCategoryPanel.SetActive(true);
        }

        public void OffQuizCategoryPanel()
        {
            quizCategoryPanel.SetActive(false);
        }

        //public void OnQuizQuestionPanel()
        //{
        //    quizQuestionPanel.SetActive(true);
        //}

        //public void OffQuizQuestionPanel()
        //{
        //    quizQuestionPanel.SetActive(false);
        //}

        public void OnMapContestBillPanel()
        {
            mapContestBill.gameObject.SetActive(true);
        }

        public void OffMapContestBillPanel()
        {
            mapContestBill.gameObject.SetActive(false);
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

                        image.color = noneSelectColor;
                        selectedInterest.RemoveAt(i);
                        break;
                    }
                    else
                    {
                        test = false;

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

        //프로필 편집 초기값
        private void ProfileFirstSet()
        {
            UserInfo userInfo = AuthManager.GetInstance().userAuthData.userInfo;
            //이름 창 세팅
            profileInputField[0].text = userInfo.name;
            //상태메시지 세팅
            profileInputField[1].text = userInfo.statusMesasge;


            //인터레스트 창 세팅
            interestInputField.text = "";
            for (int i = 0; i < userInfo.interest.Count; i++)
            {
                interestInputField.text += "#" + userInfo.interest[i] + " ";
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

            QuestManager.instance.QuestPatch(1);

            //프로필 이미지 변경 및 이름 변경
            ProfileSet();
            DataManager.instance.player.GetComponent<PlayerMalpung>().PlayerNameSet();

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
            emotionText = myProfilePanel.transform.GetChild(0).Find("Emotion_Text").GetComponent<TMP_Text>();
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

        public void SchoolGet()
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

            //프로필 아바타
            ProfileSet();

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
                    HttpInfo info = new HttpInfo();
                    info.url = HttpManager.GetInstance().SERVER_ADRESS + "/emotion-analysis/" + userInfo.id;
                    info.onComplete = (DownloadHandler res) =>
                    {
                        EmotionInfo data = JsonUtility.FromJson<EmotionInfo>(res.text);
                        if (data.emotion == "" || data.emotion == "없음")
                        {
                            comp.StateText.text = "기쁨";
                        }
                        else
                        {
                            comp.StateText.text = data.emotion;
                        }
                    };
                    StartCoroutine(HttpManager.GetInstance().Get(info));
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
                WebSocketManager.GetInstance().OnRequestFriendPanel(comp.id);
            });
        }

        public void MapTutorial()
        {
            if (DataManager.instance.mapType == DataManager.MapType.Quiz || DataManager.instance.mapType == DataManager.MapType.Login || DataManager.instance.mapType == DataManager.MapType.ContestClassroom || DataManager.instance.mapType == DataManager.MapType.Note || DataManager.instance.mapType == DataManager.MapType.Others)
                return;



            HttpInfo info = new HttpInfo();
            info.url = HttpManager.GetInstance().SERVER_ADRESS + "/user-pos-visit-count/" + DataManager.instance.mapType + "/" + AuthManager.GetInstance().userAuthData.userInfo.id;
            info.onComplete = (DownloadHandler downloadHandler) =>
            {
                string jsonData = downloadHandler.text;
                mapCount = JsonUtility.FromJson<MapCount>(jsonData);
                print("맵카!!!!!!! : " + mapCount.count);
                if (mapCount.count < 1)
                {

                    tutorialPanel.SetActive(true);

                    tutorialPanel.GetComponent<TutorialText>().TextChange(DataManager.instance.mapType);
                }

                info = new HttpInfo();
                info.url = HttpManager.GetInstance().SERVER_ADRESS + "/user-pos-visit-count/" + DataManager.instance.mapType + "/" + AuthManager.GetInstance().userAuthData.userInfo.id;
                info.contentType = "application/json";
                info.onComplete = (DownloadHandler downloadHandler) =>
                {
                    print(downloadHandler.text);
                };
                StartCoroutine(HttpManager.GetInstance().Patch(info));
            };
            StartCoroutine(HttpManager.GetInstance().Get(info));
        }

    }
}