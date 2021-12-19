using Assets.Scripts.Payloads;
using DG.Tweening;
using Questions;
using SuperMaxim.Messaging;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using static Cups;
using static Levels;

public class LevelSelectionScreen : MonoBehaviour
{
    [SerializeField]
    private GameObject levelContainter;
    [SerializeField]
    private Transform grid;
    [SerializeField]
    private LevelButton levelButtonPrefab;
    [SerializeField]
    private Button playButton;

    [SerializeField]
    private TextMeshProUGUI cupName;
    [SerializeField]
    private TextMeshProUGUI levelName;
    [SerializeField]
    private TextMeshProUGUI levelDesc;

    [SerializeField]
    private Sprite normalSprite;
    [SerializeField]
    private Sprite completedSprite;
    [SerializeField]
    private Sprite selectedSprite;
    [SerializeField]
    private Sprite completedSelectedSprite;

    [SerializeField]
    private Ease ease;


    private List<LevelButton> buttons = new List<LevelButton>();

    private LevelScriptableC currentSelectedLevel;

    private QuestionGenerator questionGenerator;

    private CupScriptable currentCup;


    public bool IsActive { get { return levelContainter.activeSelf; } }

    private void OnEnable()
    {
        levelContainter.SetActive(false);
    }

    private void Start()
    {
        questionGenerator = QuestionGenerator.Instance;
        levelContainter.transform.DOMove(new Vector2(-Screen.width, 0), 0f).SetEase(ease);
        Messenger.Default.Subscribe<CupSelectedPayload>(OnCupSelected);
    }

    private async void OnCupSelected(CupSelectedPayload obj)
    {
        if(obj == null)
        {
            await levelContainter.transform.DOMove(new Vector2(-Screen.width, 0), 0.5f).SetEase(ease).AsyncWaitForCompletion();
            levelContainter.SetActive(false);
        }
        else if(!obj.endless)
        {
            Setup(obj.Cup);
            await levelContainter.transform.DOMove(Vector2.zero, 0.5f).SetEase(ease).AsyncWaitForCompletion();
        }
    }

    public void DeactivateSection()
    {
        levelContainter.SetActive(false);
    }

    public void Setup(CupScriptable cup)
    {
        buttons.Clear();

        currentCup = cup;

        ClearGrid();
        levelContainter.SetActive(true);

        cupName.text = Translations.instance.GetText(cup.title);

        int completedLevels = 0;

        for (int i = 0; i < cup.levels.Count; i++)
        {

            var btn = Instantiate(levelButtonPrefab, grid);
            btn.Level = cup.levels[i];
            btn.OnClick = SelectLevel;

            buttons.Add(btn);

            var loadedState = LevelIsCompleted(cup.levels[i].id) ? LevelState.completed : LevelIsLocked(cup.levels[i].id) ? LevelState.locked : LevelState.unlocked;

            cup.levels[i].state = loadedState;
            btn.Button.interactable = !LevelIsLocked(cup.levels[i].id) || btn.Level.alwaysUnlocked;

            if(cup.levels[i].state == LevelState.completed)
            {
                completedLevels++;
            }

            btn.Button.GetComponent<Image>().sprite = LevelIsCompleted(cup.levels[i].id) ? completedSprite : normalSprite;
        }

        buttons[0].OnClick.Invoke(buttons[0]);
    }

    private static bool LevelIsLocked(string levelID)
    {
        return SaveManager.instance.RetrieveInt(levelID) == (int)LevelState.locked;
    }

    private static bool LevelIsCompleted(string levelID)
    {
        return SaveManager.instance.RetrieveInt(levelID) == (int)LevelState.completed;
    }

    public void SelectLevel(LevelButton levelButton)
    {
        currentSelectedLevel = levelButton.Level;
        SetAllButtonsToNormalColor();

        levelButton.SetSprite(levelButton.Level.state == LevelState.completed ? completedSelectedSprite : selectedSprite);

        levelName.text = Translations.instance.GetText(levelButton.Level.levelTitle);
        SetLevelWinCondition(levelButton);
    }

    private void SetLevelWinCondition(LevelButton levelButton)
    {
        if(levelButton.Level.state == LevelState.completed)
        {
            levelDesc.text = $"\n Level completed!";
            return;
        }

        switch (levelButton.Level.winCondition)
        {
            case LevelCondition.half:
                string translatedTextPart1 = Translations.instance.GetText("LevelCondition_Custom_Part1");
                string translatedTextPart12 = Translations.instance.GetText("LevelCondition_Custom_Part2");
                int count = Mathf.CeilToInt(levelButton.Level.questionTemplates.Count / 2f);
                levelDesc.text = $"{translatedTextPart1} {count} {translatedTextPart12}";
                break;
            case LevelCondition.full:
                levelDesc.text = $"{Translations.instance.GetText("LevelCondition_All")}";
                break;
            case LevelCondition.one:
                levelDesc.text = $"{Translations.instance.GetText("LevelCondition_One")}";
                break;
        }
    }

    private void SetAllButtonsToNormalColor()
    {
        foreach (var btn in buttons)
        {
            btn.SetSprite(btn.Level.state == LevelState.completed ? completedSprite : normalSprite);
        }
    }

    private void ClearGrid()
    {
        for (int i = grid.childCount - 1; i >= 0; i--)
        {
            Destroy(grid.GetChild(i).gameObject);
        }
    }

    public async void Play()
    {
        playButton.interactable = false;
        //disable go back
        await levelContainter.transform.DOMove(new Vector2(Screen.width, 0), 0.5f).SetEase(Ease.InOutBack).AsyncWaitForCompletion();
        StartCoroutine(LoadGame());
    }

    IEnumerator LoadGame()
    {
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync("Game", LoadSceneMode.Additive);
        while (!asyncLoad.isDone)
        {
            yield return null;
        }
        yield return null;

        if (currentSelectedLevel.handwrittenQuestionSet != null)
        {
            currentSelectedLevel.handwrittenQuestionSet.Initialize();
        }
        List<Question> questions = GenerateQuestions();


        FindObjectOfType<GameLogic>().StartGame(currentSelectedLevel, questions, 20f);

        SceneManager.UnloadSceneAsync("CupSelection");
    }

    public List<Question> GenerateQuestions()
    {
        List<Question> questions = new List<Question>();

        List<string> toExclude = new List<string>();

        int i = 0;

        foreach (var questionTemplate in currentSelectedLevel.questionTemplates)
        {
            Question question = null;

            if (questionTemplate.ContentType == QuestionTemplate.QuestionContent.handwriten)
            {

                question = currentSelectedLevel.handwrittenQuestionSet.GetQuestion(toExclude);
                toExclude.Add(question.Id);
            }
            else
            {
                int tries = 0;
                while ((question == null || (questions.FirstOrDefault(q => q.CorrectAnswer == question.CorrectAnswer) != null)) && tries < 20)
                {
                    question = questionGenerator.FromTemplate(questionTemplate, currentSelectedLevel.filters, toExclude);
                    tries++;
                }

                if(question == null)
                {
                    Debug.LogError($"Couldn't get a question from template: {questionTemplate.ContentType}, generating generic.");
                    question = questionGenerator.GetRandomGenericQuestion(currentSelectedLevel.filters);
                }

            }
            i++;
            questions.Add(question);
            toExclude.Add(question.Comparer);
        }

        return questions;
    }

    public void Back()
    {
        Messenger.Default.Publish<CupSelectedPayload>(null);
    }
}
