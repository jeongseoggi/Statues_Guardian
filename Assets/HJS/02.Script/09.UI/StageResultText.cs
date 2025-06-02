using DG.Tweening;
using TMPro;
using UnityEngine;

public class StageResultText : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI resultText;

    private void OnEnable()
    {
        ShowResult("Stage Clear!!");
    }

    public void ShowResult(string showText)
    {
        gameObject.SetActive(true);

        resultText.transform.localScale = Vector3.zero;
        resultText.transform.DOScale(1.2f, 0.3f).SetEase(Ease.OutBack)
            .OnComplete(() => resultText.transform.DOShakeScale(0.3f, 0.1f));
    }
}