using Assets.Scripts.Payloads;
using SuperMaxim.Messaging;
using System;
using UnityEngine;

public class AudioManager : Singleton<AudioManager>
{
    [SerializeField]
    private AudioSource soundAudioSource;
    [SerializeField]
    private AudioSource musicAudioSource;

    [SerializeField]
    private AudioClip accept;
    [SerializeField]
    private AudioClip back;
    [SerializeField]
    private AudioClip win;
    [SerializeField]
    private AudioClip fail;
    [SerializeField]
    private AudioClip correct;
    [SerializeField]
    private AudioClip win2;

    private SaveManager saveManager;

    public enum SoundEffect
    {
        Accept,
        Back,
        Win,
        Fail,
        Correct,
        WinAlt
    }

    private void OnEnable()
    {
        Messenger.Default.Subscribe<AnswerGiven>(OnAnswerReceived);
    }

    private void OnDisable()
    {
        Messenger.Default.Unsubscribe<AnswerGiven>(OnAnswerReceived);
    }
    private void OnAnswerReceived(AnswerGiven answer)
    {
        if (answer.Correct)
        {
            PlaySound(SoundEffect.Correct);
        }
        else
        {
            PlaySound(SoundEffect.Fail);
        }
    }

    public bool SoundMuted { get; private set; }
    public bool MusicMuted { get; private set; }

    public void PlaySound(SoundEffect sfx)
    {
        switch (sfx)
        {
            case SoundEffect.Accept:
                soundAudioSource.PlayOneShot(accept);
                break;
            case SoundEffect.Back:
                soundAudioSource.PlayOneShot(back);
                break;
            case SoundEffect.Win:
                soundAudioSource.PlayOneShot(win);
                break;
            case SoundEffect.Fail:
                soundAudioSource.PlayOneShot(fail);
                break;
            case SoundEffect.Correct:
                soundAudioSource.PlayOneShot(correct);
                break;
            case SoundEffect.WinAlt:
                soundAudioSource.PlayOneShot(win2);
                break;
            default:
                break;
        }
    }

    public void LoadAudioSettings()
    {
        saveManager = SaveManager.instance;

        soundAudioSource.volume = saveManager.RetrieveFloat("soundVolume", 1);
        musicAudioSource.volume = saveManager.RetrieveFloat("musicVolume", 1);

        soundAudioSource.mute = saveManager.RetrieveInt("soundMuted", 0) == 1;
        musicAudioSource.mute = saveManager.RetrieveInt("musicMuted", 0) == 1;

        SoundMuted = soundAudioSource.mute;
        MusicMuted = musicAudioSource.mute;
    }

    public void ToggleSound()
    {
        soundAudioSource.mute = !soundAudioSource.mute;
        SoundMuted = soundAudioSource.mute;
        saveManager.Save("soundMuted", SoundMuted ? 1 : 0);
    }
    public void ToggleMusic()
    {
        musicAudioSource.mute = !musicAudioSource.mute;
        MusicMuted = musicAudioSource.mute;
        saveManager.Save("musicMuted", MusicMuted ? 1 : 0);
    }
}
