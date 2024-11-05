using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using static HttpManager;

namespace GH
{


    public class LoginJoinUIManager : MonoBehaviour
    {
        public string iP;
        public string port;
        public enum Loginstep
        {
            START,
            SERVICE,
            LOGIN,
            NAME,
            EMAIL,
            PASSWORD,
            INTEREST,
            PERSONAL
        }

        public Loginstep currentLoginstep;

        public UserInfo currentJoinInfo;

        #region Button
        [Header("다음 버튼 리스트")]
        public List<Button> nextButtons;

        [Header("이전 버튼 리스트")]
        public List<Button> backButtons;

        [Header("4. 중복 버튼")]
        public Button checkIDButton;

        [Header("4. 인증 확인 버튼")]
        public Button checkNumButton;

        [Header("7. 인증 확인 버튼")]
        public Button joinButton;

        #endregion

        #region Panel
        [Header("로그인 패널 리스트")]
        public List<GameObject> logins;

        #endregion

        [Header("인덱스 슬라이더")]
        public Slider indexSlider;

        [Header("4. 중복 아이디 텍스트")]
        public TMP_Text checkIDText;

        [Header("4. 중복 확인 텍스트")]
        public TMP_Text checkNumText;

        [Header("5. 비밀번호 인풋필드")]
        public TMP_InputField pWInputField;

        [Header("5. 비밀번호 확인 인풋필드")]
        public TMP_InputField pWCheckInputField;

        [Header("6. 관심사 리스트")]
        public List<string> interests;

        [Header("6. 관심사 버튼 프리팹")]
        public GameObject interestButtonPrefab;

        [Header("6. 관심사 버튼 생성위치")]
        public Transform interestButtonTransform;

        [Header("6. 관심사 띄우는 배열")]
        public Queue<String> interestsQueue = new Queue<String>(5);

        [Header("통신 인풋필드 리스트")]
        // 0 - 이름, 1 - 이메일, 2 - PassWord, 3 - 생년월일
        public List<TMP_InputField> joinInfoInfoList;


        private void Start()
        {

            // 초기 패널 엑티브 적용
            for (int i = 0; i < logins.Count; i++)
            {
                logins[i].SetActive(false);
            }
            logins[0].SetActive(true);

            // 다음으로 넘어가는 버튼들 할당
            for (int i = 0; i < nextButtons.Count; i++)
            {
                nextButtons[i].onClick.AddListener(NextStep);
            }

            //  뒤로가는 버튼들 할당
            for (int i = 0; i < backButtons.Count; i++)
            {
                backButtons[i].onClick.AddListener(BackStep);
            }

            //4 페이지
            checkNumButton.onClick.AddListener(CheckNum);
            checkNumText.gameObject.SetActive(false);
            checkIDButton.onClick.AddListener(CheckID);
            checkIDText.gameObject.SetActive(false);

            //6 페이지
            InterestButtonCreate();

            //7페이지
            joinButton.onClick.AddListener(UserJoin);
        }

        private void Update()
        {
            Slider();
            PWCheck();
        }

        public void NextStep()
        {
            switch (currentLoginstep)
            {
                case Loginstep.NAME:
                    currentJoinInfo.name = joinInfoInfoList[0].text;
                    break;

                case Loginstep.EMAIL:
                    break;

                case Loginstep.PASSWORD:
                    break;

                case Loginstep.INTEREST:
                    break;
                    
                case Loginstep.PERSONAL:
                    break;

            }

            for (int i = 0; i < logins.Count; i++)
            {

                logins[i].SetActive(false);

            }
            logins[(int)currentLoginstep + 1].SetActive(true);
            currentLoginstep++;
        }
        public void BackStep()
        {
            for (int i = 0; i < logins.Count; i++)
            {

                logins[i].SetActive(false);

            }
            logins[(int)currentLoginstep - 1].SetActive(true);
            currentLoginstep--;
        }

        private void Slider()
        {
            int indexConvert = (int)currentLoginstep - 2;
            float sliderValue = indexConvert * 0.2f;
            indexSlider.value = Mathf.Lerp(indexSlider.value, sliderValue, 0.05f);
        }

        public void CheckNum()
        {
            //통신***** 인증번호 확인
            checkNumText.gameObject.SetActive(true);
        }
        public void CheckID()
        {
            //통신***** 아이디 중복 확인
            checkIDText.gameObject.SetActive(true);
        }

        public void PWCheck()
        {
            if (pWInputField.text == pWCheckInputField.text)
            {
                nextButtons[5].GetComponent<Image>().color = new Color32(242, 136, 75, 255);
                nextButtons[5].interactable = true;
            }
            else
            {
                nextButtons[5].GetComponent<Image>().color = new Color32(242, 242, 242, 255);
                nextButtons[5].interactable = false;
            }
        }

        private void InterestButtonCreate()
        {
            for (int i = 0; i < interests.Count; i++)
            {
                Button interestButton = Instantiate(interestButtonPrefab, interestButtonTransform).GetComponent<Button>();
                interestButton.GetComponentInChildren<TMP_Text>().text = interests[i];
            }
        }



        //통신 코드
        private void UserJoin()
        {
            UserInfo joinInfo = new UserInfo();
            joinInfo.email = "aaa@naver.com";
            joinInfo.name = "이규현";
            joinInfo.birthday = "20241231";
            joinInfo.gender = true;
            joinInfo.password = "asd123";
            joinInfo.interest = new List<string>() {"공부", "영화", "게임" };


            HttpInfo info = new HttpInfo();
            info.url = "http://" + iP +":"+port+"/user";
            info.body = JsonUtility.ToJson(joinInfo);
            info.contentType = "application/json";
            info.onComplete = (DownloadHandler downloadHandler) =>
            {
                print(downloadHandler.text);
            };
            StartCoroutine(HttpManager.GetInstance().Post(info));
        }

    }
}