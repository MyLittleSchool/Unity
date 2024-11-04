using GH;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

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

        [Header("맵 인벤토리 패널 - 맵 인벤토리 패널 버튼")]
        public UnityEngine.UI.Button InventoryButton;

        [Header("맵 인벤토리 패널 - 맵 인벤토리 패널 끄기 버튼")]
        public UnityEngine.UI.Button InventoryCloseButton;


        [Header("메뉴 버튼")]
        public UnityEngine.UI.Button topMenuButton;

        [Header("친구창 버튼")]
        public UnityEngine.UI.Button friendsButton;

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

        [Header("맵 인벤토리 패널")]
        public GameObject mapInventoryPanel;

        [Header("메뉴 패널")]
        public GameObject menuPanel;

        [Header("친구창 패널")]
        public GameObject friendsPanel;

        [Header("채팅 패널")]
        public GameObject ChatPanel;
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
            //if(mapRegisterButton)
            //    mapRegisterButton.onClick.AddListener(OnMapRegisterPanel);

            //if (MapRegisterCloseButton)
            //{
            //    MapRegisterCloseButton.onClick.AddListener(CloseMapRegisterPanel);
            //    MapRegisterCloseButton.onClick.AddListener(OnMapContestPanel);
            //}

            //if(MapContestCloseButton)
            //    MapContestCloseButton.onClick.AddListener(CloseMapContestPanel);

            //if (!InventoryButton)
            //    InventoryButton = GameObject.Find("MJCanvas/MapContestOnOffPanel/Button_Panel/Button").GetComponent<UnityEngine.UI.Button>();
            //InventoryButton.onClick.AddListener(OnMapInventoryPanel);

            //if (!InventoryCloseButton)
            //    InventoryCloseButton = GameObject.Find("MJCanvas/InventoryOnOffPanel/Button_Panel/Button_Panel (1)/Button").GetComponent<UnityEngine.UI.Button>();
            //InventoryCloseButton.onClick.AddListener(CloseMapInventoryPanel);

            //if (mapContestButton)
            //    mapContestButton.onClick.AddListener(OnMapContestPanel);
        }

        public void RestartSetting(
            UnityEngine.UI.Button _mapContestCloseButton,
            UnityEngine.UI.Button _mapRegisterCloseButton,
            UnityEngine.UI.Button _mapRegisterButton,
            UnityEngine.UI.Button _InventoryButton,
            UnityEngine.UI.Button _InventoryCloseButton,
            UnityEngine.UI.Button _mapContestButton,
            UnityEngine.UI.Button _mapConfirmYesButton,
            UnityEngine.UI.Button _mapConfirmNoButton,
            GameObject _mapContestPanel,
            GameObject _mapRegisterPanel,
            GameObject _mapConfirmPanel
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

            if (mapRegisterButton)
                mapRegisterButton.onClick.AddListener(OnMapRegisterPanel);

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

            if(_mapContestPanel)
                mapContestPanel = _mapContestPanel;
            if (_mapRegisterPanel)
                mapRegisterPanel = _mapRegisterPanel;
            if (_mapConfirmPanel)
                mapConfirmPanel = _mapConfirmPanel;
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
            if (!(mapContestPanel && mapRegisterPanel && mapConfirmPanel))
                return;
            mapContestPanel.SetActive(false);
            mapRegisterPanel.SetActive(false);
            mapConfirmPanel.SetActive(false);
        }
    }
}