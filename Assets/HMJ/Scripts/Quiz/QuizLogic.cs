using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


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
        QuizOrderStat_End
    }
    public TMP_Text text;

    int idx = 0;
    int quizN = 5;

    float quizTime = 10.0f;
    bool quizClear = false;

    public List<QuizData> quizList;


    QuizState m_eNextQuizOrder = QuizState.QuizNoneState;
    QuizState m_eCurQuizOrder = QuizState.QuizReadyState;


    private void Update()
    {
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

    /// <summary>
    /// 퀴즈 상태 변환
    /// </summary>
    public void NextQuizState()
    {
        if (m_eCurQuizOrder == m_eNextQuizOrder)
            return;
        Debug.Log("상태 변환: " + m_eCurQuizOrder.ToString());
        switch (m_eCurQuizOrder)
        {
            case QuizState.QuizReadyState:
                QuizReady();
                break;
            case QuizState.QuizRunState:
                QuizTimeLimit();
                break;
            case QuizState.QuizOverState:
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
        idx %= quizN;
        text.text = quizList[idx].quizText;
        StartCoroutine(ProceedQuiz(quizTime));
        quizClear = true;
    }

    public void QuizReady()
    {
        text.text = "잠시 후 퀴즈가 시작됩니다.\n";
        StartCoroutine(ReadyStart(3.0f));
        m_eNextQuizOrder = QuizState.QuizRunState;
    }

    public IEnumerator ProceedQuiz(float _quizTime)
    {
        yield return new WaitForSeconds(_quizTime);
        m_eNextQuizOrder = QuizState.QuizOverState;
    }

    public IEnumerator ReadyStart(float _readyTime)
    {
        yield return new WaitForSeconds(_readyTime);
    }

    /// <summary>
    /// 랜덤으로 섞기
    /// </summary>
    public void shuffleQuizList()
    {
        quizList.OrderBy(a => Guid.NewGuid()).ToList();
    }
}
