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


public class QuizLogic : MonoBehaviour
{

    [Serializable]
    public struct QuizData
    {
        public string quizText;
        public bool quizBoolean;
    }

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
    int quizN = 5;

    float quizTime = 5.0f;
    bool quizClear = false;

    int minimumPlayer = 2;

    public List<QuizData> quizList;


    QuizState m_eNextQuizOrder = QuizState.QuizNoneState;
    QuizState m_eCurQuizOrder = QuizState.QuizNoneState;

    GameObject player;

    List<int> correctAnswers;

    private void Start()
    {
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
        quizList.Add(quizData);
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
        if(m_eNextQuizOrder == QuizState.QuizNoneState)
        {
            if(PhotonNetwork.CurrentRoom.PlayerCount >= minimumPlayer)
                m_eNextQuizOrder = QuizState.QuizReadyState;
            else
            {
                text.text = minimumPlayer.ToString() + "인 이상 모여야 퀴즈가 시작됩니다.";
            }
        }
    }

    /// <summary>
    /// 퀴즈 상태 변환
    /// </summary>
    public void NextQuizState()
    {
        if (m_eCurQuizOrder == m_eNextQuizOrder)
            return;
        Debug.Log("상태 변환: " + m_eCurQuizOrder.ToString());
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
        text.text = quizList[idx].quizText;

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
        Debug.Log("플레이어 x:" + player.transform.position.x);
        if (((player.transform.position.x > 0) && !quizList[idx].quizBoolean) || ((player.transform.position.x < 0) && quizList[idx].quizBoolean))// x
            quizClear = true;
        else
            quizClear = false;
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
        yield return new WaitForSeconds(_lastTime);
        text.text = "";
    }
    /// <summary>
    /// 랜덤으로 섞기
    /// </summary>
    public void shuffleQuizList()
    {
        quizList.OrderBy(a => Guid.NewGuid()).ToList();
    }
}
