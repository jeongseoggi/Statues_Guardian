using DG.Tweening;
using System.Collections;
using TMPro;
using UnityEngine;

public class ShopPanelHandler : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    [SerializeField] GameObject shopObj;
    [SerializeField] TextMeshProUGUI myGoldText;

    private void OnEnable()
    {
        if(GameManager.Instance?.PlayerData != null)
        {
            SetGoldText(GameManager.Instance.PlayerData.GetMyGold());
        }
        else
        {
            GameManager.OnPlayerDataReady += SetGoldText;
        }
    }

    void Start()
    {
        DOTween.Init();
        // transform 의 scale 값을 모두 0.1f로 변경합니다.
        shopObj.transform.localScale = Vector3.one * 0.1f;
        // 골드 변경 감지 이벤트 등록합니다.
        GameManager.Instance.PlayerData.OnGoldValueChanged += SetGoldText;
        // 객체를 비활성화 합니다.
        shopObj.SetActive(false);
    }


    public void Show()
    {
        DungeonUIManager.Instance.WaveText.SetText("Shopping Time!", ()=>
        {
            StartCoroutine(ShoppingTime());
        });

       
    }

    public void Hide()
    {
        var seq = DOTween.Sequence();

        shopObj.transform.localScale = Vector3.one * 0.2f;

        seq.Append(shopObj.transform.DOScale(1.1f, 0.1f));
        seq.Append(shopObj.transform.DOScale(0.2f, 0.2f));

        seq.Play().OnComplete(() =>
        {
            shopObj.gameObject.SetActive(false);
        });
    }

    IEnumerator ShoppingTime()
    {
        yield return new WaitForSeconds(1f);

        shopObj.SetActive(true);

        var seq = DOTween.Sequence();

        seq.Append(shopObj.transform.DOScale(1.1f, 0.2f));
        seq.Append(shopObj.transform.DOScale(1f, 0.1f));

        seq.Play();
        DungeonUIManager.Instance.WaveText.gameObject.SetActive(false);
        DungeonUIManager.Instance.nextWaveStartBTN.gameObject.SetActive(true);
    }

    public void SetGoldText(int gold)
    {
        myGoldText.text = gold.ToString();
    }

    public void Test()
    {
        shopObj.SetActive(true);

        var seq = DOTween.Sequence();

        seq.Append(shopObj.transform.DOScale(1.1f, 0.2f));
        seq.Append(shopObj.transform.DOScale(1f, 0.1f));

        seq.Play();
    }


    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F1))
        {
            Test();
        }
    }

    private void OnDisable()
    {
        GameManager.OnPlayerDataReady -= SetGoldText;
    }

    private void OnDestroy()
    {
        GameManager.Instance.PlayerData.OnGoldValueChanged -= SetGoldText;
    }
}
