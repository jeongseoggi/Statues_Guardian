using System;
using System.Threading;
using TMPro;
using UnityEditor.Timeline.Actions;
using UnityEngine;

public class Monster : Character, IHitable
{
    #region private
    [SerializeField] AttackDetectRange attackDetectRange;
    [SerializeField] MonsterData statData;
    [SerializeField] MonsterType monsterType;
    [SerializeField] UIController monsterUIController;
    [SerializeField] GameObject target;
    [SerializeField] GameObject projectileStartZone;
    #endregion

    #region public
    public Action<bool> attackAction; // ���� Action
    public Action onDieEffect; // ���� ���� Action
    public Action<Monster> OnDie; // �׾��� �� Action
    public Action trackingAction; //�߰� Action
    #endregion

    #region ������Ƽ
    public AttackDetectRange AttackDetectRange { get =>  attackDetectRange;}
    public GameObject Target { get => target; set => target = value; }
    public MonsterType MonsterType 
    {   get => monsterType; 
        set
        {
            monsterType = value;
            Init();
        }
    }
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

    void Awake()
    {
        //Init();
        StateMachine = new StateMachine<Character>(this);
        StateMachine.AddState(STATE.IDLE, new MonsterIdleState());
        StateMachine.AddState(STATE.MOVE, new MonsterMoveState());
        StateMachine.AddState(STATE.ATTACK, new MonsterAttackState());
        StateMachine.AddState(STATE.HIT, new MonsterHitState());
        StateMachine.AddState(STATE.DIE, new MonsterDieState());
        StateMachine.SetAnimator(animator);
        StateMachine.SetState(STATE.IDLE);
    }

    protected override void Start()
    {
        base.Start();
    }

    /// <summary>
    /// �ʱ� ���� ����
    /// </summary>
    public override void Init()
    {
        MonsterStatData data = statData.GetStatData(monsterType);
        if(data != null)
        {
            spriteRender.sprite = data.sprite;
            animator.runtimeAnimatorController = data.animator;
            MaxHp = data.maxHealth;
            Atk = data.attackPower;
            Def = data.defense;
            Speed = data.moveSpeed;
            Hp = maxHp;

            //���� ���� ����
            if(data.attackRangeSize == Vector2.zero)
            {
                attackDetectRange.boxCollider2D.size = data.attackRangeOriginSize;
                attackDetectRange.boxCollider2D.offset = data.attackRangeOriginOffset;
            }
            else
            {
                attackDetectRange.boxCollider2D.size = data.attackRangeSize;
                attackDetectRange.boxCollider2D.offset = data.attackRangeOffset;
            }
        }

        Target = StageManager.Instance.stageObject; // Ÿ�� ����
        weapon.SetOwner(this); 
        stateMachine.SetState(STATE.IDLE);
    }

    /// <summary>
    /// �������� �Ծ��� �� ó�����ִ� �Լ�
    /// </summary>
    /// <param name="atk"></param>
    public void Hit(float atk)
    {
        hp -= (atk - Def);
        if (hp <= 0)
        {
            if (stateMachine.CurState is MonsterDieState)
                return;
            attackAction(false);
            stateMachine.SetState(STATE.DIE);
        }
        monsterUIController.TakeDamage(hp);
    }

    /// <summary>
    /// �ִϸ��̼� �̺�Ʈ �Լ�
    /// </summary>
    public override void AttackOn()
    {
        //���Ÿ��� �ٰŸ� ���� ����
        if (monsterType == MonsterType.SkeletonArcher)
        {
            Projectile projecTile = ProjectileManager.instance.SpawnPool();
            projecTile.transform.position = projectileStartZone.transform.position;
            projecTile.damage = weapon.damage;
            projecTile.target = Target;
            projecTile.Shoot();
        }
        else
        {
            weapon.weaponCol.enabled = true;
        }

    }


    /// <summary>
    /// �ִϸ��̼� �̺�Ʈ �Լ�
    /// </summary>
    public override void AttackOff()
    {
        weapon.weaponCol.enabled = false;
    }

    public void ResetUI()
    {
        monsterUIController.GetMaxHp(maxHp);
    }
}   
