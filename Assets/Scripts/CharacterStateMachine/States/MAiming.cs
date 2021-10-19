using UnityEngine;

public class MAiming : MGrounded
{
    private CharacterStateMachine m_SM;
    private bool isAiming;
    private float aimSmoothRef;

    public MAiming(MStateMachine stateMachine) : base("Aiming", stateMachine)
    {
        m_SM = (CharacterStateMachine)stateMachine;
    }

    public override void Enter()
    {
        base.Enter();
        isAiming = true;

        MInputManager.aimCancelled += HandleAimCancelled;
    }

    public override void Exit()
    {
        base.Exit();
        MInputManager.aimCancelled -= HandleAimCancelled;
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();

        float target = isAiming ? 1 : 0;
        float smoothValue = Mathf.Clamp(Mathf.SmoothDamp(m_SM.weaponAimRigLayer.weight, target, ref aimSmoothRef, m_SM.aimSpeed * Time.deltaTime), 0, 1);
        m_SM.weaponAimRigLayer.weight = smoothValue;


        float diff = smoothValue - 0;

        if (diff < .1f && target == 0)
        {
            Debug.Log("sdsds");
            m_SM.UnWind();
        }
    }

    private void HandleAimCancelled()
    {
        isAiming = false;
    }
}