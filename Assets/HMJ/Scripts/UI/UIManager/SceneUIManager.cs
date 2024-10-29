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

        [Header("맵 콘테스트 패널")]
        public GameObject mapContestPanel;

        [Header("메뉴 패널")]
        public GameObject menuPanel;

        [Header("친구창 패널")]
        public GameObject friendsPanel;
        #endregion

        #region SingleTone
        private static SceneUIManager instence;
        public static SceneUIManager GetInstance()
        {
            if (instence == null)
            {
                GameObject go = new GameObject();
                go.name = "SceneUIManager";
                go.AddComponent<SceneUIManager>();
            }
            return instence;
        }

        private void Awake()
        {
            if(instence == null)
            {
                instence = this;
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
            //PlayerDecoration DecorationDT = DecorationPanel.GetComponent<PlayerDecoration>();
            //PlayerAnimation AnimationDT = PlayerObject.GetComponent<PlayerAnimation>();

            //mapRegisterButton.onClick.AddListener(OnMapRegisterPanel);

            //MapRegisterCloseButton.onClick.AddListener(CloseMapRegisterPanel);
            //MapRegisterCloseButton.onClick.AddListener(OnMapContestPanel);

            //MapContestCloseButton.onClick.AddListener(CloseMapContestPanel);

            //for (int i = 0; i < decorationEnumButton.Length; i++)
            //{
            //    int data = i;
            //    decorationEnumButton[i].onClick.AddListener(() => DecorationDT.SetPlayerDecorationData((DecorationEnum.DECORATION_DATA)data));
            //}

            //for (int i = 0; i < decorationChoiceButton.Length; i++)
            //{
            //    int data = i;
            //    decorationChoiceButton[i].onClick.AddListener(() =>
            //    {
            //        DecorationDT.SetPlayerSelectDecorationData(DecorationDT.CurDecorationPanel, data);
                    
            //        AnimationDT.ResetDecorationAnimData(DecorationDT.CurDecorationPanel); 
            //        AnimationDT.SetDecorationAnimData(DecorationDT.CurDecorationPanel, data);
            //    });
            //}
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
        }

        public void CloseMapRegisterPanel()
        {
            mapRegisterPanel.SetActive(false);
        }

        public void CloseMapContestPanel()
        {
            mapContestPanel.SetActive(false);
        }

        public void OnMenuButtonClick()
        {
            menuPanel.SetActive(!menuPanel.activeSelf);
        }
        public void OnFriendsPanel()
        {
            friendsPanel.SetActive(true);
        }
    }
}