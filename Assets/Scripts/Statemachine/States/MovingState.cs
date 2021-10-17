using UnityEngine;
using UnityEngine.InputSystem;

public class MovingState : GroundedState
{
    private bool isSprinting;
    private bool isAiming;
    private bool isShooting;
    private InputAction sprintAction;
    private InputAction jumpAction;
    private InputAction aimAction;
    private InputAction shootAction;

    public MovingState(Character character, StateMachine stateMachine) : base(character, stateMachine)
    {
    }

    public override void Enter()
    {
        base.Enter();
        isAiming = false;
        sprintAction = character.PlayerInput.actions["Sprint"];
        jumpAction = character.PlayerInput.actions["Jump"];
        aimAction = character.PlayerInput.actions["Aim"];
        shootAction = character.PlayerInput.actions["Shoot"];

        sprintAction.performed += HandleSprintInput;
        sprintAction.canceled += HandleSprintInput;
        aimAction.performed += (_) => StartAim();
        shootAction.performed += HandleShoot;
    }

    public override void Exit()
    {
        base.Exit();
        sprintAction.performed -= HandleSprintInput;
        sprintAction.canceled -= HandleSprintInput;
        aimAction.performed -= (_) => StartAim();
        shootAction.performed -= HandleShoot;
    }

    public override void HandleUpdate()
    {
        base.HandleUpdate();

        if (jumpAction.triggered && character.Grounded)
        {
            stateMachine.ChangeState(character.jumping);
        }
        else if (isAiming)
        {
            character.aiming.smoothMovementInput = this.smoothMovementInput;
            character.aiming.rawInput = this.rawInput;
            stateMachine.ChangeState(character.aiming);
        }
        else if (isShooting)
        {
            isShooting = false;
            character.shooting.smoothMovementInput = this.smoothMovementInput;
            character.shooting.rawInput = this.rawInput;
            stateMachine.ChangeState(character.shooting);
        }
    }

    public override void HandlePhysicsUpdate()
    {
        base.HandlePhysicsUpdate();

        if (isSprinting)
        {
            character.MoveGround(smoothMovementInput * character.sprintMultiplier);
        }
        else
        {
            character.MoveGround(smoothMovementInput);
        }
    }

    private void HandleSprintInput(InputAction.CallbackContext context)
    {
        isSprinting = context.ReadValue<float>() == 1.0f;
    }

    private void StartAim()
    {
        isAiming = true;
    }

    private void HandleShoot(InputAction.CallbackContext context)
    {
        isShooting = context.ReadValue<float>() == 1;
    }
}