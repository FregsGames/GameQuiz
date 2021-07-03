using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Questions;

public class QuestionTests : MonoBehaviour
{
    public TextMeshProUGUI questionText;
    public TextMeshProUGUI answer1ButtonText;
    public TextMeshProUGUI answer2ButtonText;
    public TextMeshProUGUI answer3ButtonText;
    public TextMeshProUGUI answer4ButtonText;

    public GamesDB gamesDB;

    public int year = 2000;

    private QuestionGenerator questionGenerator;

    private void Start()
    {
        questionGenerator = new QuestionGenerator();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.L))
        {
            Question question = questionGenerator.GameFromPlatform(3);

            SetQuestion(question.Statement, question.WrongOptions.ToArray(), question.CorrectAnswer);
        }
    }

    public void SetQuestion(string question, string[] answers, string correct)
    {
        questionText.text = question;
        answer1ButtonText.text = answers[0];
        answer2ButtonText.text = answers[1];
        answer3ButtonText.text = answers[2];
        answer4ButtonText.text = correct;
    }

}
