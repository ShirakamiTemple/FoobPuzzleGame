//***
// Author: Nate
// Description: AudioHandler is a Handler for managing the games audio
//***

using System.Collections.Generic;
using System.Linq;
using FoxHerding.Generics;
using UnityEngine;
using UnityEngine.Audio;

namespace FoxHerding.Handlers
{
    public class AudioHandler : Handler<AudioHandler>
    {
        [SerializeField] private AudioMixer mainMixer;
        [SerializeField] private AudioSource musicSource, effectsSource;
        private const string MasterVolumeKey = "MasterVolume";
        private const string MusicVolumeKey = "MusicVolume";
        private const string SfxVolumeKey = "SFXVolume";
        public float MasterVolume { get; set; }
        public float MusicVolume { get; set; }
        public float SfxVolume { get; set; }
        [field: SerializeField, Tooltip("BGM References")]
        public SoundClip[] BgmClips { get; private set; }
        [field: SerializeField, Tooltip("SFX References")]
        public SoundClip[] SfxClips { get; private set; }

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
            SfxVolume = PlayerPrefs.GetFloat(SfxVolumeKey);
        }

        private void SaveAudioValues()
        {
            PlayerPrefs.SetFloat(MasterVolumeKey, MasterVolume);
            PlayerPrefs.SetFloat(MusicVolumeKey, MusicVolume);
            PlayerPrefs.SetFloat(SfxVolumeKey, SfxVolume);
        }

        private static SoundClip GetSoundClipByName(IEnumerable<SoundClip> clips, string soundName)
        {
            return clips.FirstOrDefault(sound => sound.Name == soundName);
        }

        private float AssignPitch(SoundClip soundClip)
        {
            return soundClip.UseRandomPitch ? Random.Range(soundClip.RandomPitchMin, soundClip.RandomPitchMax) : soundClip.Pitch;
        }
        
        public void PlaySound(SoundClip soundClip)
        {
            effectsSource.pitch = AssignPitch(soundClip);
            effectsSource.PlayOneShot(soundClip.Clip, soundClip.Volume);
        }

        public void PlaySound(AudioClip clip)
        {
            effectsSource.PlayOneShot(clip);
        }

        public void PlaySound(string soundName)
        {
            SoundClip clip = GetSoundClipByName(SfxClips, soundName);
            if (clip == null)
            {
                Debug.LogError("Could not find sound clip: " + soundName);
                return;
            }
            effectsSource.PlayOneShot(clip.Clip);
        }

        public void PlayMusic(SoundClip soundClip)
        {
            musicSource.pitch = soundClip.Pitch;
            musicSource.PlayOneShot(soundClip.Clip, soundClip.Volume);
        }

        public void PlayMusic(AudioClip clip)
        {
            musicSource.clip = clip;
            musicSource.Play();
        }
        
        public void PlayMusic(string musicName)
        {
            SoundClip clip = GetSoundClipByName(BgmClips, musicName);
            if (clip == null)
            {
                Debug.LogError("Could not find music clip: " + musicName);
                return;
            }
            musicSource.PlayOneShot(clip.Clip);
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
    
        public void ChangeSfxVolume(float value)
        {
            SfxVolume = Mathf.Log10(value) * 20;
            mainMixer.SetFloat(SfxVolumeKey, Mathf.Log10(value) * 20);
        }
    }

    [System.Serializable]
    public class SoundClip
    {
        [field: SerializeField]
        public string Name { get; private set; }
        [field: SerializeField]
        public AudioClip Clip { get; private set; }
        [field: SerializeField]
        public float Volume { get; private set; } = 1f;
        [field: SerializeField]
        public float Pitch { get; private set; } = 1f;
        [field: SerializeField]
        public bool UseRandomPitch { get; private set; }
        [field: SerializeField] 
        public float RandomPitchMin { get; private set; }
        [field: SerializeField] 
        public float RandomPitchMax { get; private set; }
    }
}