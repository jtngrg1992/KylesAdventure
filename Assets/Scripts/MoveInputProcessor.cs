using UnityEngine;
using UnityEngine.InputSystem;

class MoveInputProcessor
{
    private PlayerInput playerInput;
    private Vector2 rawInput;
    private Vector2 smoothInput;
    private Vector2 smoothInputRef;

    private bool isSprinting = false;
    private bool isJumping = false;

    private float smoothTime = 0.2f;
    private float sprintFactor = 2;
    private float jumpHeight = 2f;
    private float gravity = -9.8f;

    private InputAction moveAction;
    private InputAction sprintAction;
    private InputAction jumpAction;

    public MoveInputProcessor(PlayerInput playerInput)
    {
        this.playerInput = playerInput;
        this.moveAction = playerInput.actions["Movement"];
        this.sprintAction = playerInput.actions["Sprint"];
        this.jumpAction = playerInput.actions["Jump"];
    }

    public void EnableControls()
    {
        moveAction.performed += HandleMove;
        moveAction.canceled += HandleMove;
        sprintAction.performed += HandleSprint;
        jumpAction.performed += HandleJump;
        jumpAction.canceled += HandleJump;
    }

    public void DisableControls()
    {
        moveAction.performed -= HandleMove;
        moveAction.canceled -= HandleMove;
        sprintAction.performed -= HandleSprint;
        jumpAction.performed -= HandleJump;
        jumpAction.canceled -= HandleJump;
    }

    private void HandleMove(InputAction.CallbackContext context)
    {
        rawInput = context.ReadValue<Vector2>();
        smoothInput = Vector2.SmoothDamp(smoothInput, rawInput, ref smoothInputRef, smoothTime);
    }

    private void HandleSprint(InputAction.CallbackContext context)
    {
        isSprinting = context.ReadValue<float>() == 1f;
    }

    private void HandleJump(InputAction.CallbackContext context)
    {
        isJumping = context.ReadValue<float>() == 1f;
    }


    private float SprintFactor
    {
        get
        {
            if (isSprinting)
            {
                return sprintFactor;
            }
            else
            {
                return 1;
            }
        }
    }

    public Vector2 SmoothInput
    {
        get
        {
            if (rawInput.magnitude > 0)
            {
                smoothInput = Vector2.SmoothDamp(smoothInput, rawInput, ref smoothInputRef, smoothTime);
                return new Vector2(smoothInput.x * SprintFactor, smoothInput.y * SprintFactor);
            }
            return Vector2.zero;

        }
    }

    public void ProcessMovementInput(Vector2 move)
    {
        smoothInput = Vector2.SmoothDamp(smoothInput, move, ref smoothInputRef, smoothTime);
    }

    public bool IsJumping
    {
        get
        {
            return isJumping;
        }
    }

    public float Gravity
    {
        get
        {
            return gravity;
        }
    }

    public float JumpHeight
    {
        get
        {
            if (isJumping)
            {
                return Mathf.Sqrt(jumpHeight * -2f * gravity);
            }
            return 0;
        }
    }

}
