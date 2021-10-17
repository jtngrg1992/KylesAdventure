using UnityEngine;
using UnityEngine.InputSystem;

public class GroundedState : State
{
    public Vector2 smoothMovementInput;
    public Vector2 rawInput;

    private bool isSprinting = false;
    private Vector2 smoothInputRef;
    private float smoothTime = 0.2f;


    public GroundedState(Character character, StateMachine stateMachine) : base(character, stateMachine)
    {
    }

    public override void Enter()
    {
        base.Enter();
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
        MInputManager.movementPressed += HandleMoveInput;
    }

    protected override void DisableInput()
    {
        base.DisableInput();
        MInputManager.movementPressed -= HandleMoveInput;
    }

    private void HandleMoveInput(Vector2 move)
    {
        Debug.Log(move);
        rawInput = move;
    }
}