using UnityEngine;

public class MSprinting : MGrounded
{
    private bool isSprinting
    {
        get
        {
            return m_SM.isSprinting;
        }
        set
        {
            m_SM.isSprinting = value;
        }
    }

    public MSprinting(MStateMachine stateMachine) : base("Sprinting", stateMachine) { }

    public override void Enter()
    {
        base.Enter();
        isSprinting = true;

        MInputManager.sprintDisengaged += HandleSprintButtonRelease;
        MInputManager.aimEngaged += HandleAimEngaged;
    }

    public override void Exit()
    {
        base.Exit();
        MInputManager.sprintDisengaged -= HandleSprintButtonRelease;
        MInputManager.aimEngaged -= HandleAimEngaged;
    }

    public override void Update()
    {
        base.Update();

        if (!isSprinting)
        {
            m_SM.ActivateState(m_SM.walkingState);
        }
    }

    private void HandleSprintButtonRelease()
    {
        isSprinting = false;
    }

    private void HandleAimEngaged()
    {
        m_SM.aimingState.fallbackState = this;
        m_SM.ActivateState(m_SM.aimingState);
    }
}