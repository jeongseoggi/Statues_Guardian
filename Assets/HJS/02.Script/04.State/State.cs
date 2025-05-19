using UnityEngine;

public abstract class State
{
    public IStateMachine sm;

    public virtual void Init(IStateMachine sm)
    {
        this.sm = sm;
    }
    public abstract void Enter(); 
    public abstract void Update();
    public abstract void Exit();
}
