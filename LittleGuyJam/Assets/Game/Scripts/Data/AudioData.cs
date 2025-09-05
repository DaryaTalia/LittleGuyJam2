using NUnit.Framework;
using UnityEngine;

[CreateAssetMenu(fileName = "AudioData", menuName = "Scriptable Objects/AudioData")]
public class AudioData : ScriptableObject
{
    public float mainVolume;
    public float musicVolume;
    public float sfxVolume;

    System.Collections.Generic.List<AudioTrack> tracks;

    public AudioTrack GetAudioTrack(string name)
    {
        return tracks.Find(track => track.AudioName == name);
    }
}

[System.Serializable]
public class AudioTrack
{
    string audioName;
    AudioClip clip;

    public string AudioName
    {
        get { return audioName; }   
    }

    public AudioClip Clip
    {
        get { return clip; }
    }
}