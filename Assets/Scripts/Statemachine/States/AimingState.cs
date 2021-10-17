using UnityEngine;
using UnityEngine.InputSystem;

public class AimingState : MovingState
{
    private bool isAiming;
    private InputAction aimAction;
    private bool isShooting;

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
            character.moving.smoothMovementInput = this.smoothMovementInput;
            character.moving.rawInput = this.rawInput;
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
        isShooting = false;
        isAiming = false;
    }

    private void CancelAim()
    {
        isAiming = false;
    }
}