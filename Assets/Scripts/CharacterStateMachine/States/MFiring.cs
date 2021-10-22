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
        m_SM.weapon.StartFiring();
    }

    public override void Update()
    {
        base.Update();
        if (!isFiring)
        {
            m_SM.weapon.StopFiring();
        }
        else
        {
            m_SM.weapon.UpdateFiring(Time.deltaTime);
        }

        m_SM.weapon.UpdateBullets(Time.deltaTime);
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