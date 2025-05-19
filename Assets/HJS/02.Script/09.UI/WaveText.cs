using DG.Tweening;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// 웨이브 관련 UI
/// </summary>
public class WaveText : MonoBehaviour
{
    public TextMeshProUGUI waveTextMsg; // 텍스트 메시지
    public RectTransform waveTextRect;  // UI Text의 RectTransform
    public float startY = 600f;     // 시작 Y 위치 (화면 밖 위쪽)
    public float endY = -60f;         // 최종 위치
    public float duration = 1.5f;     // 떨어지는 시간
    public Ease easeType = Ease.OutBounce;

    bool isStageClear; //스테이지 클리어 여부

    /// <summary>
    /// 웨이브 텍스트 연출
    /// </summary>
    /// <param name="isClear"></param>
    public void PlayStampEffect(bool isClear, UnityAction onCompleteCallBack)
    {
        gameObject.SetActive(true);
        waveTextMsg.color = new Color(1, 1, 1, 0); // 처음에 투명

        // 초기 scale
        waveTextMsg.rectTransform.localScale = Vector3.zero;

        // 도장 찍듯이: 크기 커졌다가 줄어들고 알파 올라옴
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
