using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;

/// <summary>
/// ���� ��ũ�� ���� ��ũ��Ʈ
/// </summary>

public class RandomScrollEffect : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private float scaleUp = 1.05f; // ����� Ŀ�� ũ��
    private float duration = 0.2f; 

    private Vector3 originalScale; // ���� ������
    private Tween scaleTween;

    void Start()
    {
        originalScale = transform.localScale;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        scaleTween?.Kill(); //���� Tween �ִٸ� ���� 
        scaleTween = transform.DOScale(originalScale * scaleUp, duration).SetEase(Ease.OutBack);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        scaleTween?.Kill(); // Tween ����
        scaleTween = transform.DOScale(originalScale, duration).SetEase(Ease.InOutSine);
    }
}
