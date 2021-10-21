using UnityEngine;

public class MAiming : MGrounded
{

    private float aimSmoothRef;
    public BaseState fallbackState;

    public MAiming(string name, MStateMachine stateMachine) : base(name, stateMachine) { }


    public override void Enter()
    {
        base.Enter();
        m_SM.isAiming = true;
        MInputManager.aimCancelled += HandleAimCancel;
        MInputManager.shootingStarted += HandleShootStart;
        MInputManager.sprintEngaged += HandleSprintPress;
        MInputManager.sprintDisengaged += HandleSprintRelease;
    }

    public override void Exit()
    {
        base.Exit();
        MInputManager.aimCancelled -= HandleAimCancel;
        MInputManager.shootingStarted -= HandleShootStart;
        MInputManager.sprintEngaged -= HandleSprintPress;
        MInputManager.sprintDisengaged -= HandleSprintRelease;
    }

    public override void Update()
    {
        base.Update();

        if (m_SM.isFiring)
        {
            m_SM.ActivateState(m_SM.firingState);
            m_SM.firingState.fallbackState = this.fallbackState;
        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();

        float target = m_SM.isAiming ? 1 : 0;

        float smoothValue = Mathf.SmoothDamp(m_SM.weaponAimRigLayer.weight, target, ref aimSmoothRef, m_SM.aimSpeed * Time.deltaTime);
        float clampedSmoothValue = Mathf.Clamp(smoothValue, 0, 1);
        m_SM.weaponAimRigLayer.weight = clampedSmoothValue;

        if (clampedSmoothValue < 0.01f)
        {
            HandleDeactivation();
        }
    }

    public virtual void HandleAimCancel()
    {
        m_SM.isAiming = false;
    }

    private void HandleShootStart()
    {
        m_SM.isFiring = true;
    }

    private void HandleSprintPress()
    {
        m_SM.isSprinting = true;
    }

    private void HandleSprintRelease()
    {
        m_SM.isSprinting = false;
    }

    private void HandleDeactivation()
    {
        if (m_SM.isSprinting)
        {
            m_SM.ActivateState(m_SM.sprintingState);
        }
        else
        {
            m_SM.ActivateState(fallbackState);
        }
    }

}