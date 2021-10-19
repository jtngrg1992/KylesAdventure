using UnityEngine;

public class MIdle : MGrounded
{
    private CharacterStateMachine m_SM;
    private bool isMoving = false;
    private bool isAiming;

    public MIdle(MStateMachine stateMachine) : base("Idle", stateMachine)
    {
        m_SM = (CharacterStateMachine)stateMachine;
    }

    public override void Enter()
    {
        base.Enter();
        isMoving = false;
        isAiming = false;
        MInputManager.aimEngaged += HandleAim;
    }

    public override void Exit()
    {
        base.Exit();
        MInputManager.aimEngaged -= HandleAim;
    }

    public override void Update()
    {
        base.Update();

        bool isMoving = !Mathf.Approximately(rawMovement.magnitude, 0);

        if (isMoving)
        {
            m_SM.ActivateState(m_SM.walkingState);
        }
        else if (isAiming)
        {
            m_SM.ActivateState(m_SM.aimingState);
        }
    }

    private void HandleAim()
    {
        isAiming = true;
    }
}