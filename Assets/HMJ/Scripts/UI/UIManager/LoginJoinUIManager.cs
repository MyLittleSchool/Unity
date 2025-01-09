using SW;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using static HttpManager;

namespace GH
{


    public class LoginJoinUIManager : MonoBehaviour
    {
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

        public UserInfoData getUserInfo;

        public TokenData tokenGet;

        #region Button
        [Header("다음 버튼 리스트")]
        public List<Button> nextButtons;

        [Header("이전 버튼 리스트")]
        public List<Button> backButtons;

        [Header("4. 중복 버튼")]
        public Button checkIDButton;

        [Header("4. 인증 확인 버튼")]
        public Button checkNumButton;

        [Header("7. 가입 완료 버튼")]
        public Button joinButton;

        [Header("7. 남자 버튼")]
        public Button menButton;

        [Header("7. 여자 버튼")]
        public Button womenButton;

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

        [Header("6. 관심사 버튼 프리팹")]
        public GameObject interestButtonPrefab;

        [Header("6. 관심사 버튼 생성위치")]
        public Transform interestButtonTransform;

        [Header("6. 관심사 띄우는 텍스트")]
        public TMP_Text interestText;

        [Header("6. 선택된 관심사 리스트")]
        public List<string> selectedInterest;

        [Header("6. 관심사 딕셔너리")]
        private Dictionary<string, GameObject> buttonList = new Dictionary<string, GameObject>();

        [Header("7. 남녀성별")]
        public bool gender;

        [Header("8. 이메일 중복")]
        public bool isEmailDuplicate;

        [Header("통신 인풋필드 리스트")]
        // 0 - 이름, 1 - 이메일, 2 - PassWord, 3 - 생년월일
        public List<TMP_InputField> joinInfoInfoList;

        [Header("로그인 인풋필드 리스트")]
        // 0 - 이메일. 1 - 비밀번호
        public List<TMP_InputField> loginList;

        [Header("로그인 버튼")]
        public Button loginButton;

        [Header("비밀번호 틀림 텍스트")]
        public GameObject PWCheckText;


        private Color32 selectColor = new Color32(242, 136, 75, 255);
        private Color32 noneSelectColor = new Color32(242, 242, 242, 255);


        public TMP_InputField schoolInputField;

        private void Start()
        {

            isEmailDuplicate = false;

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
            menButton.onClick.AddListener(ClickMen);
            womenButton.onClick.AddListener(ClickWomen);


            //로그인 버트
            loginButton.onClick.AddListener(UserLogin);

            PWCheckText.SetActive(false);

        }

        private void Update()
        {
            switch (currentLoginstep)
            {
                case Loginstep.NAME:
                    break;

                case Loginstep.EMAIL:
                    break;

                case Loginstep.PASSWORD:
                    PWCheck();
                    break;

                case Loginstep.INTEREST:
                    break;
            }
            Slider();
        }

        public void NextStep()
        {
            switch (currentLoginstep)
            {
                case Loginstep.NAME:
                    currentJoinInfo.name = joinInfoInfoList[0].text;
                    break;

                case Loginstep.EMAIL:
                    if (isEmailDuplicate)
                        return;
                    currentJoinInfo.email = joinInfoInfoList[1].text;
                    break;

                case Loginstep.PASSWORD:
                    currentJoinInfo.password = joinInfoInfoList[2].text;
                    break;

                case Loginstep.INTEREST:
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
            IsEmailDuplicate((isDuplicate) =>
            {
                checkIDText.gameObject.SetActive(true);
                isEmailDuplicate = isDuplicate;
                if (isDuplicate)
                    checkIDText.text = "가입 불가능한 이메일 입니다.";
                else
                    checkIDText.text = "가입 가능한 이메일 입니다.";
            });

        }

        public void PWCheck()
        {
            if (pWCheckInputField.text.Length > 0 && pWInputField.text == pWCheckInputField.text)
            {
                nextButtons[5].GetComponent<Image>().color = selectColor;
                nextButtons[5].interactable = true;
            }
            else
            {
                nextButtons[5].GetComponent<Image>().color = noneSelectColor;
                nextButtons[5].interactable = false;
            }
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
            interestText.text = "";

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
                    interestText.text += "#" + selectedInterest[i] + " ";

                }
            }
            else
            {
                image.color = new Color32(242, 136, 75, 255);
                selectedInterest.Add(key);
                interestText.text += "#" + key + " ";
            }

        }


        //통신 코드
        private void UserJoin()
        {
            currentJoinInfo.birthday = joinInfoInfoList[3].text;

            UserInfo joinInfo = new UserInfo();
            joinInfo.email = currentJoinInfo.email;
            joinInfo.name = currentJoinInfo.name;
            joinInfo.birthday = currentJoinInfo.birthday;
            joinInfo.gender = gender;
            joinInfo.password = currentJoinInfo.password;
            joinInfo.interest = selectedInterest;


            HttpInfo info = new HttpInfo();
            info.url = HttpManager.GetInstance().SERVER_ADRESS + "/auth/sign-up";
            info.body = JsonUtility.ToJson(joinInfo);
            info.contentType = "application/json";
            info.onComplete = (DownloadHandler downloadHandler) =>
            {
                print(downloadHandler.text);
            };
            StartCoroutine(HttpManager.GetInstance().Post(info));

            logins[7].SetActive(false);
            logins[2].SetActive(true);

            currentLoginstep = Loginstep.LOGIN;
        }



        private void UserLogin()
        {

            UserInfo tokenInfo = new UserInfo();
            tokenInfo.userEmail = loginList[0].text;
            tokenInfo.password = loginList[1].text;

            HttpInfo info = new HttpInfo();
            info.url = HttpManager.GetInstance().SERVER_ADRESS + "/auth/login";
            info.body = JsonUtility.ToJson(tokenInfo);
            info.contentType = "application/json";
            info.onComplete = (DownloadHandler downloadHandler) =>
            {
                string jsonData = "{ \"data\" : " + downloadHandler.text + "}";

                tokenGet = JsonUtility.FromJson<TokenData>(jsonData);
                AuthManager.GetInstance().accessToken = tokenGet.data.accessToken;
                AuthManager.GetInstance().refreshToken = tokenGet.data.refreshToken;

                if (AuthManager.GetInstance().accessToken != "")
                {
                    print("id : " + loginList[0].text);
                    // 겟으로 받아오기
                    HttpInfo info2 = new HttpInfo();
                    //info2.url = HttpManager.GetInstance().SERVER_ADRESS + "/user/list";
                    info2.url = HttpManager.GetInstance().SERVER_ADRESS + "/user/email/" + loginList[0].text;
                    info2.onComplete = (DownloadHandler downloadHandler) =>
                    {
                        string jsonData = "{ \"data\" : " + downloadHandler.text + "}";
                        print(jsonData);
                        //jsonData를 PostInfoArray 형으로 바꾸자.
                        getUserInfo = JsonUtility.FromJson<UserInfoData>(jsonData);
                        print("get : " + getUserInfo);
                        LoginCallback();
                    };
                    StartCoroutine(HttpManager.GetInstance().Get(info2));
                }
                else
                {
                    PWCheckText.SetActive(true);
                    print("로그인에 실패하였습니다.");
                }
            };
            StartCoroutine(HttpManager.GetInstance().Post(info));


        }

        private void testGet()
        {
            UserInfo tokenInfo = new UserInfo();
            tokenInfo.userEmail = loginList[0].text;
            tokenInfo.password = loginList[1].text;


            HttpInfo info = new HttpInfo();
            info.url = HttpManager.GetInstance().SERVER_ADRESS + "/auth/login";
            info.body = JsonUtility.ToJson(tokenInfo);
            info.contentType = "application/json";
            info.onComplete = (DownloadHandler downloadHandler) =>
            {
                string jsonData = "{ \"data\" : " + downloadHandler.text + "}";

                tokenGet = JsonUtility.FromJson<TokenData>(jsonData);
                print("token : " + tokenGet.data.accessToken);
                AuthManager.GetInstance().accessToken = tokenGet.data.accessToken;
                if (AuthManager.GetInstance().accessToken != "")
                {
                    print("id : " + loginList[0].text);
                    // 겟으로 받아오기
                    HttpInfo info2 = new HttpInfo();
                    info.url = HttpManager.GetInstance().SERVER_ADRESS + "/user/list";
                    //info.url = HttpManager.GetInstance().SERVER_ADRESS + "/user/email/" + loginList[0].text;
                    info.onComplete = (DownloadHandler downloadHandler) =>
                    {
                        string jsonData = "{ \"data\" : " + downloadHandler.text + "}";
                        print(jsonData);
                        //jsonData를 PostInfoArray 형으로 바꾸자.
                        //getUserInfo = JsonUtility.FromJson<UserInfoData>(jsonData);
                        // print("get : " + getUserInfo);
                        // LoginCallback();
                    };
                    StartCoroutine(HttpManager.GetInstance().Get(info2));
                }
                else
                {
                    PWCheckText.SetActive(true);
                    print("로그인에 실패하였습니다.");
                }
            };
            StartCoroutine(HttpManager.GetInstance().Post(info));
        }
        private void LoginCallback()
        {
            print("로그인 성공");
            AuthManager.GetInstance().userAuthData = new AuthManager.AuthData(getUserInfo.data);
            //씬 넘어가기
            DataManager.instance.playerName = getUserInfo.data.name;
            SceneMgr.instance.Login();

        }

        private void ClickMen()
        {
            menButton.GetComponent<Image>().color = selectColor;
            womenButton.GetComponent<Image>().color = noneSelectColor;
            gender = true;
        }
        private void ClickWomen()
        {
            menButton.GetComponent<Image>().color = noneSelectColor;
            womenButton.GetComponent<Image>().color = selectColor;
            gender = false;
        }


        public void IsEmailDuplicate(System.Action<bool> callback)
        {
            HttpManager.HttpInfo info = new HttpManager.HttpInfo();
            info.url = HttpManager.GetInstance().SERVER_ADRESS + "/user/is-exist/" + joinInfoInfoList[1].text;
            info.onComplete = (DownloadHandler res) =>
            {
                Debug.Log("info.url: " + info.url);
                Debug.Log("res bool: " + res.text);

                bool isDuplicate = res.text == "true";
                callback(isDuplicate);
            };
            StartCoroutine(HttpManager.GetInstance().Get(info));
        }
    }

    [System.Serializable]
    public struct Token
    {
        public string accessToken;
        public string refreshToken;
    }

    public struct TokenData
    {
        public Token data;
    }
}