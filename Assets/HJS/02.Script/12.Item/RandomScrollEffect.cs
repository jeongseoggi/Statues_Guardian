using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;

/// <summary>
/// 랜덤 스크롤 연출 스크립트
/// </summary>

public class RandomScrollEffect : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private float scaleUp = 1.05f; // 사이즈가 커질 크기
    private float duration = 0.2f; 

    private Vector3 originalScale; // 기존 스케일
    private Tween scaleTween;

    void Start()
    {
        originalScale = transform.localScale;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        scaleTween?.Kill(); //기존 Tween 있다면 종료 
        scaleTween = transform.DOScale(originalScale * scaleUp, duration).SetEase(Ease.OutBack);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        scaleTween?.Kill(); // Tween 종료
        scaleTween = transform.DOScale(originalScale, duration).SetEase(Ease.InOutSine);
    }
}
