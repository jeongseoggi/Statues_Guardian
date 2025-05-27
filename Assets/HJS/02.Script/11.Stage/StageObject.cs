using DG.Tweening;
using System.Collections;
using UnityEngine;

public class StageObject : MonoBehaviour, IHitable
{
    [SerializeField] float statuesCurHp;
    [SerializeField] float statuesMaxHp;
    [SerializeField] float statuesDef;
    [SerializeField] UIController uiController;
    [SerializeField] Player player;

    public float duration = 0.2f;      // Èçµé¸² ½Ã°£
    public float strength = 0.2f;      // Èçµé¸² ¼¼±â
    public int vibrato = 10;           // Áøµ¿ È½¼ö
    public float randomness = 90f;     // ·£´ý¼º

    Tween shakeTween;

    public void Init()
    {
        player = GameManager.Instance.GetPlayer();
        statuesMaxHp = player.MaxHp;
        statuesDef = player.Def;
        statuesCurHp = statuesMaxHp;
        uiController.GetMaxHp(statuesMaxHp);
    }

    public void Hit(float atk)
    {
        float damage = (atk - statuesDef) > 0 ? (atk - statuesDef) : 0;



        statuesCurHp -= damage;
        player.Hit(damage);
        TriggerShake();
        uiController.TakeDamage(statuesCurHp);
    }

    public void TriggerShake()
    {
        if (shakeTween == null || !shakeTween.IsActive() || !shakeTween.IsPlaying())
        {
            shakeTween = transform.DOShakePosition(duration, strength, vibrato, randomness, false, true);
        }
    }
}

