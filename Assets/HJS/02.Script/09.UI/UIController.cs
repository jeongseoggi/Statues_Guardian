using System;
using TMPro;
using UnityEngine;

/// <summary>
/// HP Subject Ŭ�����Դϴ�.
/// </summary>
public class UIController : MonoBehaviour
{
    public event Action<float, int> OnHealthChanged;
    public int maxHealth;
    public int curHp;
    public TextMeshProUGUI hpText;


    /// <summary>
    /// �������� �޾��� �� HPBar ��ȭ�� ���� �Լ��Դϴ�.
    /// </summary>
    /// <param name="curHp"></param>
    public void TakeDamage(float curHp)
    {
        this.curHp = (int)curHp;
        OnHealthChanged?.Invoke(curHp, maxHealth);
    }

    /// <summary>
    /// �ִ� ü���� �޾ƿͼ� �������ִ� �Լ��Դϴ�. (�߰�) �ִ� ü���� �޾� �� �� �� ������Ʈ�� ���������� �ٽ� ����
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
    /// ü��ǥ�� �Լ��Դϴ�.
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
