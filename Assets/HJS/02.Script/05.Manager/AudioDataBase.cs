using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SoundDataBase", menuName = "Sound/SoundDataBase")]

public class AudioDataBase : ScriptableObject
{
    public Dictionary<string, AudioClip> audioDictionary = new Dictionary<string, AudioClip>();
    public AudioClip[] audioClips;

    public void Init()
    {
        for(int i = 0; i< audioClips.Length; i++)
        {
            audioDictionary.Add(audioClips[i].name, audioClips[i]);
        }
    }
}
