using DG.Tweening;
using UnityEngine;

public class DropDownAnimator : MonoBehaviour
{
    public GameObject[] targets;             // 연출할 대상 오브젝트 5개
    public float fadeDuration = 0.2f;        // 각 오브젝트 페이드 시간
    public float interval = 0.15f;            // 다음 오브젝트로 넘어가는 간격

    private void OnEnable()
    {
        StartFadeSequence();
    }

    public void ActiveDropDownObject(Vector2 mousePoint, Camera pressEventCamera)
    {
        this.gameObject.SetActive(true);

        RectTransformUtility.ScreenPointToLocalPointInRectangle(
               gameObject.transform.parent.GetComponent<RectTransform>(),
               mousePoint,
               pressEventCamera,
               out Vector2 localPoint);

        Vector2 offset = new Vector2(10f, -10f); // 오른쪽 아래
        this.gameObject.GetComponent<RectTransform>().anchoredPosition = localPoint + offset;

    }

    public void StartFadeSequence()
    {
        for (int i = 0; i < targets.Length; i++)
        {
            var target = targets[i];
            var canvasGroup = target.GetComponent<CanvasGroup>();

            if (canvasGroup == null)
            {
                canvasGroup = target.AddComponent<CanvasGroup>();
            }

            canvasGroup.alpha = 0f;       // 처음엔 투명
            target.SetActive(true);       // 오브젝트 활성화

            // DOTween으로 지연 후 fade-in
            canvasGroup.DOFade(1f, fadeDuration)
                .SetDelay(i * interval)
                .SetEase(Ease.OutCubic);
        }
    }
}
