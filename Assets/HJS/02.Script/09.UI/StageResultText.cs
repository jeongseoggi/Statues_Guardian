using DG.Tweening;
using TMPro;
using UnityEngine;

public class StageResultText : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI resultText;
    Color[] resultTextColor = new Color[2];

    private void Start()
    {
        resultTextColor[0] = Color.yellow;
        resultTextColor[1] = Color.gray;
    }

    /// <summary>
    /// 임시코드
    /// </summary>
    private void OnEnable()
    {
        ShowResult("Stage Fail...", false);
    }

    public void ShowResult(string showText, bool isClear)
    {
        gameObject.SetActive(true);
        resultText.color = isClear ? resultTextColor[0] : resultTextColor[1];
        resultText.text = showText;

        resultText.transform.localScale = Vector3.zero;
        resultText.transform.DOScale(1.2f, 0.3f).SetEase(Ease.OutBack)
            .OnComplete(() => resultText.transform.DOShakeScale(0.3f, 0.1f));
    }
}