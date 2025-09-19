using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : SingletonMonoBehavior<AudioManager>
{
    public AudioSource backgroundSource;
    public AudioSource effectSource;
    public AudioClip backgroundClip; //背景音乐
    public SO_Sound so_Sound;
    private Dictionary<string, AudioClip> soundDictionary;
    protected override void Awake()
    {
        soundDictionary = new Dictionary<string, AudioClip>();
        backgroundSource.clip = backgroundClip;
        backgroundSource.Play();

        CreateSoundDictionary();
        base.Awake();
    }

    private void CreateSoundDictionary()
    {
        foreach (Sound sound in so_Sound.soundList)
        {
            soundDictionary.Add(sound.soundName, sound.audioClip);
        }
    }
    public void PlaySound(string soundName)
    {
        AudioClip audioClip = soundDictionary[soundName];
        effectSource.clip = audioClip;
        effectSource.Play();
    }
}
