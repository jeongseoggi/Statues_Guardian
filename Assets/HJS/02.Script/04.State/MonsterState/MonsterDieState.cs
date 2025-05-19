using UnityEngine;

public class MonsterDieState : MonsterState
{
    public override void Init(IStateMachine sm)
    {
        base.Init(sm);
        monster = sm.GetOwner() as Monster;
    }
    public override void Enter()
    {
        monster.state = STATE.DIE;
        monster.AttackDetectRange.boxCollider2D.enabled = false;
        sm.Animator.SetBool("isDie", true);
        monster.onDieEffect?.Invoke();
    }

    public override void Exit()
    {
    } 

    public override void Update()
    {

    }
}
