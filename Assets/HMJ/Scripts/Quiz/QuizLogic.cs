using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class QuizLogic : MonoBehaviour
{
    public enum QuizState
    {
        QuizReadyState, // 퀴즈 준비중
        QuizStartState, // 퀴즈 시작
        QuizRunState, // 퀴즈 실행중
        QuizOverState, // 퀴즈 끝
        QuizOrderStat_End
    }

    float quizTime = 10.0f;
    List<QuizData> quizList;

    QuizState m_eQuizOrder = QuizState.QuizStartState;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void AddQuizData(QuizData quizData)
    {
        quizList.Add(quizData);
    }

    public void RandomQuiz()
    {

    }

    public void NextQuizState()
    {
        switch (m_eQuizOrder)
        {
            case QuizState.QuizReadyState:
                m_eQuizOrder = QuizState.QuizStartState;
                break;
            case QuizState.QuizStartState:
                m_eQuizOrder = QuizState.QuizRunState;
                break;
            case QuizState.QuizRunState:
                m_eQuizOrder = QuizState.QuizOverState;
                QuizTimeLimit();
                break;
            case QuizState.QuizOverState:
                m_eQuizOrder = QuizState.QuizReadyState;
                break;
            case QuizState.QuizOrderStat_End:
                break;
        }

    }

    public void QuizTimeLimit()
    {
        StartCoroutine(ProceedQuiz(quizTime));
    }

    public IEnumerator ProceedQuiz(float _quizTime)
    {
        yield return new WaitForSeconds(_quizTime);
        NextQuizState(); // 다음 상태로 변경
    }

    public void shuffleQuizList()
    {
        quizList.OrderBy(a => Guid.NewGuid()).ToList();
    }
}
