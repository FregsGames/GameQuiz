using TMPro;
using UnityEngine;

public class EndScreen : MonoBehaviour
{
    [SerializeField]
    private string completedIdText;
    [SerializeField]
    private string failedText;

    [SerializeField]
    private TextMeshProUGUI resultText;
    [SerializeField]
    private TextMeshProUGUI answersText;

    [SerializeField]
    private GameObject newLevelUnlockedGO;

    public void Setup(bool completed, int answers, int total, bool newUnlocks)
    {
        resultText.text = Translations.instance.GetText(completed ? completedIdText : failedText);
        answersText.text = $"{answers}/{total}";
        newLevelUnlockedGO.SetActive(newUnlocks);
    }

    public void LoadLobby()
    {
        SceneLoader.Instance.LoadLobby();
    }
}
