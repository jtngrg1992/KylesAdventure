using UnityEngine;

public class MFiring : MAiming
{
    private bool isFiring
    {
        get
        {
            return m_SM.isFiring;
        }
        set
        {
            m_SM.isFiring = value;
        }
    }

    public MFiring(MStateMachine stateMachine) : base("Firing", stateMachine) { }

    public override void Enter()
    {
        base.Enter();
        isFiring = true;
        MInputManager.shootingCancelled += HandleFiringCancel;
    }


    public override void Update()
    {
        base.Update();

        if (!isFiring)
        {
            m_SM.weapon.isFiring = false;
        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
        m_SM.weapon.isFiring = this.isFiring;
    }

    private void HandleFiringCancel()
    {
        m_SM.isFiring = false;
    }

    public override void HandleAimCancel()
    {
        base.HandleAimCancel();
        m_SM.isFiring = false;
        m_SM.isAiming = false;
    }
}