using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using TMPro;
using UnityEngine;
using static GH.DataManager;

public class TutorialText : MonoBehaviour
{
    private TMP_Text tutorialText;
    private float delayTime = 0.05f;

    private bool texting = false;

    void Start()
    {
        tutorialText = GetComponentInChildren<TMP_Text>();
        TextChange(MapType.MyClassroom);
    }

    void Update()
    {

    }

    public void TextChange(MapType mapType)
    {
        switch (mapType)
        {
            case MapType.MyClassroom:
                StartCoroutine(ClassText());
                break;
            case MapType.School:
                StartCoroutine(SchoolText());
                break;
            case MapType.Square:
                StartCoroutine(SquareText());
                break;
            case MapType.QuizSquare:
                StartCoroutine(QuizSquareText());
                break;

        }
    }
    IEnumerator ClassText()
    {
        StartCoroutine(TextPrint("마이리틀스쿨에 입학한 걸 환영해!\r\n나는 로우야!!"));
        yield return new WaitUntil(() => texting && (Input.touchCount == 1 || Input.GetMouseButtonDown(0)));
        StartCoroutine(TextPrint("이곳은 나만의 교실이야! \r\n사물함에는 다른 사람이 방문해서 남긴 쪽지를 볼 수 있어!"));
        yield return new WaitUntil(() => texting && (Input.touchCount == 1 || Input.GetMouseButtonDown(0)));
        gameObject.SetActive(false);
    }
    IEnumerator SchoolText()
    {
        StartCoroutine(TextPrint("이곳은 학교야! 같은 학교 친구들끼리 만날 수 있는 공간이지! \r\n같은 학교 친구들과 이쁜 학교를 만들어봐!"));
        yield return new WaitUntil(() => texting && (Input.touchCount == 1 || Input.GetMouseButtonDown(0)));
        gameObject.SetActive(false);
    }
    IEnumerator SquareText()
    {
        StartCoroutine(TextPrint("이곳은 만남의 광장이야!\r\n모든 친구들을 만날 수 있는 공간이지!\r\n그리고 다양한 놀거리를 즐길수있어!"));
        yield return new WaitUntil(() => texting && (Input.touchCount == 1 || Input.GetMouseButtonDown(0)));
        gameObject.SetActive(false);
    }
    IEnumerator QuizSquareText()
    {
        StartCoroutine(TextPrint("이곳은 퀴즈 광장이야!\r\nOX퀴즈나 골든벨을 할 수 있어!"));
        yield return new WaitUntil(() => texting && (Input.touchCount == 1 || Input.GetMouseButtonDown(0)));
        gameObject.SetActive(false);
    }

    // 텍스트 하나씩 생기게 하기
    IEnumerator TextPrint(string text)
    {
        texting = false;
        tutorialText.text = "";
        int count = 0;

        while (count != text.Length)
        {
            if (count < text.Length)
            {
                tutorialText.text += text[count].ToString();
                count++;
            }
            yield return new WaitForSeconds(delayTime);
        }

        if (count == text.Length)
        {
            texting = true;
        }
        yield return new WaitUntil(() => texting && (Input.touchCount == 1 || Input.GetMouseButtonDown(0)));
    }
}
