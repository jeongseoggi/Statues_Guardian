using UnityEngine;

public class TestPlayer : MonoBehaviour, IUseable
{
    public float hp;
    public float mp;
    public float GetMaxHp()
    {
        return 100;
    }

    public float GetMaxMp()
    {
        return 100;
    }

    public void Heal(float amount, HealType healType)
    {
        if (healType == HealType.HP)
        {
            hp += amount;
#if UNITY_EDITOR
            Debug.Log($"HP 회복: {amount}, 현재 HP: {hp}");
#endif
        }
        else
        {
            mp += amount;
#if UNITY_EDITOR
            Debug.Log($"MP 회복: {amount}, 현재 MP: {mp}");
#endif
        }
    }

    public void Upgrade(UpgradeType upgradeType, int useCount)
    {

    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Alpha1))
        {
            UIManager.Instance.quickSlotManager.quickSlots[0].ItemData?.Use(this);
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            UIManager.Instance.quickSlotManager.quickSlots[1].ItemData?.Use(this);

        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            UIManager.Instance.quickSlotManager.quickSlots[2].ItemData?.Use(this);
        }
        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            UIManager.Instance.quickSlotManager.quickSlots[3].ItemData?.Use(this);
        }

    }
}
