using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class AudioManager : MonoBehaviour
{
    public AudioSource musicSource;
    public AudioSource sfxSource;

    public AudioData audioData;

    public AudioMixer audioMixer;

    [SerializeField]
    Slider _allVolumeSlider;
    [SerializeField]
    Slider _musicVolumeSlider;
    [SerializeField]
    Slider _sfxVolumeSlider;

    public void StartAudio()
    {
        InitializeMusic();
        InitializeSFX();
        PlayMusic();
    }

    public void InitializeMusic()
    {
        musicSource.volume = audioData.musicVolume;
        musicSource.loop = true;
        musicSource.clip = audioData.GetAudioTrack("Background").Clip;
    }

    public void InitializeSFX()
    {
        sfxSource.volume = audioData.sfxVolume;
        musicSource.loop = false;
    }

    public void PlayMusic()
    {
        musicSource.Play();
    }

    public void StopMusic()
    {

    }

    public void PlaySFX()
    {
        sfxSource?.Play();
    }

    public void StopSFX()
    {

    }

    public void UpdateAllVolume()
    {

    }

    public void UpdateMusicVolume() 
    { }

    public void UpdateSFXVolume() 
    { }
}
