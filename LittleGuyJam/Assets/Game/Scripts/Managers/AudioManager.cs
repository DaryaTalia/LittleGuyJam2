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
        musicSource.volume = Mathf.Log10(audioData.musicVolume) * 20;
        musicSource.loop = true;
    }

    public void InitializeSFX()
    {
        sfxSource.volume = Mathf.Log10(audioData.sfxVolume) * 20;
        musicSource.loop = false;
    }

    public void PlayMusic()
    {
        if(musicSource != null)
        {
            musicSource?.Play();
        }
    }

    public void StopMusic()
    {
        if (musicSource != null)
        {
            musicSource?.Stop();
        }
    }

    public void PlaySFX()
    {
        if (musicSource != null)
        {
            sfxSource?.Play();
        }
    }

    public void StopSFX()
    {
        if (musicSource != null)
        {
            sfxSource?.Stop();
        }
    }

    public void UpdateAllVolume()
    {
        audioMixer.SetFloat("MainVolume", Mathf.Log10(_allVolumeSlider.value) * 20);
    }

    public void UpdateMusicVolume()
    {
        musicSource.volume = Mathf.Log10(_musicVolumeSlider.value) * 20;
    }

    public void UpdateSFXVolume()
    {
        sfxSource.volume = Mathf.Log10(_sfxVolumeSlider.value) * 20;
    }
}
