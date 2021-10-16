using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Options : MonoBehaviour
{
    [SerializeField]
    private bool overlapOptions;
    [SerializeField]
    private TextMeshProUGUI langText;

    [SerializeField]
    private MenuManager menuManager;
    [SerializeField]
    private Transform content;
    [SerializeField]
    private GameObject background;

    [Header("Sound")]
    [SerializeField]
    private Button soundButton;
    [SerializeField]
    private Sprite muteSoundSprite;
    [SerializeField]
    private Sprite openSoundSprite;

    [Header("Music")]
    [SerializeField]
    private Button musicButton;
    [SerializeField]
    private Sprite muteMusicSprite;
    [SerializeField]
    private Sprite openMusicSprite;

    private void Start()
    {
        UpdateButtonSprites();
        langText.text = Translations.instance.currentLanguage.ToString();
    }

    public async void ShowOptions()
    {
        if (!overlapOptions)
        {
            content.DOMoveX(-Screen.width, 0);
            await Task.Delay(5);
            gameObject.SetActive(true);
            _ = menuManager.AnimatePanel();
            content.DOMoveX(0, 0.5f).SetEase(Ease.InOutBack);
        }
        else
        {
            content.DOMoveX(0, 0f);
            background.SetActive(true);
            gameObject.SetActive(true);
        }
    }

    public void HideOptions()
    {
        if (!overlapOptions)
        {
            content.DOMoveX(-Screen.width, 0.5f).SetEase(Ease.InOutBack).OnComplete(() => gameObject.SetActive(false));
            _ = menuManager.AnimatePanelIn();
        }
        else
        {
            content.DOMoveX(-Screen.width, 0);
            gameObject.SetActive(false);
            background.SetActive(false);
        }
    }

    public void NextLang()
    {
        Translations.instance.SetNextLanguage();
        langText.text = Translations.instance.currentLanguage.ToString();
    }

    public void PrevLang()
    {
        Translations.instance.SetPrevLanguage();
        langText.text = Translations.instance.currentLanguage.ToString();
    }

    public void GoMenu()
    {
        SceneLoader.Instance.LoadLobby();
    }

    public void OnSoundButtonClick()
    {
        AudioManager.Instance.ToggleSound();
        UpdateButtonSprites();
    }
    public void OnMusicButtonClick()
    {
        AudioManager.Instance.ToggleMusic();
        UpdateButtonSprites();
    }

    private void UpdateButtonSprites()
    {
        soundButton.image.sprite = AudioManager.Instance.SoundMuted ? muteSoundSprite : openSoundSprite;
        musicButton.image.sprite = AudioManager.Instance.MusicMuted ? muteMusicSprite : openMusicSprite;
    }
}
