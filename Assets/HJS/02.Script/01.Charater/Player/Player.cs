using System;
using UnityEngine;

public class Player : Character, IUseable
{
    #region public
    public Func<int> getCombo;
    #endregion

    #region 프로퍼티
    public override float Atk 
    { 
        get => base.Atk;
        set 
        {
            base.Atk = value;
            weapon.damage = Atk;
            GameManager.Instance.PlayerStatData.Atk = value;
        }
    }

    public override float Def
    {
        get => base.Def;
        set
        {
            base.Def = value;
            GameManager.Instance.PlayerStatData.Def = value;
        }
    }

    public override float Hp
    {
        get => base.Hp;
        set
        {
            base.Hp = value;
            if(base.Hp > base.MaxHp)
            {
                base.Hp = base.MaxHp;
            }
            GameManager.Instance.PlayerStatData.Hp = base.Hp;
        }
    }

    public override float Mp
    {
        get => base.Mp;
        set
        {
            base.Mp = value;

            if (base.Mp > base.MaxMp)
            {
                base.Mp = base.MaxMp;
            }

            GameManager.Instance.PlayerStatData.Mp = base.Mp;
        }
    }


    #endregion

    private void Awake()
    {
        stateMachine = new StateMachine<Character>(this);
        stateMachine.AddState(STATE.IDLE, new PlayerIdleState());
        stateMachine.AddState(STATE.MOVE, new PlayerMoveState());
        stateMachine.AddState(STATE.ATTACK, new PlayerAttackState());
        stateMachine.AddState(STATE.HIT, new PlayerHitState());
        stateMachine.AddState(STATE.DIE, new PlayerDieState());
        StateMachine.SetAnimator(animator);
    }

    public override void Init()
    {
        MaxHp = GameManager.Instance.PlayerStatData.MaxHp;
        MaxMp = GameManager.Instance.PlayerStatData.MaxMp;
        Hp = GameManager.Instance.PlayerStatData.Hp;
        Mp = GameManager.Instance.PlayerStatData.Mp;
        Atk = GameManager.Instance.PlayerStatData.Atk;
        Def = GameManager.Instance.PlayerStatData.Def;
        Speed = GameManager.Instance.PlayerStatData.Speed;
        weapon.SetOwner(this); 
    }

    protected override void Start()
    {
        base.Start();
        GameManager.OnPlayerStatDataReady += Init;
    }

    public override void AttackOn()
    {
        weapon.weaponCol.enabled = true;
    }

    public override void AttackOff()
    {
        weapon.weaponCol.enabled = false;
    }

    /// <summary>
    /// 콤보 카운트 관련 함수
    /// </summary>
    /// <returns></returns>
    public int GetCombo()
    {
        int comboCount = getCombo();
        weapon.SetComboDmg(comboCount);
        return comboCount;
    }

    public void Heal(float amount, HealType healType)
    {
        if(healType == HealType.HP)
        {
            Hp += amount;
#if UNITY_EDITOR
            Debug.Log($"HP 회복: {amount}, 현재 HP: {hp}");
#endif
        }
        else
        {
            Mp += amount;
#if UNITY_EDITOR
            Debug.Log($"MP 회복: {amount}, 현재 MP: {mp}");
#endif
        }

    }

    public void Upgrade(UpgradeType upgradeType, int useCount)
    {
        if(upgradeType == UpgradeType.Atk)
        {
            Atk += useCount;
        }
        else if(upgradeType == UpgradeType.Def)
        {
            Def += useCount;
        }
    }

    public float GetMaxHp()
    {
        return MaxHp;
    }

    public float GetMaxMp()
    {
        return Mp;
    }

    private void OnDestroy()
    {
        GameManager.OnPlayerStatDataReady -= Init;
    }
}
