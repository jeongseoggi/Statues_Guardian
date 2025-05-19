using TMPro;
using UnityEngine;
using static UnityEngine.RuleTile.TilingRuleOutput;

public class PlayerMoveState : PlayerState
{
    Vector3 target;
    float speed;
    public override void Init(IStateMachine sm)
    {
        base.Init(sm);
        player = (Player)this.sm.GetOwner();
    }

    public override void Enter()
    {
        player.state = STATE.MOVE;
        sm.Animator.SetBool("walk", true);
    }

    public override void Exit()
    {
        sm.Animator.SetBool("walk", false);
    }

    public override void Update()
    {
        
    }

}
