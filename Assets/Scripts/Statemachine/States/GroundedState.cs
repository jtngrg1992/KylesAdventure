using UnityEngine;
using UnityEngine.InputSystem;

public class GroundedState : State
{
    protected Vector2 smoothMovementInput;
    private InputAction moveAction;
    private bool isSprinting = false;
    private Vector2 rawInput;
    private Vector2 smoothInputRef;
    private float smoothTime = 0.2f;


    public GroundedState(Character character, StateMachine stateMachine) : base(character, stateMachine)
    {
    }

    public override void Enter()
    {
        base.Enter();

        if (moveAction == null)
        {
            moveAction = character.PlayerInput.actions["Movement"];
        }

        EnableInput();
    }

    public override void Exit()
    {
        base.Exit();
        DisableInput();
    }


    public override void HandleUpdate()
    {
        base.HandleUpdate();
        smoothMovementInput = Vector2.SmoothDamp(smoothMovementInput, rawInput, ref smoothInputRef, smoothTime);
    }

    protected override void EnableInput()
    {
        base.EnableInput();
        moveAction.canceled += HandleMoveInput;
        moveAction.performed += HandleMoveInput;
    }

    protected override void DisableInput()
    {
        base.DisableInput();
        moveAction.canceled -= HandleMoveInput;
        moveAction.performed -= HandleMoveInput;
    }

    private void HandleMoveInput(InputAction.CallbackContext context)
    {
        rawInput = context.ReadValue<Vector2>().normalized;
    }
}