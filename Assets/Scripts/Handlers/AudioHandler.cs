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

    private const string MasterVolume = "MasterVolume";
    private const string MusicVolume = "MusicVolume";
    private const string SFXVolume = "SFXVolume";

    public void PlaySound(AudioClip clip)
    {
        effectsSource.PlayOneShot(clip);
    }

    public void PlayMusic(AudioClip clip)
    {
        musicSource.clip = clip;
        musicSource.Play();
    }

    //Mathf.Log10(value) * 20)
    //Decibels are logarithmic and -90 sound volume is equivalent to 0db. Therefore, we need to multiply the value by 20
    //in order for the value to reach 0db.
    public void ChangeMasterVolume(float value)
    {
        mainMixer.SetFloat(MasterVolume, Mathf.Log10(value) * 20);
    }
    
    public void ChangeMusicVolume(float value)
    {
        mainMixer.SetFloat(MusicVolume, Mathf.Log10(value) * 20);
    }
    
    public void ChangeSFXVolume(float value)
    {
        mainMixer.SetFloat(SFXVolume, Mathf.Log10(value) * 20);
    }
}