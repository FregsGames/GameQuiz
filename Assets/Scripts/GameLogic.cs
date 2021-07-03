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
        Messenger.Default.Subscribe<string>(OnAnswerReceived);
    }

    private void OnDisable()
    {
        Messenger.Default.Unsubscribe<string>(OnAnswerReceived);
    }

    private void OnAnswerReceived(string answer)
    {
        currentQuestionIndex++;

        if(currentQuestion.CorrectAnswer == answer)
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
