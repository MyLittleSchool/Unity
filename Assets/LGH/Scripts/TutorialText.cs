using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using static GH.DataManager;

public class TutorialText : MonoBehaviour
{
    private TMP_Text tutorialText;
    private string mapTutorialText;
    private float delayTime = 0.075f;

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
                
                break;
            case MapType.School:
                break;
            case MapType.Square:
                break;
            case MapType.QuizSquare:
                break;

        }
    }
    IEnumerator ClassText()
    {
        mapTutorialText = "마이리틀스쿨에 입학한 걸 환영해!\r\n나는 로우야!!";
        StartCoroutine(TextPrint(mapTutorialText));
        yield return new WaitUntil(() => Input.touchCount == 1 || Input.GetMouseButtonDown(0));
        mapTutorialText = "이곳은 나만의 교실이야! \r\n사물함에는 다른 사람이 방문해서 남긴 쪽지를 볼 수 있어!";
        StartCoroutine(TextPrint(mapTutorialText));
    }




    // 텍스트 하나씩 생기게 하기
    IEnumerator TextPrint(string text)
    {
        tutorialText.text = "";
        int count = 0;

        while(count != text.Length)
        {
            if(count < text.Length)
            {
                tutorialText.text += text[count].ToString();
                count++;
            }
            yield return new WaitForSeconds(delayTime);
        }
    }
}
