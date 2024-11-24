using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
namespace SW
{
    public class ToastMessage : MonoBehaviour
    {
        private static ToastMessage instance;
        public TMP_Text text;
        public RectTransform panelRT;
        public Image image;
        private void Awake()
        {
            if (instance == null)
                instance = this;
            else
                Destroy(gameObject);
        }
        public static void OnMessage(string text)
        {
            instance.Appear(text);
        }
        private void MovePanel(float newValue)
        {
            panelRT.anchoredPosition = new Vector2(panelRT.anchoredPosition.x, newValue);
        }
        private void SetAlpha(float newValue)
        {
            Color newColor1 = new Color(image.color.r, image.color.g, image.color.b);
            newColor1.a = newValue;
            image.color = newColor1;
            Color newColor2 = new Color(text.color.r, text.color.g, text.color.b);
            newColor2.a = newValue;
            text.color = newColor2;
        }
        private void Appear(string _text)
        {
            iTween.Stop(gameObject);
            text.text = _text;
            SetAlpha(1);
            MovePanel(80);
            iTween.ValueTo(gameObject, iTween.Hash(
                "from", 80,
                "to", -350,
                "time", 0.5f,
                "easetype", iTween.EaseType.easeOutQuart,
                "onupdate", nameof(MovePanel),
                "onupdatetarget", gameObject,
                "oncomplete", nameof(Disappear),
                "oncompletetarget", gameObject
            ));
        }
        private void Disappear()
        {
            iTween.ValueTo(gameObject, iTween.Hash(
                "from", 1,
                "to", 0,
                "time", 0.5f,
                "delay", 3,
                "easetype", iTween.EaseType.easeInCubic,
                "onupdate", nameof(SetAlpha),
                "onupdatetarget", gameObject
            ));
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Alpha1))
                OnMessage("테스트 메시지입니다.");
        }
    }
}