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
    public Action<bool> attackAction; // 공격 Action
    public Action onDieEffect; // 죽음 연출 Action
    public Action<Monster> OnDie; // 죽었을 때 Action
    public Action trackingAction; //추격 Action
    #endregion

    #region 프로퍼티
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
    /// 초기 스탯 세팅
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

            //공격 범위 조정
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

        Target = StageManager.Instance.stageObject; // 타겟 설정
        weapon.SetOwner(this); 
        stateMachine.SetState(STATE.IDLE);
    }

    /// <summary>
    /// 데미지를 입었을 때 처리해주는 함수
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
    /// 애니메이션 이벤트 함수
    /// </summary>
    public override void AttackOn()
    {
        //원거리와 근거리 조건 분할
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
    /// 애니메이션 이벤트 함수
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
