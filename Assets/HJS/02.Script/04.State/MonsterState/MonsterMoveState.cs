using UnityEngine;

public class MonsterMoveState : MonsterState
{
    public override void Init(IStateMachine sm)
    {
        base.Init(sm);
        monster = sm.GetOwner() as Monster;
    }
    public override void Enter()
    {
        sm.Animator.SetBool("isWalk", true);
    }

    public override void Exit()
    {
        sm.Animator.SetBool("isWalk", false);
    }

    public override void Update()
    {
        
    }
}
