using DG.Tweening;
using System.Collections;
using UnityEngine;

public class StageObject : MonoBehaviour, IHitable
{
    [SerializeField] float stageObjHp;
    [SerializeField] float stageObjDef;
    [SerializeField] UIController uiController;

    public float duration = 0.2f;      // Èçµé¸² ½Ã°£
    public float strength = 0.2f;      // Èçµé¸² ¼¼±â
    public int vibrato = 10;           // Áøµ¿ È½¼ö
    public float randomness = 90f;     // ·£´ý¼º

    Tween shakeTween;

    private void OnEnable()
    {
      
    }

    private void Start()
    {
        stageObjHp = 100;
        uiController.GetMaxHp(stageObjHp);
    }

    public void Hit(float atk)
    {
        stageObjHp -= Mathf.Abs(atk - stageObjDef);
        TriggerShake();
        uiController.TakeDamage(stageObjHp);
    }

    public void TriggerShake()
    {
        if (shakeTween == null || !shakeTween.IsActive() || !shakeTween.IsPlaying())
        {
            shakeTween = transform.DOShakePosition(duration, strength, vibrato, randomness, false, true);
        }
    }
}

