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

    public void OnSoundButtonClick()
    {
        AudioManager.instance.ToggleSound();
        UpdateButtonSprites();
    }
    public void OnMusicButtonClick()
    {
        AudioManager.instance.ToggleMusic();
        UpdateButtonSprites();
    }

    private void UpdateButtonSprites()
    {
        soundButton.image.sprite = AudioManager.instance.SoundMuted ? muteSoundSprite : openSoundSprite;
        musicButton.image.sprite = AudioManager.instance.MusicMuted ? muteMusicSprite : openMusicSprite;
    }
}
