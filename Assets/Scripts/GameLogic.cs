using Assets.Scripts.Payloads;
using Questions;
using SuperMaxim.Messaging;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Levels;

public class GameLogic : MonoBehaviour
{
    private GameUI gameUI;

    private int questionCount;
    private int currentQuestionIndex;
    private Question currentQuestion;

    private int correctAwswers;
    private int totalQuestions = 10;

    [Header("Timer")]
    [SerializeField]
    private float timePerQuestion;

    private List<Question> questions;
    private Level currentLevel;

    void Start()
    {
        gameUI = FindObjectOfType<GameUI>();

        /*currentQuestion = questionGenerator.GetRandomGenericQuestion(3);
        gameUI.Initialize(timePerQuestion);

        gameUI.SetQuestion(currentQuestion);*/
    }

    public void StartGame(Level level, List<Question> questions, float timePerQuestion = 20f)
    {
        currentLevel = level;
        this.questions = questions;
        currentQuestionIndex = 0;

        totalQuestions = questions.Count;
        currentQuestion = questions[0];
        gameUI.Initialize(timePerQuestion);

        gameUI.SetQuestion(currentQuestion);
    }

    private void OnEnable()
    {
        Application.targetFrameRate = 0;

        Messenger.Default.Subscribe<AnswerFromUI>(OnAnswerReceived);
        Messenger.Default.Subscribe<GameRestartPayload>(OnRestart);
        Messenger.Default.Subscribe<UIReadyPayload>(OnUiReady);
    }

    private void OnUiReady(UIReadyPayload obj)
    {
        StartGame(totalQuestions);
    }

    private void OnDisable()
    {
        Messenger.Default.Unsubscribe<AnswerFromUI>(OnAnswerReceived);
        Messenger.Default.Unsubscribe<GameRestartPayload>(OnRestart);
        Messenger.Default.Unsubscribe<UIReadyPayload>(OnUiReady);
    }
    private void OnRestart(GameRestartPayload obj)
    {
        SceneLoader.instance.LoadLobby();
    }

    private void OnAnswerReceived(AnswerFromUI answer)
    {
        currentQuestionIndex++;

        if(answer != null && currentQuestion.CorrectAnswer == answer.Answer)
        {
            correctAwswers++;
        }

        if (currentQuestionIndex < questionCount)
        {
            NextQuestion();
        }
        else
        {
            gameUI.ShowResults(correctAwswers, totalQuestions);
            CheckUnlocks(correctAwswers, totalQuestions);
        }
    }

    private void CheckUnlocks(int correctAnswers, int totalQuestions)
    {
        switch (currentLevel.winCondition)
        {
            case LevelCondition.half:
                if(correctAnswers > totalQuestions / 2)
                {
                    SendUnlockMessage();
                }
                break;
            case LevelCondition.full:
                if (correctAnswers == totalQuestions)
                {
                    SendUnlockMessage();
                }
                break;
            case LevelCondition.one:
                if(correctAnswers > 0)
                {
                    SendUnlockMessage();
                }
                break;
            default:
                break;
        }
    }

    private void SendUnlockMessage()
    {
        Messenger.Default.Publish(new UnlockLevelPayload(currentLevel.id));
    }

    public void StartGame(int questionCount)
    {
        correctAwswers = 0;

        this.questionCount = questionCount;
        currentQuestionIndex = 0;
        gameUI.TriggerQuestion();
    }


    public void NextQuestion()
    {
        currentQuestion = questions[currentQuestionIndex];
        gameUI.SetQuestion(currentQuestion);
        gameUI.TriggerQuestion();
    }

}
