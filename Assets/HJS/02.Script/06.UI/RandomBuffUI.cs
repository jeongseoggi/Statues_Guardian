using DG.Tweening;
using System;
using System.Collections;
using System.Threading;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RandomBuffUI : MonoBehaviour
{
    #region public
    public RectTransform    buffBox;            //빙글빙글 돌 버프 박스
    public Image            buffBoxImg;         //버프 이미지
    public float            spinTime;           //회전 시간
    public TextMeshProUGUI  buffNameTMP;        //표시 될 버프 이름 텍스트
    public TextMeshProUGUI  buffDescTMP;        //표시 될 버프 설명 텍스트
    public TextMeshProUGUI  effectText;         //표시 될 텍스트
    public Sprite           originSprite;       //기본 이미지 
    #endregion

    #region private
    private Sprite          randomBuffSprite;   // 랜덤 버프 스프라이트
    private Action          onFinish;           // 연출 종료 액션
    private string          buffName;           //버프 이름 
    private string          buffDesc;           //버프 설명 
    private Tween           effectTextTween;    //텍스트 효과 Tween
    private bool            isPlaying;          //연출 진행중인가?
    #endregion

    #region 프로퍼티
    public bool IsPlaying { get => isPlaying; set => isPlaying = value; }
    #endregion

    public void Init(Sprite randomSprite, string buffName, string buffDesc, Action onComplete = null)
    {
        MainObjectSetting(true);
        randomBuffSprite = randomSprite;
        this.buffName = buffName;
        this.buffDesc = buffDesc;
        onFinish = onComplete;
        StartBuffSpin();
    }

    public void StartBuffSpin()
    {
        Time.timeScale = 0f;

        IsPlaying = true;
        effectText.text = "뽑기를 진행하고 있습니다...";

        //텍스트 연출
        effectTextTween = effectText.DOFade(0f, 0.5f)
            .SetLoops(6, LoopType.Yoyo)
            .SetEase(Ease.Linear)
            .SetUpdate(true); //타임 스케일 무시


        //박스 연출
        buffBox.DORotate(new Vector3(0, 0, 360 * 5), spinTime, RotateMode.FastBeyond360)
       .SetEase(Ease.OutQuart)
       .SetUpdate(true) // 타임스케일 무시
       .OnComplete(ShowBuffResult);
    }

    void ShowBuffResult()
    {
        effectTextTween.Kill();
        
        buffBoxImg.sprite = randomBuffSprite;
        effectText.text =  "스크롤에서 해당 버프 아이템이 등장했습니다!";
        buffNameTMP.text = "뽑기 결과 : " + buffName;
        buffDescTMP.text = buffDesc;

        // 1초 후 게임 재개 및 버프 적용
        DOVirtual.DelayedCall(3f, () =>
        {
            Time.timeScale = 1f;

            MainObjectSetting(false);
            IsPlaying = false;
            onFinish?.Invoke();
        }).SetUpdate(true);
    }

    void MainObjectSetting(bool isActive)
    {
        this.gameObject.SetActive(isActive);
        if(!isActive)
        {
            buffBoxImg.sprite = originSprite;
            buffNameTMP.text = string.Empty;
            buffDescTMP.text = string.Empty;
        }
    }
}
