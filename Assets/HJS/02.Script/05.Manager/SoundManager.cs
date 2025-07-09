using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class SoundManager : SingleTon<SoundManager>, IObjectPool<Sound>
{
    #region public
    [Header("풀링 관련")]
    [Space(3)]
    public Queue<Sound> poolQueue = new Queue<Sound>();
    public int poolsize; //사운드 풀링 사이즈
    public Sound poolObject;

    [Header("오디오 소스")]
    [Space(3)]
    public AudioSource bgmSource;
    public AudioSource sfxSource;

    [Header("오디오 믹서")]
    [Space(3)]
    public AudioMixer mixer;

    [Header("슬라이더")]
    [Space(3)]
    public Slider bgmSlider;
    public Slider sfxSlider;
    #endregion

    #region private
    private Dictionary<string, float> soundValueDic;
    #endregion

    private void Start()
    {
        soundValueDic = new Dictionary<string, float>();
        LoadSoundValueData();
        Init(poolsize);

        bgmSlider.onValueChanged.AddListener((vol) => { SetVolume(SoundType.BGM, vol); });
        sfxSlider.onValueChanged.AddListener((vol) => { SetVolume(SoundType.SFX, vol); });
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

    public void SetVolume(SoundType soundType, float volume)
    {
        mixer.SetFloat(soundType.ToString(), Mathf.Log10(volume) * 20);
        SaveSoundValueData(soundType, volume);
    }

    private void LoadSoundValueData()
    {
        if(PlayerPrefs.HasKey("SoundVal"))
        {
            string soundValJson = PlayerPrefs.GetString("SoundVal");
            soundValueDic = JsonConvert.DeserializeObject<Dictionary<string, float>>(soundValJson);

            foreach (var pair in soundValueDic.ToList())
            {
                string keyName = pair.Key;
                float sliderVal = pair.Value;

                if(keyName.Contains("BGM"))
                {
                    bgmSlider.value = sliderVal;
                    SetVolume(SoundType.BGM, sliderVal);
                }
                else
                {
                    sfxSlider.value = sliderVal;
                    SetVolume(SoundType.SFX, sliderVal);
                }
            }

        }
    }

    private void SaveSoundValueData(SoundType soundType,float saveVal)
    {
        if (soundValueDic.ContainsKey(soundType.ToString()))
        {
            soundValueDic[soundType.ToString()] = saveVal;
        }
        else
        {
            soundValueDic.Add(soundType.ToString(), saveVal);
        }

        string soundValJson = JsonConvert.SerializeObject(soundValueDic);
        PlayerPrefs.SetString("SoundVal", soundValJson);
    }
}
