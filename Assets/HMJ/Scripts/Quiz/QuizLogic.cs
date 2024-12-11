using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using GH;
using Photon.Pun;
using System.Numerics;
using static QuizCategory;

[Serializable]
public class QuizData
{
    public string quizText;
    public bool quizBoolean;
}

[Serializable]
public class QuizDataGroup
{
    public List<QuizData> quizDatas;
}
public class QuizLogic : MonoBehaviour
{
    public List<QuizDataGroup> quizList;

    public enum QuizState
    {
        QuizNoneState, // 
        QuizReadyState, // 퀴즈 준비중
        QuizRunState, // 퀴즈 실행중
        QuizOverState, // 퀴즈 끝
        QuizLastState, // 퀴즈 끝
        QuizOrderStat_End
    }
    public TMP_Text text;

    int idx = 0;
    int quizN = 3;

    float quizTime = 4.0f;
    bool quizClear = false;

    int minimumPlayer = 2;


    QuizState m_eNextQuizOrder = QuizState.QuizNoneState;
    QuizState m_eCurQuizOrder = QuizState.QuizNoneState;

    GameObject player;

    PhotonView pv;

    Dictionary<string, int> correctAnswers = new Dictionary<string, int>();

    string winnerData;

    QUIZCATEGORY curQuizCategory;

    private void Start()
    {
        pv = GetComponent<PhotonView>();
    }
    private void Update()
    {
        StartQuiz();
        NextQuizState();
    }
    /// <summary>
    /// 퀴즈 추가하기 - 코드로 추가할 경우
    /// </summary>
    /// <param name="quizData"></param>
    public void AddQuizData(QuizData quizData)
    {
        quizList[(int)curQuizCategory].quizDatas.Add(quizData);
    }

    /// <summary>
    /// 퀴즈 섞기
    /// </summary>
    public void RandomQuiz()
    {
        shuffleQuizList();
    }

    public void StartQuiz()
    {
        curQuizCategory = GH.DataManager.instance.curQuizChannel;
        if (m_eNextQuizOrder == QuizState.QuizNoneState)
        {
            if (PhotonNetwork.CurrentRoom != null && PhotonNetwork.CurrentRoom.PlayerCount >= minimumPlayer)
                m_eNextQuizOrder = QuizState.QuizReadyState;
            else
                text.text = minimumPlayer.ToString() + "인 이상 모여야 퀴즈가 시작됩니다.";
        }
    }

    /// <summary>
    /// 퀴즈 상태 변환
    /// </summary>
    public void NextQuizState()
    {
        if (m_eCurQuizOrder == m_eNextQuizOrder)
            return;
        switch (m_eNextQuizOrder)
        {
            case QuizState.QuizReadyState:
                QuizReady();
                break;
            case QuizState.QuizRunState:
                QuizTimeLimit();
                break;
            case QuizState.QuizOverState:
                QuizOverLimit();
                break;
            case QuizState.QuizLastState:
                QuizEnd();
                break;
            case QuizState.QuizOrderStat_End:
                break;
        }
        m_eCurQuizOrder = m_eNextQuizOrder;
    }

    /// <summary>
    /// 퀴즈 제한 시간까지 코루틴 후 다음 스테이트로 넘어가기
    /// </summary>
    public void QuizTimeLimit()
    {
        text.text = quizList[(int)curQuizCategory].quizDatas[idx].quizText;

        StartCoroutine(ProceedQuiz(quizTime));
    }

    public void QuizReady()
    {
        text.text = "잠시 후 퀴즈가 시작됩니다.\n";
        StartCoroutine(ReadyStart(3.0f));
    }

    public void QuizOverLimit()
    {
        if (quizClear)
            text.text = "정답입니다.";
        else
            text.text = "정답이 아닙니다.";
        StartCoroutine(QuizOver(3.0f));
    }

    public void QuizEnd()
    {
        text.text = "퀴즈가 종료되었습니다.";
        StartCoroutine(QuizLast(3.0f));
    }

    public void QuizClearCheck()
    {
        GameObject player = DataManager.instance.player;
        if (((player.transform.position.x > 0) && !quizList[(int)curQuizCategory].quizDatas[idx].quizBoolean) || ((player.transform.position.x < 0) && quizList[(int)curQuizCategory].quizDatas[idx].quizBoolean))// x
        {
            quizClear = true;
            SendMasterQuizClearCheck();
        }   
        else
            quizClear = false;

    }
    
    public void SendMasterQuizClearCheck()
    {
        pv.RPC("SendQuizClearCheck", RpcTarget.MasterClient, DataManager.instance.playerName);
    }

    [PunRPC]
    public void SendQuizClearCheck(string _playerName)
    {
        if (correctAnswers.ContainsKey(_playerName))
            correctAnswers[_playerName]++;
        else
            correctAnswers.Add(_playerName, 1);
    }

    public IEnumerator ProceedQuiz(float _quizTime)
    {
        yield return new WaitForSeconds(_quizTime);

        QuizClearCheck();
        idx++;
        idx %= quizN;

        m_eNextQuizOrder = QuizState.QuizOverState;
    }

    public IEnumerator ReadyStart(float _readyTime)
    {
        yield return new WaitForSeconds(_readyTime);
        m_eNextQuizOrder = QuizState.QuizRunState;
    }

    public IEnumerator QuizOver(float _overTime)
    {
        yield return new WaitForSeconds(_overTime);
        if(idx + 1 >= quizN)
            m_eNextQuizOrder = QuizState.QuizLastState;
        else
            m_eNextQuizOrder = QuizState.QuizRunState;

    }

    public IEnumerator QuizLast(float _lastTime)
    {
        // 우승자 rpc로 뿌리기
        if (PhotonNetwork.IsMasterClient)
        {
            SettingWinnerData();
            SendAllQuizWinnerData();
        }
        yield return new WaitForSeconds(_lastTime);

        text.text = winnerData;
    }

    public void SendAllQuizWinnerData()
    {
        pv.RPC("SendWinnerData", RpcTarget.Others, winnerData);
    }

    [PunRPC]
    public void SendWinnerData(string _winnerData)
    {
        winnerData = _winnerData;
    }

    public void SettingWinnerData()
    {
        List<KeyValuePair<String, int>> keyValueDataList = FindMaxPlayerKeyValue();

        foreach (KeyValuePair<String, int> item in keyValueDataList)
        {
            winnerData += item.Key + "님 (" + item.Value + "번 정답)\n";
        }
        winnerData += "최종 퀴즈 우승하셨습니다.";
    }

    
    public List<KeyValuePair<String, int>> FindMaxPlayerKeyValue()
    {
        int max = 0;
        List<KeyValuePair<String, int>> itemData = new List<KeyValuePair<String, int>>();
        foreach (KeyValuePair<String, int> item in correctAnswers)
        {
            if(max < item.Value)
            {
                max = item.Value;
                itemData.Clear();
                itemData.Add(item);
            }
            else if(max == item.Value && max != 0)
                itemData.Add(item);
        }
        return itemData;
    }
    /// <summary>
    /// 랜덤으로 섞기
    /// </summary>
    public void shuffleQuizList()
    {
        quizList.OrderBy(a => Guid.NewGuid()).ToList();
    }

    public void SelectQuiz(QUIZCATEGORY _quizCategory)
    {
        curQuizCategory = _quizCategory;
    }
}
