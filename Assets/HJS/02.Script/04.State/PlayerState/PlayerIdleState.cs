using UnityEngine;

public class PlayerIdleState : PlayerState
{
    public override void Init(IStateMachine sm)
    {
        base.Init(sm);
        player = this.sm.GetOwner() as Player;
    }
    public override void Enter()
    {
        player.state = STATE.IDLE;
        //sm.Animator.SetFloat("RunState", 0);
    }

    public override void Exit()
    {
        
    }

    public override void Update()
    {
        
    }
}
