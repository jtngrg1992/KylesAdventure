using UnityEngine;


public class MJumping : BaseState
{
    private CharacterStateMachine m_SM;
    private bool isJumpTriggered = false;

    public MJumping(MStateMachine stateMachine) : base("Jumping", stateMachine)
    {
        m_SM = (CharacterStateMachine)stateMachine;
    }

    public override void Enter()
    {
        base.Enter();
        float jumpHeight = Mathf.Sqrt(2 * -2f * m_SM.gravity);
        m_SM.animator.SetTrigger(m_SM.jumpingHash);
        m_SM.verticalVelocity += jumpHeight;
        m_SM.shouldJump = false;
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
        m_SM.Controller.Move((m_SM.playerVelocity + new Vector3(0, m_SM.verticalVelocity, 0)) * Time.deltaTime);

        if (m_SM.Controller.isGrounded)
        {
            m_SM.animator.SetTrigger(m_SM.groundedHash);
            m_SM.UnWind();
        }
    }

    public override void Update()
    {
        base.Update();

        if (m_SM.verticalVelocity < 0)
        {
            m_SM.verticalVelocity = -2f;
        }
        m_SM.verticalVelocity += m_SM.gravity * Time.deltaTime;
    }

    private void GetFallbackState()
    {

    }
}