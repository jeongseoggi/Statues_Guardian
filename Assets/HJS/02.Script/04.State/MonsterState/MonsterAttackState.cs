using UnityEngine;

public class MonsterAttackState : MonsterState
{
    public override void Init(IStateMachine sm)
    {
        base.Init(sm);
        monster = sm.GetOwner() as Monster;
    }
    public override void Enter()
    {
        monster.state = STATE.ATTACK;
        sm.Animator.SetBool("isAttack", true);
    }

    public override void Exit()
    {
        sm.Animator.SetBool("isAttack", false);
    }

    public override void Update()
    {
        
    }
}
