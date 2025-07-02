using System;
using UnityEngine;
using UnityEngine.Events;

public class Player : Character, IHitable
{
    #region public
    [Space(5)]
    [Header("플레이어 전용")]
    public Func<int> getCombo;
    public event Action OnCheckMana;
    public bool isDotActive;
    #endregion

    #region private
    [SerializeField] private UIController playerUIController;
    [SerializeField] private bool isInfinityManaActive;
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
            else if(base.Hp <= 0)
            {
                StageManager.Instance?.StageFail();
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
            PlayerUIController.UseSkill(base.Mp);
            OnCheckMana?.Invoke();
        }
    }
    public UIController PlayerUIController { get => playerUIController; }
    public Weapon Weapon { get => weapon; }
    public bool IsInfinityManaActive { get => isInfinityManaActive; set=> isInfinityManaActive = value; }
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
        AttackSpeed = GameManager.Instance.PlayerStatData.AttackSpeed;
        weapon.SetOwner(this);
        weapon.damage = Atk;
        PlayerUIController.GetMaxHp(MaxHp);
        PlayerUIController.GetMaxMp(MaxMp);

        //던전 입장 후 Init 이후에 처리 필요 현재 주석
        //StageManager.Instance?.sharedHp.SetHp(MaxHp, Def);
        //StageManager.Instance.sharedHp.OnHealthChanged += UpdateHp;

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

    private void OnDestroy()
    {
        GameManager.OnPlayerStatDataReady -= Init;
    }

    public void Hit(float atk)
    {
        float damage = (atk - Def) > 0 ? (atk - Def) : 0;
        StageManager.Instance.sharedHp.TakeDamage(damage);
    }

    public void UpdateHp(float current)
    {
        Hp = current;
        playerUIController.TakeDamage(Hp);
    }
}

