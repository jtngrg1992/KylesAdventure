using UnityEngine;
using UnityEngine.InputSystem;

public class AimingState : MovingState
{
    private bool isAiming;
    private InputAction aimAction;

    public AimingState(Character character, StateMachine stateMachine) : base(character, stateMachine)
    {

    }


    public override void Enter()
    {
        base.Enter();
        aimAction = character.PlayerInput.actions["Aim"];
        isAiming = true;
        aimAction.canceled += (_) => CancelAim();
    }

    public override void HandlePhysicsUpdate()
    {
        base.HandlePhysicsUpdate();

        if (!isAiming)
        {
            character.EndAiming();
            stateMachine.GoBackToPrevState();
        }
        else
        {
            character.StartAiming();
        }
    }

    public override void Exit()
    {
        base.Exit();
        aimAction.canceled -= (_) => CancelAim();
        smoothMovementInput = Vector2.zero;
    }

    private void CancelAim()
    {
        isAiming = false;
    }
}