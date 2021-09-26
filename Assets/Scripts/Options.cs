using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Options : MonoBehaviour
{
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
    }

    public void ShowOptions()
    {
        transform.DOMoveX(-Screen.width, 0);
        gameObject.SetActive(true);
        transform.DOMoveX(0, 0.5f);
    }

    public void HideOptions()
    {
        transform.DOMoveX(-Screen.width, 0.5f).OnComplete(() => gameObject.SetActive(false));
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
