using System;
using UnityEngine;

public class Player : Character, IUseable
{
    #region public
    public Func<int> getCombo;
    #endregion

    #region ������Ƽ
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
        //MaxHp = 100;
        //MaxMp = 100;
        //Hp = MaxHp;
        //Mp = MaxMp;
        //Atk = 50;
        //Def = 5;

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
    /// �޺� ī��Ʈ ���� �Լ�
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
            Debug.Log($"HP ȸ��: {amount}, ���� HP: {hp}");
#endif
        }
        else
        {
            Mp += amount;
#if UNITY_EDITOR
            Debug.Log($"MP ȸ��: {amount}, ���� MP: {mp}");
#endif
        }

    }

    public void Upgrade(UpgradeType upgradeType)
    {
        if(upgradeType == UpgradeType.Atk)
        {
            Atk += 1;
            Debug.Log("���ݷ� ���׷��̵�" + Atk);
        }
        else if(upgradeType == UpgradeType.Def)
        {
            Def += 1;
            Debug.Log("���� ���׷��̵�" + Def);
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
