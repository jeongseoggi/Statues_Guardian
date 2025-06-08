using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public class PlayerItemHandler : MonoBehaviour, IBuffUsable, IUseable
{
    [SerializeField] Player player;

    #region IBuffable 인터페이스 구현부
    public void ApplyBuff(BuffType buffType, float increse)
    {
        SetStatus(buffType, increse);
    }

    public float GetReturnValue(BuffType buffType)
    {
        switch (buffType)
        {
            case BuffType.AtkSpeed:
                return 0;
            case BuffType.AttackUp:
                return player.Atk;
            case BuffType.DefUp:
                return player.Def;
            case BuffType.DefDown:
                return player.Def;
            case BuffType.MoveSpeedUp:
                return player.Speed;
            default:
                return 0;
        }
    }

    public void ReturnBuffValue(BuffType buffType, float returnVal)
    {
        SetStatus(buffType, returnVal);
    }

    public Coroutine RunCoroutine(IEnumerator coroutine)
    {
        return StartCoroutine(coroutine);
    }

    public void SetStatus(BuffType buffType, float values)
    {
        switch (buffType)
        {
            case BuffType.AtkSpeed:
                break;
            case BuffType.AttackUp:
                player.Atk = values;
                break;
            case BuffType.DefUp:
                player.Def = values;
                break;
            case BuffType.DefDown:
                player.Def = values;
                break;
            case BuffType.MoveSpeedUp:
                player.Speed = values;
                break;
            case BuffType.InfinityMana:
                break;
        }
    }
    #endregion

    #region IUsable 인터페이스 구현부
    public void Heal(float amount, HealType healType)
    {
        if (healType == HealType.HP)
        {
            player.Hp += amount;
            StageManager.Instance?.stageObject.ActiveEffect();
#if UNITY_EDITOR
            Debug.Log($"HP 회복: {amount}, 현재 HP: {player.Hp}");
#endif
        }
        else
        {
            player.Mp += amount;
#if UNITY_EDITOR
            Debug.Log($"MP 회복: {amount}, 현재 MP: {player.Mp}");
#endif
        }

    }

    public void Upgrade(UpgradeType upgradeType, int useCount)
    {
        if (upgradeType == UpgradeType.Atk)
        {
            player.Atk += useCount;
        }
        else if (upgradeType == UpgradeType.Def)
        {
            player.Def += useCount;
        }
    }

    public float GetMaxHp()
    {
        return player.MaxHp;
    }

    public float GetMaxMp()
    {
        return player.Mp;
    }

    public void ApplyBuff(UnityAction action)
    {
        action?.Invoke();
    }
    #endregion
}
