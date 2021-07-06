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

    [SerializeField]
    private GameObject content;

    [SerializeField]
    private GameTimer timer;

    private void Awake()
    {
        buttons = new Transform[] { answer1Button.transform, answer2Button.transform, answer3Button.transform, answer4Button.transform };

        foreach (Transform button in buttons)
        {
            button.DOMoveX(-Screen.width / 2, 0);
        }
    }

    public void Initialize(float time)
    {
        content.transform.DOMoveX(-Screen.width / 2, 0);

        var sequence = DOTween.Sequence();

        sequence.Insert(0, content.transform.DOMoveX(Screen.width / 2, 1f).SetEase(Ease.OutBack));

        timer.Setup(sequence, time);


        sequence.OnComplete(() => Messenger.Default.Publish(new UIReadyPayload()));
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
    }

    public void TriggerQuestion()
    {
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

    private void SetButtonAnswersColors(ButtonQuestion.State state)
    {
        answer1Button.SetColor(state);
        answer2Button.SetColor(state);
        answer3Button.SetColor(state);
        answer4Button.SetColor(state);
    }

    private void OnAnswerReceived(ButtonQuestion answer)
    {
        SetButtonAnswersColors(ButtonQuestion.State.Plain);
        SetButtonsInteractables(false);

        var sequence = DOTween.Sequence();

        if (answer.AssignedAnswer != currentQuestion.CorrectAnswer)
        {
            answer.SetColor(ButtonQuestion.State.Wrong);
            sequence.Insert(0, answer.transform.DOShakePosition(0.3f, strength: 100));
            sequence.OnComplete(() => TweenOptionsOut(answer));
        }
        else
        {
            answer.SetColor(ButtonQuestion.State.Correct);
            sequence.Insert(0, answer.transform.DOPunchScale(Vector3.one * 0.3f, 0.2f, 10, 0.1f));
            sequence.OnComplete(() => TweenOptionsOut(answer));
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
        SetButtonAnswersColors(ButtonQuestion.State.Normal);
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
