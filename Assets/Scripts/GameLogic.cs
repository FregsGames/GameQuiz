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

        StartGame(totalAwswers);
    }
    private void OnEnable()
    {
        Application.targetFrameRate = 30;

        Messenger.Default.Subscribe<AnswerFromUI>(OnAnswerReceived);
        Messenger.Default.Subscribe<GameRestartPayload>(OnRestart);
    }

    private void OnDisable()
    {
        Messenger.Default.Unsubscribe<AnswerFromUI>(OnAnswerReceived);
        Messenger.Default.Unsubscribe<GameRestartPayload>(OnRestart);
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
        NextQuestion();
    }

    public void NextQuestion()
    {
        currentQuestion = questionGenerator.GetRandomGenericQuestion(3);
        gameUI.SetQuestion(currentQuestion);
    }

}
