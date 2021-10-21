using UnityEngine;

public class MGrounded : BaseState
{
    private CharacterStateMachine m_SM;
    private Vector2 movementRef;
    private float multiplier;
    private float aimSmoothRef;
    protected Vector2 rawMovement;

    public MGrounded(string name, MStateMachine stateMachine) : base(name, stateMachine)
    {
        m_SM = (CharacterStateMachine)stateMachine;
        MInputManager.movementPressed += HandleMovementInput;
        MInputManager.aimEngaged += HandleAimEngaged;
        MInputManager.aimCancelled += HandleAimCancelled;
        MInputManager.jumpTriggered += HandleJump;
    }

    public override void Update()
    {
        base.Update();

        if (m_SM.shouldJump)
        {
            m_SM.ActivateState(m_SM.jumpingState);
        }
        else
        {
            multiplier = m_SM.ActiveState == m_SM.sprintingState ? m_SM.sprintMultiplier : 1f;

            m_SM.movementInput = Vector2.SmoothDamp(m_SM.movementInput, rawMovement, ref movementRef, m_SM.movementInputSmoothTime);

            if (m_SM.Grounded() && m_SM.verticalVelocity < 0)
            {
                m_SM.verticalVelocity = -2f;
            }

            Vector3 verticalMovement = m_SM.MainCam.transform.forward * m_SM.movementInput.y * multiplier;
            Vector3 horizontalMovement = m_SM.MainCam.transform.right * m_SM.movementInput.x * multiplier;
            Vector3 movementVelociy = verticalMovement + horizontalMovement;
            movementVelociy.y = 0;
            movementVelociy *= m_SM.walkingSpeed;
            m_SM.playerVelocity = movementVelociy;
            m_SM.verticalVelocity += m_SM.gravity * Time.deltaTime;
        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
        m_SM.animator.SetFloat(m_SM.velocityXHash, m_SM.movementInput.x * multiplier);
        m_SM.animator.SetFloat(m_SM.velocityYHash, m_SM.movementInput.y * multiplier);
        m_SM.Controller.Move((m_SM.playerVelocity + new Vector3(0, m_SM.verticalVelocity, 0)) * Time.deltaTime);

        float cameraRotationAngle = m_SM.MainCam.transform.rotation.eulerAngles.y;
        Quaternion targetRotation = Quaternion.Euler(0, cameraRotationAngle, 0);
        m_SM.transform.rotation = Quaternion.Lerp(m_SM.transform.rotation, targetRotation, Time.deltaTime * m_SM.rotationSpeed);


        float target = m_SM.isAiming ? 1 : 0;
        float smoothValue = Mathf.Clamp(Mathf.SmoothDamp(m_SM.weaponAimRigLayer.weight, target, ref aimSmoothRef, m_SM.aimSpeed * Time.deltaTime), 0, 1);
        m_SM.weaponAimRigLayer.weight = smoothValue;
    }

    private void HandleMovementInput(Vector2 move)
    {
        rawMovement = move.normalized;
    }

    private void HandleAimEngaged()
    {
        m_SM.isAiming = true;
    }

    private void HandleAimCancelled()
    {
        m_SM.isAiming = false;
    }

    private void HandleJump()
    {
        if (m_SM.Grounded())
        {
            m_SM.shouldJump = true;
        }
    }
}