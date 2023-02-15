//***
// Author: Nate
// Description: AudioHandler is a Handler for managing the games audio
//***

using UnityEngine;
using UnityEngine.Audio;

public class AudioHandler : Handler<AudioHandler>
{
    [SerializeField] private AudioMixer mainMixer;
    [SerializeField] private AudioSource musicSource, effectsSource;
    private const string MasterVolumeKey = "MasterVolume";
    private const string MusicVolumeKey = "MusicVolume";
    private const string SFXVolumeKey = "SFXVolume";
    public float MasterVolume { get; set; }
    public float MusicVolume { get; set; }
    public float SFXVolume { get; set; }
    
    protected override void Awake()
    {
        base.Awake();
        SaveLoadHandler.Instance.Load += AssignAudioValues;
        SaveLoadHandler.Instance.Save += SaveAudioValues;
    }

    private void OnDestroy()
    {
        SaveLoadHandler.Instance.Load -= AssignAudioValues;
        SaveLoadHandler.Instance.Save -= SaveAudioValues;
    }

    private void AssignAudioValues()
    {
        MasterVolume = PlayerPrefs.GetFloat(MasterVolumeKey);
        MusicVolume = PlayerPrefs.GetFloat(MusicVolumeKey);
        SFXVolume = PlayerPrefs.GetFloat(SFXVolumeKey);
    }

    private void SaveAudioValues()
    {
        PlayerPrefs.SetFloat(MasterVolumeKey, MasterVolume);
        PlayerPrefs.SetFloat(MusicVolumeKey, MusicVolume);
        PlayerPrefs.SetFloat(SFXVolumeKey, SFXVolume);
    }

    public void PlaySound(AudioClip clip)
    {
        effectsSource.PlayOneShot(clip);
    }

    public void PlayMusic(AudioClip clip)
    {
        musicSource.clip = clip;
        musicSource.Play();
    }

    // Mathf.Log10(value) * 20)
    // Decibels are logarithmic and -90 sound volume is equivalent to 0db. Therefore, we need to multiply the value by 20
    // in order for the value to reach 0db.
    public void ChangeMasterVolume(float value)
    {
        MasterVolume = Mathf.Log10(value) * 20;
        mainMixer.SetFloat(MasterVolumeKey, MasterVolume);
    }
    
    public void ChangeMusicVolume(float value)
    {
        MusicVolume = Mathf.Log10(value) * 20;
        mainMixer.SetFloat(MusicVolumeKey, Mathf.Log10(value) * 20);
    }
    
    public void ChangeSFXVolume(float value)
    {
        SFXVolume = Mathf.Log10(value) * 20;
        mainMixer.SetFloat(SFXVolumeKey, Mathf.Log10(value) * 20);
    }
}