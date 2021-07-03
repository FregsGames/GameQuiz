using Questions;

using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class GameUI : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI questionText;
    [SerializeField]
    private ButtonQuestion answer1Button;
    [SerializeField]
    private ButtonQuestion answer2Button;
    [SerializeField]
    private ButtonQuestion answer3Button;
    [SerializeField]
    private ButtonQuestion answer4Button;

    public void SetQuestion(Question question)
    {
        questionText.text = question.Statement;

        List<string> options = question.WrongOptions;
        if(options.Count > 3)
        {
            options = options.GetRange(0, 3);
        }

        options.Add(question.CorrectAnswer);

        var rnd = new System.Random();

        options = options.OrderBy(item => rnd.Next()).ToList();

        answer1Button.SetQuestion(options[0]);
        answer2Button.SetQuestion(options[1]);
        answer3Button.SetQuestion(options[2]);
        answer4Button.SetQuestion(options[3]);
    }

    public void ShowResults(int correctAnswers, int totalAnswers)
    {
        answer1Button.gameObject.SetActive(false);
        answer2Button.gameObject.SetActive(false);
        answer3Button.gameObject.SetActive(false);
        answer4Button.gameObject.SetActive(false);

        questionText.text = $"{correctAnswers}/{totalAnswers}";
    }


}
