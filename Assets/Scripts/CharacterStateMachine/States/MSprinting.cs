using UnityEngine;

public class MSprinting : MGrounded
{
    private CharacterStateMachine m_Sm;
    private bool isSprinting;

    public MSprinting(MStateMachine stateMachine) : base("Sprinting", stateMachine)
    {
        m_Sm = (CharacterStateMachine)stateMachine;
    }

    public override void Enter()
    {
        base.Enter();
        isSprinting = true;

        MInputManager.sprintDisengaged += HandleSprintButtonRelease;
    }

    public override void Exit()
    {
        base.Exit();
        MInputManager.sprintDisengaged -= HandleSprintButtonRelease;
    }

    public override void Update()
    {
        base.Update();

        if (!isSprinting)
        {
            m_Sm.ActivateState(m_Sm.walkingState);
        }
    }

    private void HandleSprintButtonRelease()
    {
        isSprinting = false;
    }
}