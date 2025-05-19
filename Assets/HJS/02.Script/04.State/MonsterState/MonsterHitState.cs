using System.Threading;
using UnityEngine;

public class MonsterHitState : MonsterState
{
    public override void Init(IStateMachine sm)
    {
        base.Init(sm);
        monster = sm.GetOwner() as Monster;
    }

    public override void Enter()
    {
        throw new System.NotImplementedException();
    }

    public override void Exit()
    {
        throw new System.NotImplementedException();
    }

    public override void Update()
    {
        throw new System.NotImplementedException();
    }
}
