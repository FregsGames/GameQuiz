using Questions;
using System.Collections;
using System.Collections.Generic;
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


    private List<LevelButton> buttons = new List<LevelButton>();

    private Level currentSelectedLevel;

    private QuestionGenerator questionGenerator;


    public bool IsActive { get; set; }

    private void OnEnable()
    {
        levelContainter.SetActive(false);
    }

    private void Start()
    {
        questionGenerator = QuestionGenerator.Instance;
    }

    public void DeactivateSection()
    {
        levelContainter.SetActive(false);
    }

    public void Setup(Cup cup)
    {
        IsActive = true;
        buttons.Clear();

        ClearGrid();
        levelContainter.SetActive(true);

        cupName.text = cup.title;

        for (int i = 0; i < cup.levels.Count; i++)
        {
            var btn = Instantiate(levelButtonPrefab, grid);
            btn.Level = Levels.Instance.GetLevel(cup.levels[i]);
            btn.OnClick = SelectLevel;

            buttons.Add(btn);

            btn.Button.interactable = !LevelIsLocked(cup.levels[i]) || btn.Level.alwaysUnlocked;

            btn.Button.GetComponent<Image>().sprite = LevelIsCompleted(cup.levels[i]) ? completedSprite : normalSprite;
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

        levelName.text = levelButton.Level.levelTitle;
        levelDesc.text = levelButton.Level.levelDesc;
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

    public void Play()
    {
        playButton.interactable = false;
        //disable go back
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

        //generate questions

        List<Question> questions = new List<Question>();

        foreach (var questionTemplate in currentSelectedLevel.questionTemplates)
        {
            questions.Add(questionGenerator.FromTemplate(questionTemplate));
        }


        FindObjectOfType<GameLogic>().StartGame(currentSelectedLevel, questions, 20f);

        SceneManager.UnloadSceneAsync("Lobby");
    }
}
