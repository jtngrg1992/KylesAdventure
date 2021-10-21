using UnityEngine;

public class BaseState
{
    protected MStateMachine stateMachine;
    protected CharacterStateMachine m_SM;
    public string name;

    public BaseState(string name, MStateMachine stateMachine)
    {
        this.name = name;
        this.stateMachine = stateMachine;
        this.m_SM = (CharacterStateMachine)this.stateMachine;
    }

    public virtual void Update() { }

    public virtual void PhysicsUpdate() { }

    public virtual void Enter() { }

    public virtual void Exit() { }
}