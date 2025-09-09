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
    }

    public void InitializeMusic()
    {
        musicSource.volume = audioData.musicVolume;
        _musicVolumeSlider.value = audioData.musicVolume;
    }

    public void InitializeSFX()
    {
        sfxSource.volume = audioData.sfxVolume;
        _sfxVolumeSlider.value = audioData.sfxVolume;
    }

    public void PlayMusic(string clipName)
    {
        if(musicSource != null)
        {
            musicSource.clip = audioData.GetAudioClip(clipName);
            musicSource.Play();
        }
    }

    public void StopMusic()
    {
        if (musicSource != null)
        {
            musicSource.Stop();
        }
    }

    public void PlaySFX(string clipName)
    {
        if (sfxSource != null)
        {
            RandomizePitch();
            sfxSource.clip = audioData.GetAudioClip(clipName);
            sfxSource.Play();
        }
    }

    void RandomizePitch()
    {
        float rand = Random.Range(1f, 2.5f);
        sfxSource.pitch = rand;
    }

    public void StopSFX()
    {
        if (sfxSource != null)
        {
            sfxSource.Stop();
        }
    }

    public void UpdateAllVolume()
    {
        audioMixer.SetFloat("MainVolume", Mathf.Log10(_allVolumeSlider.value) * 20);
    }

    public void UpdateMusicVolume()
    {
        musicSource.volume = _musicVolumeSlider.value;
    }

    public void UpdateSFXVolume()
    {
        sfxSource.volume = _sfxVolumeSlider.value;
    }
}
