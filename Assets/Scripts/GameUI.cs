using Assets.Scripts.Payloads;
using DG.Tweening;
using Questions;
using SuperMaxim.Messaging;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

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

    [SerializeField]
    private Button restartButton;

    private Transform[] buttons;

    private Question currentQuestion;

    private void Awake()
    {
        buttons = new Transform[] { answer1Button.transform, answer2Button.transform, answer3Button.transform, answer4Button.transform };

        foreach (Transform button in buttons)
        {
            button.DOMoveX(-Screen.width / 2, 0);
        }
    }

    public void SetQuestion(Question question)
    {
        currentQuestion = question;

        questionText.text = question.Statement;

        List<string> options = question.WrongOptions;
        if (options.Count > 3)
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

        for (int i = 0; i < buttons.Length; i++)
        {
            buttons[i].DOMoveX(Screen.width / 2, 1f + (i * 0.2f)).SetDelay(0.1f).SetEase(Ease.InCubic);
        }
    }

    public void ShowResults(int correctAnswers, int totalAnswers)
    {
        EnableOptionButtons(false);
        EnableRestartButton(true);

        questionText.text = $"{correctAnswers}/{totalAnswers}";
    }

    private void EnableOptionButtons(bool enable)
    {
        answer1Button.gameObject.SetActive(enable);
        answer2Button.gameObject.SetActive(enable);
        answer3Button.gameObject.SetActive(enable);
        answer4Button.gameObject.SetActive(enable);
    }

    private void SetButtonsInteractables(bool state)
    {
        answer1Button.SetInteractable(state);
        answer2Button.SetInteractable(state);
        answer3Button.SetInteractable(state);
        answer4Button.SetInteractable(state);
    }

    private void EnableRestartButton(bool enable)
    {
        restartButton.gameObject.SetActive(enable);
    }

    public void OnRestart()
    {
        EnableRestartButton(false);
        EnableOptionButtons(true);
        Messenger.Default.Publish(new GameRestartPayload());
    }

    private void OnEnable()
    {
        Messenger.Default.Subscribe<ButtonQuestion>(OnAnswerReceived);
    }

    private void OnDisable()
    {
        Messenger.Default.Unsubscribe<ButtonQuestion>(OnAnswerReceived);
    }

    private void OnAnswerReceived(ButtonQuestion answer)
    {
        SetButtonsInteractables(false);

        var sequence = DOTween.Sequence();

        if (answer.AssignedAnswer != currentQuestion.CorrectAnswer)
        {
            sequence.Insert(0, answer.transform.DOShakePosition(0.5f, strength: 100));
            sequence.OnComplete(() => TweenOptionsOut(answer));
        }
        else
        {
            TweenOptionsOut(answer);
        }

    }

    private void TweenOptionsOut(ButtonQuestion answer)
    {
        var sequence = DOTween.Sequence();

        for (int i = 0; i < buttons.Length; i++)
        {
            sequence.Insert(0, buttons[i].DOMoveX(Screen.width * 2, 1f).SetDelay(i * 0.2f)).SetEase(Ease.InCubic);
        }

        sequence.OnComplete(() => PublishUIFinishedUpdate(answer.AssignedAnswer));

        //sequence.Play();
    }

    private void PublishUIFinishedUpdate(string answer)
    {
        foreach (Transform button in buttons)
        {
            button.DOMoveX(-Screen.width / 2, 0);
        }
        SetButtonsInteractables(true);

        Messenger.Default.Publish(new AnswerFromUI() { Answer = answer });
    }
}
