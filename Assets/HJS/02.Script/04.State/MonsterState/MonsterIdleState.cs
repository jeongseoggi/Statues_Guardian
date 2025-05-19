using UnityEngine;

public class MonsterIdleState : MonsterState
{
    public override void Init(IStateMachine sm)
    {
        base.Init(sm);
        monster = sm.GetOwner() as Monster;
    }
    public override void Enter()
    {
        monster.AttackDetectRange.boxCollider2D.enabled = true;
        monster.state = STATE.IDLE;
    }

    public override void Exit()
    {

    }

    public override void Update()
    {
    }
}
