using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TargetHPBar : MonoBehaviour
{
    public Slider hpSlider;
    public UIController targetUI;

    private void OnEnable()
    {
        if (targetUI != null)
        {
            targetUI.OnHealthChanged += UpdateUI;
        }
    }


    public void UpdateUI(float curHP, int maxHP)
    {
        hpSlider.value = (float)curHP / maxHP;
        targetUI.SetText();
    }

    private void OnDisable()
    {
        targetUI.OnHealthChanged -= UpdateUI;
    }
}
