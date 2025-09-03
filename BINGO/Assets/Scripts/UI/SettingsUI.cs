using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingsUI : MonoBehaviour
{
    [SerializeField]
    private Slider musicSlider;
    [SerializeField]
    private Slider sfxSlider;
    // Start is called before the first frame update
    void Start()
    {
        musicSlider.onValueChanged.AddListener(UpdateMusicVolume);
        sfxSlider.onValueChanged.AddListener(UpdateSFXVolume);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void UpdateMusicVolume(float volume)
    {
        AudioManager.Singleton.SetMusicVolume(volume);
    }

    private void UpdateSFXVolume(float volume)
    {
        AudioManager.Singleton.SetSFXVolume(volume);
    }
}
