using System.Collections;
using UnityEngine;

public class Sound : MonoBehaviour
{
    [SerializeField] AudioSource audioSource;
    private AudioClip soundClip;

    public void Init(AudioClip soundClip)
    {
        this.soundClip = soundClip;
    }

    public void Play(AudioClip clip)
    {
        gameObject.SetActive(true);
        audioSource.clip = clip;
        audioSource.Play();

        StartCoroutine(ReturnToPoolAfter(clip.length));
    }

    private IEnumerator ReturnToPoolAfter(float duration)
    {
        yield return new WaitForSeconds(duration);
        SoundManager.Instance.ReturnPool(this);
    }
}
