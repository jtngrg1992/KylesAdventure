using UnityEngine;
using UnityEngine.InputSystem;

public class ShootingState : MovingState
{
    private InputAction shootAction;
    private bool isShooting;

    public ShootingState(Character character, StateMachine stateMachine) : base(character, stateMachine)
    {
    }

    public override void Enter()
    {
        base.Enter();
        isShooting = true;
        shootAction = character.PlayerInput.actions["Shoot"];
        shootAction.canceled += HandleShoot;
    }

    public override void Exit()
    {
        base.Exit();
        isShooting = false;
        shootAction.canceled -= HandleShoot;
    }

    public override void HandlePhysicsUpdate()
    {
        base.HandlePhysicsUpdate();

        if (isShooting)
        {
            character.StartShooting();
        }
        else
        {
            character.moving.smoothMovementInput = this.smoothMovementInput;
            character.moving.rawInput = this.rawInput;
            character.StopShooting();
            stateMachine.GoBackToPrevState();
        }
    }


    public void HandleShoot(InputAction.CallbackContext context)
    {
        isShooting = false;
    }
}