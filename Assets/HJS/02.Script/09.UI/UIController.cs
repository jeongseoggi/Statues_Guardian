using System;
using TMPro;
using UnityEngine;

/// <summary>
/// HP Subject 클래스입니다.
/// </summary>
public class UIController : MonoBehaviour
{
    public event Action<float, int> OnHealthChanged;
    public int maxHealth;
    public int curHp;
    public TextMeshProUGUI hpText;


    /// <summary>
    /// 데미지를 받았을 때 HPBar 변화를 위한 함수입니다.
    /// </summary>
    /// <param name="curHp"></param>
    public void TakeDamage(float curHp)
    {
        this.curHp = (int)curHp;
        OnHealthChanged?.Invoke(curHp, maxHealth);
    }

    /// <summary>
    /// 최대 체력을 받아와서 세팅해주는 함수입니다. (추가) 최대 체력을 받아 올 때 이 오브젝트가 꺼져있으면 다시 켜줌
    /// </summary>
    /// <param name="hp"></param>
    public void GetMaxHp(float hp)
    {
        maxHealth = (int)hp;
        curHp = maxHealth;
        OnHealthChanged?.Invoke(curHp, maxHealth);
        SetText();
    }

    /// <summary>
    /// 체력표기 함수입니다.
    /// </summary>
    public void SetText()
    {
        if(hpText != null)
        {
            float ratio = maxHealth > 0 ? (float)curHp / maxHealth : 0f;
            hpText.text = (ratio * 100f).ToString("F0") + "%";
        }
    }
}
