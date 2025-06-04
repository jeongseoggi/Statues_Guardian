using UnityEngine;

public class EffectAutoReturn : MonoBehaviour
{
    public EffectType effectType;

    public void AutoReturn()
    {
        EffectPoolManager.Instance.ReturnEffect(effectType, gameObject);
    }
}
