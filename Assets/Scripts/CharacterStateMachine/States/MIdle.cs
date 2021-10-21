using UnityEngine;

public class MIdle : MGrounded
{
    private bool isMoving = false;

    public MIdle(MStateMachine stateMachine) : base("Idle", stateMachine) { }

    public override void Enter()
    {
        base.Enter();
        isMoving = false;
        MInputManager.aimEngaged += HandleAimEngaged;
    }

    public override void Exit()
    {
        base.Exit();
        MInputManager.aimEngaged -= HandleAimEngaged;
    }

    public override void Update()
    {
        base.Update();

        bool isMoving = !Mathf.Approximately(rawMovement.magnitude, 0);

        if (isMoving)
        {
            m_SM.ActivateState(m_SM.walkingState);
        }
    }

    private void HandleAimEngaged()
    {
        m_SM.aimingState.fallbackState = this;
        m_SM.ActivateState(m_SM.aimingState);
    }
}