using Assets.Scripts.Payloads;
using DG.Tweening;
using Questions;
using SuperMaxim.Messaging;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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

    private RectTransform[] buttons;

    [SerializeField]
    private VerticalLayoutGroup questionLayout;

    private Question currentQuestion;

    [SerializeField]
    private RectTransform content;

    [SerializeField]
    private GameTimer timer;

    [SerializeField]
    private EndScreen endScreenPrefab;

    private IAPManager iAPManager;

    [SerializeField]
    private RewardVideoPanel rewardVideoPanel;

    public bool test = false;

    private void Start()
    {
        iAPManager = IAPManager.Instance;
    }

    private async void Awake()
    {
        await Task.Delay(10);

        content.DOAnchorPosX(-content.rect.width * 2, 0);

        buttons = new RectTransform[] { answer1Button.GetComponent<RectTransform>(), answer2Button.GetComponent<RectTransform>(), answer3Button.GetComponent<RectTransform>(), answer4Button.GetComponent<RectTransform>() };

        foreach (RectTransform button in buttons)
        {
            button.DOAnchorPosX(-content.rect.width * 1.5f, 0);
        }

        if (test)
        {
            await Task.Delay(2000);
            Initialize(10);
        }
    }

    public void Initialize(float time)
    {
        var sequence = DOTween.Sequence();

        sequence.SetDelay(1);

        sequence.Append(content.DOAnchorPosX(0, 0.75f).SetEase(Ease.OutBack));

        SetButtonsInteractables(false);

        timer.Setup(sequence, time);

        sequence.OnComplete(() => Messenger.Default.Publish(new UIReadyPayload()));
        LoadAd();
    }

    private void LoadAd()
    {
        if (iAPManager != null && iAPManager.showAds && !AdsManager.Instance.RewardActive())
        {
            AdsManager.Instance.LoadAd();
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
    }

    public void TriggerQuestion()
    {
        var sequence = DOTween.Sequence();

        for (int i = 0; i < buttons.Length; i++)
        {
            AudioManager.Instance.PlaySound(AudioManager.SoundEffect.Enter,  i * 90 + 450);
            sequence.Insert(0, buttons[i].DOAnchorPosX(0, 0.5f + (i * 0.2f)).SetDelay(0.1f).SetEase(Ease.InCubic));
        }
        sequence.OnComplete(OnAnswerShowCompleted);
    }

    private void OnAnswerShowCompleted()
    {
        SetButtonsInteractables(true);

        if (timer.UsesTime)
        {
            timer.TimeQuestion();
        }
    }

    public void ShowResults(bool completed, int correctAnswers, int totalAnswers, bool unlocks)
    {
        ShowAd();

        questionText.text = "";

        timer.FadeOut();

        EnableOptionButtons(false);

        var endScreen = Instantiate(endScreenPrefab);
        endScreen.Setup(completed, correctAnswers, totalAnswers, unlocks);
    }

    private void ShowAd()
    {
        if (iAPManager != null && iAPManager.showAds && !AdsManager.Instance.RewardActive())
        {
            if (AdsManager.Instance.NoAdsPanelDiscarded)
            {
                AdsManager.Instance.ShowAd();
            }
            else
            {
                AdsManager.Instance.LoadRewarded();
                rewardVideoPanel.ShowPanel();
            }
        }
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

    public void OnRestart()
    {
        EnableOptionButtons(true);
        Messenger.Default.Publish(new GameRestartPayload());
    }

    private void OnEnable()
    {
        Messenger.Default.Subscribe<ButtonQuestion>(OnAnswerReceived);   
        Messenger.Default.Subscribe<TimeOut>(OnTimeOut);   
    }


    private void OnDisable()
    {
        Messenger.Default.Unsubscribe<ButtonQuestion>(OnAnswerReceived);
        Messenger.Default.Unsubscribe<TimeOut>(OnTimeOut);
    }
    private void OnTimeOut(TimeOut payload)
    {
        OnAnswerReceived(null);
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

        timer.StopTimer();

        var sequence = DOTween.Sequence();

        if (answer == null)
        {
            sequence.OnComplete(() => TweenOptionsOut(answer));

            Messenger.Default.Publish(new AnswerGiven(false));
        }
        else if (answer.AssignedAnswer != currentQuestion.CorrectAnswer)
        {
            answer.SetColor(ButtonQuestion.State.Wrong);
            sequence.Insert(0, answer.transform.DOShakePosition(0.3f, strength: 100));
            sequence.OnComplete(() => TweenOptionsOut(answer));

            Messenger.Default.Publish(new AnswerGiven(false));

        }
        else
        {
            answer.SetColor(ButtonQuestion.State.Correct);
            sequence.Insert(0, answer.transform.DOPunchScale(Vector3.one * 0.3f, 0.2f, 10, 0.1f));
            sequence.OnComplete(() => TweenOptionsOut(answer));

            Messenger.Default.Publish(new AnswerGiven(true));
        }
    }

    private void TweenOptionsOut(ButtonQuestion answer)
    {
        var sequence = DOTween.Sequence();

        for (int i = 0; i < buttons.Length; i++)
        {
            sequence.Insert(0, buttons[i].DOAnchorPosX(content.rect.width * 1.5f, 1f).SetDelay(i * 0.2f)).SetEase(Ease.InCubic);
        }

        sequence.OnComplete(() => PublishUIFinishedUpdate(answer));
        SetButtonAnswersColors(ButtonQuestion.State.Normal);
    }

    private async void PublishUIFinishedUpdate(ButtonQuestion answer)
    {
        foreach (RectTransform button in buttons)
        {
            button.DOAnchorPosX(-content.rect.width * 1.5f, 0);
        }

        if (timer.UsesTime)
        {
            timer.Restart();
        }

        await Task.Delay(5);

        Messenger.Default.Publish(new AnswerFromUI() { Answer = answer == null? null : answer.AssignedAnswer });
    }
}
