using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PointBar : MonoBehaviour
{
    public Slider hpSlider;
    public Image hpSliderImg;
    public Image mpSliderImg;
    public UIController targetUI;


    private void OnEnable()
    {
        RegisterTarget();
    }

    public void RegisterTarget()
    {
        if (targetUI != null)
        {
            targetUI.BeforeDestory();

            if (hpSlider != null)
                targetUI.OnHealthChanged += UpdateUISlider;
            else if (hpSliderImg != null)
            {
                targetUI.OnHealthChanged += UpdateHpImage;
                targetUI.OnManaChanged += UpdateMpImage;
            }
        }
        else
        {
            if (GameManager.Instance?.Player != null)
            {
                targetUI = GameManager.Instance.GetPlayer().PlayerUIController;
                RegisterTarget();
            }
            else
            {
                GameManager.OnPlayerReady += () =>
                {
                    targetUI = GameManager.Instance.GetPlayer().PlayerUIController;
                    RegisterTarget();
                };
            }
        }
    }


    public void UpdateUISlider(float curHP, int maxHP)
    {
        hpSlider.value = (float)curHP / maxHP;
        targetUI.SetText();
    }

    public void UpdateHpImage(float curHP, int maxHP)
    {
        hpSliderImg.fillAmount = (float)curHP / maxHP;
        targetUI.SetText();
    }

    public void UpdateMpImage(float curMP, int maxMP)
    {
        mpSliderImg.fillAmount = (float)curMP / maxMP;
        targetUI.SetText();
    }

    private void OnDisable()
    {
        if (targetUI != null)
        {
            targetUI.OnHealthChanged -= UpdateUISlider;

            if (hpSliderImg != null)
            {
                targetUI.OnHealthChanged -= UpdateHpImage;
            }
            if (mpSliderImg != null)
            {
                targetUI.OnManaChanged -= UpdateMpImage;
            }
        }

      
    }
}
