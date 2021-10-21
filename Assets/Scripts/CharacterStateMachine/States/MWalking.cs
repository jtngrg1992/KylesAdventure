using UnityEngine;

public class MWalking : MGrounded
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

    public MWalking(MStateMachine stateMachine) : base("Walking", stateMachine) { }

    public override void Enter()
    {
        base.Enter();
        isSprinting = false;
        MInputManager.sprintEngaged += HandleSprintPress;
        MInputManager.aimEngaged += HandleAimEngaged;

    }

    public override void Exit()
    {
        base.Exit();
        MInputManager.sprintEngaged -= HandleSprintPress;
        MInputManager.aimEngaged -= HandleAimEngaged;

    }

    public override void Update()
    {
        base.Update();

        bool isMoving = !rawMovement.Equals(Vector2.zero);

        if (!isMoving)
        {
            m_SM.ActivateState(m_SM.idleState);
        }
        else if (isSprinting)
        {
            m_SM.ActivateState(m_SM.sprintingState);
        }
    }

    private void HandleSprintPress()
    {
        isSprinting = true;
    }

    private void HandleAimEngaged()
    {
        m_SM.aimingState.fallbackState = this;
        m_SM.ActivateState(m_SM.aimingState);
    }
}