using System;
using UnityEngine;
using UnityEditor;
using System.Collections;

public abstract class Character : MonoBehaviour
{
    #region 변수
    //protected
    [SerializeField] protected float hp;
    [SerializeField] protected float mp;
    [SerializeField] protected float maxHp;
    [SerializeField] protected float maxMp;
    [SerializeField] protected float atk;
    [SerializeField] protected float def;
    [SerializeField] protected Animator animator;
    [SerializeField] public STATE state;
    [SerializeField] protected float speed;
    [SerializeField] protected Weapon weapon;
    protected StateMachine<Character> stateMachine;
    protected Action<string> waitAnimAction;
    protected bool isAttacking;

    //public
    public SpriteRenderer spriteRender;
    public Coroutine waitAnimCor;
    #endregion

    #region 프로퍼티

    public StateMachine<Character> StateMachine
    {
        get => stateMachine; set => stateMachine = value;
    }


    public virtual float Hp //HP 프로퍼티
    {
        get => hp;
        set
        {
            hp = value;
            if(hp >= maxHp)
            {
                hp = maxHp;
            }
        }
    }
    public virtual float Mp //MP 프로퍼티
    {
        get => mp;
        set
        {
            mp = value;
            if (mp >= maxMp)
            {
                mp = maxMp;
            }
        }
    }
    public virtual float Atk { get => atk; set => atk = value; }     //ATK 프로퍼티
    public virtual float Def { get => def; set => def = value; }    //DEF 프로퍼티
    public virtual float MaxHp { get => maxHp; set => maxHp = value; }    //MaxHp 프로퍼티
    public virtual float MaxMp { get => maxMp; set => maxMp = value; }    //MaxHp 프로퍼티
    public Action<string> WaitAnimAction { get => waitAnimAction; set => waitAnimAction = value; }    //애니메이션 기다리는 Action프로퍼티
    public Animator Animator { get => animator; }    //애니메이터 프로퍼티
    public bool IsAttacking { get => isAttacking; set => isAttacking = value; }    //현재 공격중인지 판단하는 프로퍼티
    public float Speed { get=> speed; set => speed = value; }     //속도 프로퍼티
    #endregion

    protected virtual void Start()
    {
        WaitAnimAction += WaitAnim;
    }


    public abstract void Init();

    /// <summary>
    /// filpX 값에 따른 각종 오브젝트 위치 이동
    /// </summary>
    /// <param name="dir"></param>
    public virtual void SetFilpX(Vector3 dir)
    {
        if (dir.x > 0)
        {
            //spriteRender.flipX = false;
            //if(weapon != null)
            //{
            //    weapon.gameObject.transform.localScale = Vector3.one;
            //}
            this.gameObject.transform.localScale = new Vector3(3, 3, 1);
        }
        else
        {
            //spriteRender.flipX = true;
            //if(weapon != null)
            //{
            //    weapon.gameObject.transform.localScale = new Vector3(-2, 1, 1);
            //}

            this.gameObject.transform.localScale = new Vector3(-3, 3, 1);
        }
    }

    public void WaitAnim(string animName)
    {
        if (waitAnimCor == null)
            waitAnimCor = StartCoroutine(WaitAnimCo(animName));
        else
            return;
    }

    public abstract void AttackOn();
    public abstract void AttackOff();

    /// <summary>
    /// 애니메이션이 종료되면 대기 상태로 변경 코루틴
    /// </summary>
    /// <returns></returns>
    IEnumerator WaitAnimCo(string animName)
    {
        // 현재 애니메이션이 재생되기 시작할 때까지 대기
        yield return new WaitUntil(() => animator.GetCurrentAnimatorStateInfo(0).IsName(animName));

        // 현재 공격 애니메이션이 거의 끝날 때까지 대기
        while (animator.GetCurrentAnimatorStateInfo(0).normalizedTime < 0.9f)
        {
            yield return null;
        }

        isAttacking = false;
        waitAnimCor = null;
        StateMachine.SetState(STATE.IDLE);
    }
}
