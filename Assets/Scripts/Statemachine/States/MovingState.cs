using UnityEngine;
using UnityEngine.InputSystem;

public class MovingState : GroundedState
{
    private bool isSprinting;
    private bool isAiming;
    private InputAction sprintAction;
    private InputAction jumpAction;
    private InputAction aimAction;

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
        sprintAction.performed += HandleSprintInput;
        sprintAction.canceled += HandleSprintInput;
        aimAction.performed += (_) => StartAim();
    }

    public override void Exit()
    {
        base.Exit();
        sprintAction.performed -= HandleSprintInput;
        sprintAction.canceled -= HandleSprintInput;
        aimAction.performed -= (_) => StartAim();
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
            stateMachine.ChangeState(character.aiming);
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
}