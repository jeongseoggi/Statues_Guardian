using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterBehaviour : MonoBehaviour
{
    #region private
    [SerializeField] Monster monster;
    Coroutine trackingCor;
    #endregion

    private void OnEnable()
    {
        monster.trackingAction += TrackingTarget;
        monster.attackAction += OnAttack;
        monster.onDieEffect += DieActionChain;
    }


    private void Start()
    {
        
    }

    private void Update()
    {
        monster.StateMachine.Update();
    }

    public void TrackingTarget()
    {
        monster.StateMachine.SetState(STATE.MOVE);
        trackingCor = StartCoroutine(TrackingTargetCor());
    }

    private void OnAttack(bool isAttack)
    {
        monster.IsAttacking = isAttack;
        if (monster.IsAttacking)
            monster.StateMachine.SetState(STATE.ATTACK);
    }

    /// <summary>
    /// ���� �׾��� �� ����ϴ� Action ü�̴�
    /// </summary>
    public void DieActionChain()
    {
        StartCoroutine(DieSpriteAlphaSettingCor());
    }

    /// <summary>
    /// Ÿ�� ���� �ڷ�ƾ
    /// </summary>
    /// <returns></returns>
    IEnumerator TrackingTargetCor()
    {
        Vector3 dir = monster.Target.transform.position - transform.position;
        monster.SetFilpX(dir);

        while (!monster.IsAttacking)
        {
            if (monster.StateMachine.CurState is MonsterDieState)
                yield break;

            transform.position = Vector3.MoveTowards(transform.position, monster.Target.transform.position, Time.deltaTime * monster.Speed);
            yield return null;
        }
        trackingCor = null;

    }

    /// <summary>
    /// �׾��� �� ���� ��������Ʈ �̹��� ���İ� ������ ���� ���� ����
    /// </summary>
    /// <returns></returns>
    IEnumerator DieSpriteAlphaSettingCor()
    {
        Color color = monster.spriteRender.color;
        yield return new WaitForSeconds(1.5f);
        while (monster.spriteRender.color.a >= 0)
        {
            color.a -= 0.1f;
            monster.spriteRender.color = color;
            yield return new WaitForSeconds(0.3f);
        }
        color.a = 1.0f;
        monster.spriteRender.color = color;
        monster.OnDie?.Invoke(monster);
    }

    

    private void OnDisable()
    {
        if(trackingCor != null)
        {
            StopCoroutine(trackingCor);
            trackingCor = null;
        }

        monster.trackingAction -= TrackingTarget;
        monster.attackAction -= OnAttack;
        monster.onDieEffect -= DieActionChain;
    }
}
