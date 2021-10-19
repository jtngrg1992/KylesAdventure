using UnityEngine;

public class BaseState
{
    protected MStateMachine stateMachine;
    public string name;

    public BaseState(string name, MStateMachine stateMachine)
    {
        this.name = name;
        this.stateMachine = stateMachine;
    }

    public virtual void Update() { }

    public virtual void PhysicsUpdate() { }

    public virtual void Enter() { }

    public virtual void Exit() { }
}