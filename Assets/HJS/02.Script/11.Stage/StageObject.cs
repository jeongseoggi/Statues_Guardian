using DG.Tweening;
using System.Collections;
using UnityEditor.ShaderGraph.Internal;
using UnityEngine;

public class StageObject : MonoBehaviour, IHitable
{
    [SerializeField] float statuesCurHp;
    [SerializeField] float statuesMaxHp;
    [SerializeField] float statuesDef;
    [SerializeField] Vector3 effectPos;
    Player player;

    public float duration = 0.2f;      // Èçµé¸² ½Ã°£
    public float strength = 0.2f;      // Èçµé¸² ¼¼±â
    public int vibrato = 10;           // Áøµ¿ È½¼ö
    public float randomness = 90f;     // ·£´ý¼º

    Tween shakeTween;

    public void Init(float maxHp, float def)
    {
        statuesMaxHp = maxHp;
        statuesDef = def;
        statuesCurHp = statuesMaxHp;
        player = GameManager.Instance.GetPlayer();
        StageManager.Instance.sharedHp.OnHealthChanged += UpdateUI;
        ActiveEffect();
    }

    public void Hit(float atk)
    {
        float damage = (atk - statuesDef) > 0 ? (atk - statuesDef) : 0;
        StageManager.Instance.sharedHp.TakeDamage(damage);
        TriggerShake();
    }

    public void UpdateUI(float changeHp)
    {
        statuesCurHp = changeHp;
        player.PlayerUIController.TakeDamage(statuesCurHp);
    }


    public void TriggerShake()
    {
        if (shakeTween == null || !shakeTween.IsActive() || !shakeTween.IsPlaying())
        {
            shakeTween = transform.DOShakePosition(duration, strength, vibrato, randomness, false, true);
        }
    }

    public void ActiveEffect()
    {
        GameObject effectObj = EffectPoolManager.Instance.GetEffect(EffectType.HealEffect, gameObject.transform);
        effectObj.transform.localPosition = Vector2.up;
    }

    public void OnDestroy()
    {
        StageManager.Instance.sharedHp.OnHealthChanged -= UpdateUI;
    }
}

