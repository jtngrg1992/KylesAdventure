using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class State
{
    protected StateMachine stateMachine;
    protected Character character;

    public State(Character character, StateMachine stateMachine)
    {
        this.character = character;
        this.stateMachine = stateMachine;
    }

    protected virtual void EnableInput()
    {

    }

    protected virtual void DisableInput()
    {

    }

    public virtual void Enter()
    {

    }

    public virtual void Exit()
    {

    }

    public virtual void HandleUpdate()
    {

    }

    public virtual void HandlePhysicsUpdate()
    {

    }
}
