using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

namespace GH
{
    public class UIManager : MonoBehaviour
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

        #region Button
        [Header("다음 버튼 리스트")]
        public List<Button> nextButtons;

        [Header("이전 버튼 리스트")]
        public List<Button> backButtons;

        [Header("4. 중복 버튼")]
        public Button checkIDButton;


        [Header("4. 인증 확인 버튼")]
        public Button checkNumButton;

        #endregion

        #region Panel
        [Header("로그인 패널 리스트")]
        public List<GameObject> logins;

        #endregion

        [Header("인덱스 슬라이더")]
        private Slider indexSlider;

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


        private void Start()
        {
            indexSlider = GameObject.Find("Index_Slider").GetComponent<Slider>();

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

        }

        private void Update()
        {
            Slider();
            PWCheck();
        }

        public void NextStep()
        {
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



    }
}