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
        MaxHp = 100;
        MaxMp = 100;
        Hp = MaxHp;
        Mp = MaxMp;
        Atk = 50;
        Def = 5;
        weapon.SetOwner(this);
    }

    protected override void Start()
    {
        base.Start();
        Init();
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

    public float GetMaxHp()
    {
        return MaxHp;
    }

    public float GetMaxMp()
    {
        return Mp;
    }
}
