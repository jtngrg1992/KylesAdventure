using UnityEngine;


public class MJumping : BaseState
{
    private bool isJumpTriggered = false;


    public MJumping(MStateMachine stateMachine) : base("Jumping", stateMachine) { }

    public override void Enter()
    {
        base.Enter();
        float jumpHeight = Mathf.Sqrt(m_SM.jumpHeight * -2f * m_SM.gravity);
        m_SM.animator.SetTrigger(m_SM.jumpingHash);
        m_SM.verticalVelocity += jumpHeight;
        m_SM.shouldJump = false;


    }

    public override void Update()
    {
        base.Update();

        m_SM.Controller.Move((m_SM.playerVelocity + new Vector3(0, m_SM.verticalVelocity, 0)) * Time.deltaTime);

        if (m_SM.Controller.isGrounded)
        {
            m_SM.animator.SetTrigger(m_SM.groundedHash);
            m_SM.UnWind();
        }

        m_SM.verticalVelocity += -15 * Time.deltaTime;

    }
}