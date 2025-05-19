using Mono.Cecil.Cil;
using System.Collections.Generic;
using UnityEngine;

public class StateMachine<T> : IStateMachine where T : Character
{
    public Animator animator = null;
    T owner;
    State curState;
    int stateEnumint;
    public Dictionary<STATE, State> stateDic = new Dictionary<STATE, State>();

    public State CurState => curState;
    public Animator Animator => animator;
    public object GetOwner() => owner;
    public StateMachine(T owner)
    {
        this.owner = owner;
    }
    public void SetAnimator(Animator animator)
    {
        this.animator = animator;
    }

    public void SetState(STATE state)
    {
        curState?.Exit();
        curState = stateDic[state];
        curState.Enter();
    }

    public void AddState(STATE state_type, State state)
    {
        if (stateDic.ContainsKey(state_type)) //키 중복 방지
            return;
        state.Init(this);
        stateDic.Add(state_type, state);
    }

    public State GetState()
    {
        return curState;
    }

    public void Update()
    {
        curState.Update();
    }
}
