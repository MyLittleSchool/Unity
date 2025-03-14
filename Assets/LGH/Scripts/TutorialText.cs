using GH;
using Photon.Pun;
using SW;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using static GH.DataManager;
using static HttpManager;
[System.Serializable]
public struct MapCount
{
    public int mapId;
    public string mapType;
    public int userId;
    public int count;
}
public class TutorialText : MonoBehaviour
{
    private TMP_Text tutorialText;
    private float delayTime = 0.05f;

    private bool texting = false;

    public TutorialManager tutorialManager;

    void Start()
    {
        tutorialText = GetComponentInChildren<TMP_Text>();
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
                StartCoroutine(TextPrintDone("이곳은 학교야! 같은 학교 친구들끼리 만날 수 있는 공간이지! \r\n같은 학교 친구들과 이쁜 학교를 만들어봐!"));
                break;
            case MapType.Square:
                StartCoroutine(TextPrintDone("이곳은 만남의 광장이야!\r\n모든 친구들을 만날 수 있는 공간이지!\r\n그리고 다양한 놀거리를 즐길수있어!"));
                break;
            case MapType.QuizSquare:
                StartCoroutine(TextPrintDone("이곳은 퀴즈 광장이야!\r\nOX퀴즈나 골든벨을 할 수 있어!"));
                break;
        }
    }
    IEnumerator ClassText()
    {
        yield return new WaitUntil(() => tutorialManager.lawStart);
        yield return null;
        StartCoroutine(TextPrint("마이리틀스쿨에 입학한 걸 환영해!\r\n나는 로우야!!\r\n새로운 친구들을 사귈 수 있게 도와주고 있어!"));
        yield return new WaitUntil(() => texting && (Input.touchCount == 1 || Input.GetMouseButtonDown(0)));
        yield return null;
        StartCoroutine(TextPrintDone("처음에 어떻게 할 지 모르겠으면 내 정보창에서 퀘스트를 확인해봐!!\r\n그럼 어느 중학교에 다니고 있어??"));
    }

    // 텍스트 하나씩 생기게 하기
    IEnumerator TextPrintDone(string text)
    {
        SoundManager.instance.InteractMainEftSound(SoundManager.EInteractEftType.WRITE_DATA);

        yield return new WaitUntil(() => { return PhotonNetwork.InRoom; });
        yield return new WaitForSeconds(0.3f);

        int count = 0;
        tutorialText.text = "";

        while (count != text.Length)
        {
            if (count < text.Length)
            {
                tutorialText.text += text[count].ToString();
                count++;
                if (Input.touchCount == 1 || Input.GetMouseButtonDown(0))
                {
                    tutorialText.text = text;
                    count = text.Length;
                    break;
                }
            }
            yield return new WaitForSeconds(delayTime);
        }

        if (count == text.Length)
        {
            SoundManager.instance.StopEftSound();
            yield return new WaitUntil(() => Input.touchCount == 1 || Input.GetMouseButtonDown(0));
            tutorialText.text = "";
            gameObject.SetActive(false);
        }

    }
    // 텍스트 하나씩 생기게 하기
    IEnumerator TextPrint(string text)
    {
        SoundManager.instance.InteractMainEftSound(SoundManager.EInteractEftType.WRITE_DATA);

        yield return new WaitUntil(() => { return PhotonNetwork.InRoom; });
        yield return new WaitForSeconds(0.3f);
        int count = 0;
        texting = false;
        tutorialText.text = "";

        while (count != text.Length)
        {
            if (count < text.Length)
            {
                tutorialText.text += text[count].ToString();
                count++;
                if (Input.touchCount == 1 || Input.GetMouseButtonDown(0))
                {
                    tutorialText.text = text;
                    count = text.Length;
                    break;
                }
            }
            yield return new WaitForSeconds(delayTime);
        }

        if (count == text.Length)
        {
            texting = true;
            SoundManager.instance.StopEftSound();
        }
    }

 
}
