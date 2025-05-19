using System;
using UnityEngine;
using UnityEditor;
using System.Collections;

public abstract class Character : MonoBehaviour
{
    #region ����
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

    #region ������Ƽ

    public StateMachine<Character> StateMachine
    {
        get => stateMachine; set => stateMachine = value;
    }


    public virtual float Hp //HP ������Ƽ
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
    public virtual float Mp //MP ������Ƽ
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
    public virtual float Atk { get => atk; set => atk = value; }     //ATK ������Ƽ
    public virtual float Def { get => def; set => def = value; }    //DEF ������Ƽ
    public virtual float MaxHp { get => maxHp; set => maxHp = value; }    //MaxHp ������Ƽ
    public virtual float MaxMp { get => maxMp; set => maxMp = value; }    //MaxHp ������Ƽ
    public Action<string> WaitAnimAction { get => waitAnimAction; set => waitAnimAction = value; }    //�ִϸ��̼� ��ٸ��� Action������Ƽ
    public Animator Animator { get => animator; }    //�ִϸ����� ������Ƽ
    public bool IsAttacking { get => isAttacking; set => isAttacking = value; }    //���� ���������� �Ǵ��ϴ� ������Ƽ
    public float Speed { get=> speed; set => speed = value; }     //�ӵ� ������Ƽ
    #endregion

    protected virtual void Start()
    {
        WaitAnimAction += WaitAnim;
    }


    public abstract void Init();

    /// <summary>
    /// filpX ���� ���� ���� ������Ʈ ��ġ �̵�
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
    /// �ִϸ��̼��� ����Ǹ� ��� ���·� ���� �ڷ�ƾ
    /// </summary>
    /// <returns></returns>
    IEnumerator WaitAnimCo(string animName)
    {
        // ���� �ִϸ��̼��� ����Ǳ� ������ ������ ���
        yield return new WaitUntil(() => animator.GetCurrentAnimatorStateInfo(0).IsName(animName));

        // ���� ���� �ִϸ��̼��� ���� ���� ������ ���
        while (animator.GetCurrentAnimatorStateInfo(0).normalizedTime < 0.9f)
        {
            yield return null;
        }

        isAttacking = false;
        waitAnimCor = null;
        StateMachine.SetState(STATE.IDLE);
    }
}
