using System;
using TMPro;
using UnityEngine;

/// <summary>
/// HP Subject 클래스입니다.
/// </summary>
public class UIController : MonoBehaviour
{
    public event Action<float, int> OnHealthChanged;
    public event Action<float, int> OnManaChanged;
    public int maxHealth;
    public int maxMana;
    public int curHp;
    public int curMp;
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

    public void GetMaxMp(float mp)
    {
        maxMana = (int)mp;
        curMp = maxMana;
        OnManaChanged?.Invoke(curMp, maxMana);
        SetText();
    }

    public void UseSkill(float curMp)
    {
        this.curMp = (int)curMp;
        OnManaChanged?.Invoke(curMp, maxMana);
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

    public void BeforeDestory()
    {
        OnHealthChanged = null;
        OnManaChanged = null;
    }

    private void OnDisable()
    {
        BeforeDestory();
    }
}
