using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : Singleton<AudioManager>
{
    [SerializeField]
    private AudioSource soundAudioSource;
    [SerializeField]
    private AudioSource musicAudioSource;

    private SaveManager saveManager;

    public bool SoundMuted { get; private set; }
    public bool MusicMuted { get; private set; }

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
