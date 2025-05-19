using DG.Tweening;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// ���̺� ���� UI
/// </summary>
public class WaveText : MonoBehaviour
{
    public TextMeshProUGUI waveTextMsg; // �ؽ�Ʈ �޽���
    public RectTransform waveTextRect;  // UI Text�� RectTransform
    public float startY = 600f;     // ���� Y ��ġ (ȭ�� �� ����)
    public float endY = -60f;         // ���� ��ġ
    public float duration = 1.5f;     // �������� �ð�
    public Ease easeType = Ease.OutBounce;

    bool isStageClear; //�������� Ŭ���� ����

    /// <summary>
    /// ���̺� �ؽ�Ʈ ����
    /// </summary>
    /// <param name="isClear"></param>
    public void PlayStampEffect(bool isClear, UnityAction onCompleteCallBack)
    {
        gameObject.SetActive(true);
        waveTextMsg.color = new Color(1, 1, 1, 0); // ó���� ����

        // �ʱ� scale
        waveTextMsg.rectTransform.localScale = Vector3.zero;

        // ���� �����: ũ�� Ŀ���ٰ� �پ��� ���� �ö��
        Sequence seq = DOTween.Sequence();
        seq.Append(waveTextMsg.DOFade(1f, 0.1f));
        seq.Join(waveTextMsg.rectTransform.DOScale(2f, 0.3f).SetEase(Ease.OutBack));
        seq.Append(waveTextMsg.rectTransform.DOScale(1f, 0.3f));

        seq.OnComplete(() =>
        {
            onCompleteCallBack();
        });
    }

    public void SetText(string text, UnityAction onStampEvent = null)
    {
        gameObject.SetActive(true);
        waveTextMsg.text = text;
        PlayStampEffect(isStageClear, () =>
        {
            if(onStampEvent == null)
            {
                if (!isStageClear)
                    StartCoroutine(CountingCor());
            }
            else
            {
                onStampEvent();
            }
        });
    }

    IEnumerator CountingCor()
    {
        int count = 3;
        yield return new WaitForSeconds(1);
        while (count > 0)
        {
            waveTextMsg.text = count + "...";
            yield return new WaitForSeconds(1);
            count--;
        }

        waveTextMsg.text = "Start!";
        yield return new WaitForSeconds(1);
        gameObject.SetActive(false);
        GameManager.Instance.WaveManager.waitCountDown = false;
    }

}
