using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Singleton;
    public AudioClip gameButtonSoundClip;
    public AudioClip[] gameButtonNumbersClip;
    public AudioClip bingoAudioClip;
    public AudioClip nearMissAudioClip;

    [SerializeField]
    private AudioSource sfxAudioSource;
    [SerializeField]
    private AudioSource musicAudioSource;
    private void Awake()
    {
        if(Singleton == null)
        {
            Singleton = this;
        }

        DontDestroyOnLoad(this.gameObject);
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PlayOneShotSFX(AudioClip clip)
    {
        sfxAudioSource.PlayOneShot(clip);
    }

    public void SetMusicVolume(float volume)
    {
        musicAudioSource.volume = volume;
    }

    public void SetSFXVolume(float volume)
    {
        sfxAudioSource.volume = volume;
    }
}
