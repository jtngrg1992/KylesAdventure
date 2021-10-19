using UnityEngine;

public class MWalking : MGrounded
{
    CharacterStateMachine m_SM;
    private bool isSprinting = false;

    public MWalking(MStateMachine stateMachine) : base("Walking", stateMachine)
    {
        m_SM = (CharacterStateMachine)stateMachine;
    }

    public override void Enter()
    {
        base.Enter();
        isSprinting = false;
        MInputManager.sprintEngaged += HandleSprintPress;

    }

    public override void Exit()
    {
        base.Exit();
        MInputManager.sprintEngaged -= HandleSprintPress;

    }

    public override void Update()
    {
        base.Update();

        bool isMoving = !rawMovement.Equals(Vector2.zero);

        if (!isMoving)
        {
            Debug.Log(Mathf.Epsilon);
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
}