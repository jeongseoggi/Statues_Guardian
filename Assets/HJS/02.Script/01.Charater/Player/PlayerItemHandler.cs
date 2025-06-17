using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerItemHandler : MonoBehaviour, IBuffUsable, IUseable
{
    [SerializeField] Player player;
    public List<BuffType> bucffList;

    private void Start()
    {
        bucffList = new List<BuffType>();
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.F12))
        {
            DataManager.Instance.BuffDatabase.buffDataList[1].BuffEffect(this);
        }
    }

    #region IBuffable 인터페이스 구현부
    public void ApplyBuff(BuffType buffType, float increse = 0)
    {
        SetStatus(buffType, increse, true);
    }

    public float GetReturnValue(BuffType buffType)
    {
        switch (buffType)
        {
            case BuffType.AtkSpeed:
                return player.AttackSpeed;
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

    public void ReturnBuffValue(BuffType buffType, float returnVal = 0)
    {
        SetStatus(buffType, returnVal, false);
    }

    public Coroutine RunCoroutine(IEnumerator coroutine)
    {
        return StartCoroutine(coroutine);
    }

    public void SetStatus(BuffType buffType, float values, bool isAdd)
    {
        float setValue = isAdd ? values : values * -1;

        switch (buffType)
        {
            case BuffType.AtkSpeed:
                player.AttackSpeed += setValue;
                break;
            case BuffType.AttackUp:
                player.Atk += setValue;
                break;
            case BuffType.DefUp:
                player.Def += setValue;
                break;
            case BuffType.DefDown:
                player.Def += setValue;
                break;
            case BuffType.MoveSpeedUp:
                player.Speed += setValue;
                break;
            case BuffType.InfinityMana:
                player.IsInfinityManaActive = isAdd;
                break;
        }
    }

    public void AddUsingBuff(BuffType buffType)
    {
        bucffList.Add(buffType);
    }

    public bool IsAlreadyUsingBuff(BuffType buffType)
    {
        return bucffList.Contains(buffType);
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
