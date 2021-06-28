using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class QuestionTests : MonoBehaviour
{
    public TextMeshProUGUI questionText;
    public TextMeshProUGUI answer1ButtonText;
    public TextMeshProUGUI answer2ButtonText;
    public TextMeshProUGUI answer3ButtonText;
    public TextMeshProUGUI answer4ButtonText;

    public GamesDB gamesDB;


    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.L))
        {
            List<Game> lists = gamesDB.GetXGamesFromYearX(4, 2006);

            List<string> answers = new List<string>();

            foreach (var item in lists)
            {
                answers.Add(item.name);
            }

            SetQuestion("Games from 2006: ", answers.ToArray());
        }
    }

    public void SetQuestion(string question, string[] answers)
    {
        questionText.text = question;
        answer1ButtonText.text = answers[0];
        answer2ButtonText.text = answers[1];
        answer3ButtonText.text = answers[2];
        answer4ButtonText.text = answers[3];
    }

}
