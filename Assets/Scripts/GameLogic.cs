﻿using Assets.Scripts.Payloads;
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

    private int correctAnswers;
    private int totalQuestions = 10;

    private bool endless = false;

    [Header("Timer")]
    [SerializeField]
    private float timePerQuestion;

    private List<Question> questions;
    private LevelScriptableC currentLevel;
    private CupScriptable currentCup;

    void Start()
    {
        gameUI = FindObjectOfType<GameUI>();
    }

    public void StartGame(LevelScriptableC level, List<Question> questions, CupScriptable currentCup, float timePerQuestion = 20f)
    {
        endless = false;

        currentLevel = level;
        this.questions = questions;
        currentQuestionIndex = 0;

        totalQuestions = questions.Count;
        currentQuestion = questions[0];
        gameUI.Initialize(timePerQuestion);

        gameUI.SetQuestion(currentQuestion);

        this.currentCup = currentCup;
    }

    public void StartEndless(CupScriptable cup)
    {
        currentCup = cup;
        totalQuestions = 0;
        endless = true;
        gameUI.Initialize(0);

        currentQuestion = QuestionGenerator.Instance.GetRandomGenericQuestion(currentCup.infiniteFilters);

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
        SceneLoader.Instance.LoadLobby();
    }

    private void OnAnswerReceived(AnswerFromUI answer)
    {
        currentQuestionIndex++;

        if(answer != null && currentQuestion.CorrectAnswer == answer.Answer)
        {
            correctAnswers++;
            if (currentQuestion.Handwritten)
            {
                SaveManager.instance.Save(currentQuestion.Id, 1); // Set as answered
            }
        }

        if (currentQuestionIndex < questionCount || endless)
        {
            NextQuestion();
        }
        else
        {
            bool levelCompleted = CheckWinCondition(correctAnswers, totalQuestions);
            bool unlocks = levelCompleted? Cups.Instance.CheckUnlocks(currentLevel) : false;
            gameUI.ShowResults(levelCompleted, correctAnswers, totalQuestions, currentCup, currentLevel);
        }
    }

    private bool CheckWinCondition(int correctAnswers, int totalQuestions)
    {
        switch (currentLevel.winCondition)
        {
            case LevelCondition.half:
                if (this.correctAnswers >= Mathf.CeilToInt(totalQuestions / 2f))
                {
                    return true;
                }
                break;
            case LevelCondition.full:
                if (this.correctAnswers == totalQuestions)
                {
                    return true;
                }
                break;
            case LevelCondition.one:
                if (this.correctAnswers > 0)
                {
                    return true;
                }
                break;
            default:
                return false;
        }
        return false;
    }

    public void StartGame(int questionCount)
    {
        correctAnswers = 0;

        this.questionCount = questionCount;
        currentQuestionIndex = 0;
        gameUI.TriggerQuestion();
    }


    public void NextQuestion()
    {
        if (endless)
        {
            currentQuestion = QuestionGenerator.Instance.GetRandomGenericQuestion(currentCup.infiniteFilters);

            gameUI.SetQuestion(currentQuestion);
            gameUI.TriggerQuestion();
        }
        else
        {
            currentQuestion = questions[currentQuestionIndex];
            gameUI.SetQuestion(currentQuestion);
            gameUI.TriggerQuestion();
        }
        
    }

}
