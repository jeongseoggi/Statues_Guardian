using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StageResultText : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI resultText;
    [SerializeField] Image[] rewardImage; // 보상 UI들 (오른쪽 밖에 배치)
    [SerializeField] TextMeshProUGUI[] rewardAmounTexts; // 보상 UI들 (오른쪽 밖에 배치)
    [SerializeField] GameObject rewardObject; // 보상 부모 오브젝트


    public TextMeshProUGUI additionalText; //부가 설명 텍스트
    public RectTransform[] rewardItems; // 보상 UI들 (오른쪽 밖에 배치)
    public float delayBetween = 0.3f;
    public float moveDuration = 0.5f;
    public float targetX = 0f; // 왼쪽 이동 목표 x 좌표



    public void ShowResult(string showText, bool isClear)
    {
        gameObject.SetActive(true);
        resultText.color = isClear ? Color.yellow : Color.gray;
        resultText.text = showText;
        additionalText.text = isClear ? GameString.STAGE_CLEAR_ADDITIONAL : GameString.STAGE_FAIL_ADDITIONAL;

        ShowClearText();

        if (isClear)
        {
            rewardObject.SetActive(true);
            PlayRewardSlideIn();
        }
    }

    
    /// <summary>
    /// 스테이지 클리어 여부 텍스트 연출
    /// </summary>
    public void ShowClearText()
    {
        resultText.transform.localScale = Vector3.zero;
        Sequence seq = DOTween.Sequence();

        //연출 코드
        seq.Append(resultText.DOFade(1f, 0.1f)) // 빠르게 나타남
        .Join(resultText.transform.DOScale(1.3f, 0.2f).SetEase(Ease.OutBack)) // 팡 튀어나옴
        .Append(resultText.transform.DOShakeScale(0.3f, strength: 0.1f, vibrato: 10)) // 살짝 흔들
        .Join(resultText.transform.DOScale(1f, 0.2f)); // 원래 크기로 안정화
    }

    /// <summary>
    /// 보상 연출 및 보상 이미지 및 수량 표출 함수
    /// </summary>
    public void PlayRewardSlideIn()
    {
        RewardData rewardData = DataManager.Instance.GetRewardData();

        for (int i = 0; i < rewardData.itemIDs.Count; i++)
        {
            int index = i;
            RectTransform reward = rewardItems[i];
            targetX = reward.localPosition.x;

            // 초기 위치 오른쪽 밖으로 설정
            reward.anchoredPosition = new Vector2(Screen.width + 200f, reward.anchoredPosition.y);

            // 시퀀스 적용
            reward.DOAnchorPosX(targetX, moveDuration)
                  .SetEase(Ease.OutBack)
                  .SetDelay(index * delayBetween)
                  .OnComplete(()=>
                  {
                      string rewardItemSpriteName = DataManager.Instance.GetItemData(rewardData.itemIDs[index]).spriteName; // 아이템 스프라이트 이름 가져오기
                      rewardImage[index].sprite = SpriteManager.Instance.GetItemSprite(rewardItemSpriteName); // 보상 아이템 이미지 표기
                      rewardAmounTexts[index].text = 
                      DataManager.Instance.RewardDataBase.rewardList[GameManager.Instance.PlayerData.Stage - 1].amounts[index].ToString(); //보상 수량 표기
                  });
        }
    }
}