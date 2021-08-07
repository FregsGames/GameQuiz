using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static AudioManager;

public class ClickSoundPlayer : MonoBehaviour
{
    public SoundEffect sfx;

    void Start()
    {
        GetComponent<Button>().onClick.AddListener(PlaySound);
    }

    private void PlaySound()
    {
        AudioManager.Instance.PlaySound(sfx);
    }
}
