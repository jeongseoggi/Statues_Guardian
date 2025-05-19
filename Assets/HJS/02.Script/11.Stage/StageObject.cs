using DG.Tweening;
using System.Collections;
using UnityEngine;

public class StageObject : MonoBehaviour, IHitable
{
    [SerializeField] float stageObjHp;
    [SerializeField] float stageObjDef;
    [SerializeField] UIController uiController;

    public float duration = 0.2f;      // ��鸲 �ð�
    public float strength = 0.2f;      // ��鸲 ����
    public int vibrato = 10;           // ���� Ƚ��
    public float randomness = 90f;     // ������

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

