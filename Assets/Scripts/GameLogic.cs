using Assets.Scripts.Payloads;
using Questions;
using SuperMaxim.Messaging;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameLogic : MonoBehaviour
{
    private QuestionGenerator questionGenerator;
    private GameUI gameUI;

    private int questionCount;
    private int currentQuestionIndex;
    private Question currentQuestion;

    private int correctAwswers;
    private int totalAwswers = 10;

    void Start()
    {
        questionGenerator = new QuestionGenerator();
        gameUI = FindObjectOfType<GameUI>();

        currentQuestion = questionGenerator.GetRandomGenericQuestion(3);
        gameUI.SetQuestion(currentQuestion);
    }

    private void OnEnable()
    {
        Application.targetFrameRate = 30;

        Messenger.Default.Subscribe<AnswerFromUI>(OnAnswerReceived);
        Messenger.Default.Subscribe<GameRestartPayload>(OnRestart);
        Messenger.Default.Subscribe<UIReadyPayload>(OnUiReady);
    }

    private void OnUiReady(UIReadyPayload obj)
    {
        StartGame(totalAwswers);
    }

    private void OnDisable()
    {
        Messenger.Default.Unsubscribe<AnswerFromUI>(OnAnswerReceived);
        Messenger.Default.Unsubscribe<GameRestartPayload>(OnRestart);
        Messenger.Default.Unsubscribe<UIReadyPayload>(OnUiReady);
    }
    private void OnRestart(GameRestartPayload obj)
    {
        StartGame(totalAwswers);
    }

    private void OnAnswerReceived(AnswerFromUI answer)
    {
        currentQuestionIndex++;

        if(currentQuestion.CorrectAnswer == answer.Answer)
        {
            correctAwswers++;
        }

        if (currentQuestionIndex < questionCount)
        {
            NextQuestion();
        }
        else
        {
            gameUI.ShowResults(correctAwswers, totalAwswers);
        }
    }

    public void StartGame(int questionCount)
    {
        correctAwswers = 0;

        this.questionCount = questionCount;
        currentQuestionIndex = 1;
        gameUI.TriggerQuestion();
    }


    public void NextQuestion()
    {
        currentQuestion = questionGenerator.GetRandomGenericQuestion(3);
        gameUI.SetQuestion(currentQuestion);
        gameUI.TriggerQuestion();
    }

}
