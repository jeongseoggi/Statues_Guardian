using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class SoundManager : SingleTon<SoundManager>, IObjectPool<Sound>
{
    public Queue<Sound> poolQueue = new Queue<Sound>();
    public int poolsize; //사운드 풀링 사이즈
    public Sound poolObject;

    [Header("오디오 소스")]
    public AudioSource bgmSource;
    public AudioSource sfxSource;

    private void Start()
    {
        Init(poolsize);
    }

    public void Init(int size)
    {
        for (int i = 0; i < poolsize; i++)
        {
            Sound so = Instantiate(poolObject, gameObject.transform);
            so.gameObject.SetActive(false);
            poolQueue.Enqueue(so);
        }
    }
    public Sound SpawnPool()
    {
        if (poolQueue.Count == 0)
        {
            Init(poolsize / 2);
        }
        return poolQueue.Dequeue();
    }

    public void ReturnPool(Sound poolObject)
    {
        poolObject.gameObject.SetActive(false);
        poolQueue.Enqueue(poolObject);
    }

   /// <summary>
    /// BGM에 사용
    /// </summary>
    /// <param name="clip"></param>
    /// <param name="loop"></param> 
    public void PlayBGM(AudioClip clip, bool loop = true)
    {
        bgmSource.clip = clip;
        bgmSource.loop = loop;
        bgmSource.Play();
    }

    /// <summary>
    /// UI 조작음에 사용
    /// </summary>
    /// <param name="clip"></param>
    public void PlaySFX(AudioClip clip)
    {
        sfxSource.PlayOneShot(clip);
    }

    /// <summary>
    /// BGM 종료
    /// </summary>
    public void StopBGM()
    {
        bgmSource.Stop();
    }
}

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
