using GH;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace MJ
{
    public class SceneUIManager : MonoBehaviour
    {
        #region Button
        [Header("닫기 버튼")]
        public UnityEngine.UI.Button MapRegisterCloseButton;
        public UnityEngine.UI.Button MapContestCloseButton;

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

        #endregion

        #region Panel
        [Header("꾸미기 패널")]
        public GameObject DecorationPanel;

        [Header("플레이어 꾸미기 오브젝트")]
        public GameObject PlayerObject;

        [Header("방명록 패널")]
        public GameObject guestbookPanel;

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
            if(instance == null)
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

        // Start is called before the first frame update
        private void Start()
        {
            myProfileButton.onClick.AddListener(OnOffMyProfile);
        }

        public void RestartSetting(
            Button _mapContestCloseButton,
            Button _mapRegisterCloseButton,
            Button _mapRegisterButton,
            Button _InventoryButton,
            Button _InventoryCloseButton,
            Button _mapContestButton,
            Button _mapConfirmYesButton,
            Button _mapConfirmNoButton,
            Button _mapRegisterSuccessCloseButton,
            Button _InventoryErrorCloseButton,

            GameObject _mapContestPanel,
            GameObject _mapRegisterPanel,
            GameObject _mapConfirmPanel,
            GameObject _mapSuccessRegisterPanel,
            GameObject _mapInventoryErrorPanel
            )
        {
            MapContestCloseButton = _mapContestCloseButton;
            MapRegisterCloseButton = _mapRegisterCloseButton;
            mapRegisterButton = _mapRegisterButton;
            InventoryButton = _InventoryButton;
            InventoryCloseButton = _InventoryCloseButton;
            mapContestButton = _mapContestButton;
            MapConfirmYesButton = _mapConfirmYesButton;
            MapConfirmNoButton = _mapConfirmNoButton;
            MapRegisterSuccessCloseButton = _mapRegisterSuccessCloseButton;
            mapInventoryErrorPanel = _mapInventoryErrorPanel;
            InventoryErrorCloseButton = _InventoryErrorCloseButton;

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

            InventoryButton.onClick.AddListener(OnMapInventoryPanel);

            InventoryCloseButton.onClick.AddListener(CloseMapInventoryPanel);

            if (mapContestButton)
                mapContestButton.onClick.AddListener(OnMapConfirmPanel);

            if(MapConfirmYesButton)
                MapConfirmYesButton.onClick.AddListener(OnMapRegisterPanel);
            if (MapConfirmYesButton)
                MapConfirmYesButton.onClick.AddListener(OffMapConfirmPanel);
            if (MapConfirmNoButton)
                MapConfirmNoButton.onClick.AddListener(OffMapConfirmPanel);

            if (MapRegisterSuccessCloseButton)
                MapRegisterSuccessCloseButton.onClick.AddListener(OffMapSuccessRegisterPanel);

            if (InventoryErrorCloseButton)
                InventoryErrorCloseButton.onClick.AddListener(OffMapInventoryErrorPanel);

            if (_mapContestPanel)
                mapContestPanel = _mapContestPanel;
            if (_mapRegisterPanel)
                mapRegisterPanel = _mapRegisterPanel;
            if (_mapConfirmPanel)
                mapConfirmPanel = _mapConfirmPanel;
            if (_mapSuccessRegisterPanel)
                mapSuccessRegisterPanel = _mapSuccessRegisterPanel;
        }
        
        public void initDecorationPanel()
        {
            PlayerDecoration DecorationDT = DecorationPanel.GetComponent<PlayerDecoration>();
            PlayerAnimation AnimationDT = PlayerObject.GetComponent<PlayerAnimation>();

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
                    DecorationDT.SetPlayerSelectDecorationData(DecorationDT.CurDecorationPanel, data);

                    AnimationDT.ResetDecorationAnimData(DecorationDT.CurDecorationPanel);
                    AnimationDT.SetDecorationAnimData(DecorationDT.CurDecorationPanel, data);
                });
            }
        }
        public void OnGuestbookPanel()
        {
            guestbookPanel.SetActive(true);
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
            if(DataManager.instance.player != null)
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
            mapSuccessRegisterPanel.SetActive(true);
        }

        public void OffMapSuccessRegisterPanel()
        {
            mapSuccessRegisterPanel.SetActive(false);
        }

        public void OnMenuButtonClick()
        {
            menuPanel.SetActive(!menuPanel.activeSelf);
        }

        public void OnFriendsPanel()
        {
            friendsPanel.SetActive(true);
        }

        public void OffAllMapPanel()
        {
            if(mapContestPanel)
                mapContestPanel.SetActive(false);
            if (mapRegisterPanel)
                mapRegisterPanel.SetActive(false);
            if (mapConfirmPanel)
                mapConfirmPanel.SetActive(false);
            if(mapSuccessRegisterPanel)
                mapSuccessRegisterPanel.SetActive(false);
        }

        public void OnOffMyProfile()
        {
            //버튼으로 키고 끄기
            myProfilePanel.SetActive(!myProfilePanel.activeSelf);

            Image myProfileImage = myProfileButton.GetComponentInChildren<Image>();
            Color32 myprofileColor = myProfilePanel.activeSelf ? new Color32 (242, 136, 75, 255) : new Color32(29, 27, 32, 255);
            myProfileImage.color = myprofileColor;

        }

        public void OffMapInventoryErrorPanel()
        {
            mapInventoryErrorPanel.SetActive(false);
        }

        public void OnMapInventoryErrorPanel()
        {
            mapInventoryErrorPanel.SetActive(true);
        }
    }
}