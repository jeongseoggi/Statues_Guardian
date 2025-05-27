using UnityEngine;

public class DamageHandler : MonoBehaviour
{
    public float TakeDamage(float atk , float def)
    {
        return Mathf.Max(atk - def, 0);
    }
}
