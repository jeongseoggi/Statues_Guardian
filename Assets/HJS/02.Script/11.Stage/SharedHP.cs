using System;
using UnityEngine;

public class SharedHP : MonoBehaviour
{
    private float hp;
    private float maxHp;
    private float def;
    public Action<float> OnHealthChanged;

    public void SetHp(float maxHp, float def)
    {
        this.maxHp = maxHp;
        hp = maxHp;
        this.def = def;
        StageManager.Instance?.stageObject.Init(maxHp, def);
    }

    public void TakeDamage(float amount)
    {
        hp -= amount;
        OnHealthChanged?.Invoke(hp);
    }
}
