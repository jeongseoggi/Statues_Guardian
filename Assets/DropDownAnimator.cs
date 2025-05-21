using DG.Tweening;
using UnityEngine;

public class DropDownAnimator : MonoBehaviour
{
    public GameObject[] targets;             // ������ ��� ������Ʈ 5��
    public float fadeDuration = 0.2f;        // �� ������Ʈ ���̵� �ð�
    public float interval = 0.15f;            // ���� ������Ʈ�� �Ѿ�� ����

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

        Vector2 offset = new Vector2(10f, -10f); // ������ �Ʒ�
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

            canvasGroup.alpha = 0f;       // ó���� ����
            target.SetActive(true);       // ������Ʈ Ȱ��ȭ

            // DOTween���� ���� �� fade-in
            canvasGroup.DOFade(1f, fadeDuration)
                .SetDelay(i * interval)
                .SetEase(Ease.OutCubic);
        }
    }
}
