using NUnit.Framework;
using UnityEngine;

[CreateAssetMenu(fileName = "AudioData", menuName = "Scriptable Objects/AudioData")]
public class AudioData : ScriptableObject
{
    [UnityEngine.Range(0f, 1f)]
    public float mainVolume;
    [UnityEngine.Range(0f, 1f)]
    public float musicVolume;
    [UnityEngine.Range(0f, 1f)]
    public float sfxVolume;

    public System.Collections.Generic.List<AudioTrack> tracks;

    public AudioClip GetAudioClip(string name)
    {
        return tracks.Find(track => track.AudioName == name).clip;
    }
}

[System.Serializable]
public class AudioTrack
{
    public string audioName;
    public AudioClip clip;

    public string AudioName
    {
        get { return audioName; }   
    }

    public AudioClip Clip
    {
        get { return clip; }
    }
}