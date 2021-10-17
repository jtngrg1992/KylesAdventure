using UnityEngine.InputSystem;
using UnityEngine;



public delegate void NotifyMovement(Vector2 movement);
public delegate void NotifyAction();

class MInputManager
{
    private PlayerInput playerInput;
    private static MInputManager instance;

    public InputAction moveAction;
    public InputAction sprintAction;
    public InputAction jumpAction;
    public InputAction aimAction;
    public InputAction shootAction;

    public static event NotifyMovement movementPressed;
    public static event NotifyAction sprintEngaged;
    public static event NotifyAction sprintDisengaged;
    public static event NotifyAction jumpTriggered;
    public static event NotifyAction aimEngaged;
    public static event NotifyAction aimCancelled;
    public static event NotifyAction shootingStarted;
    public static event NotifyAction shootingCancelled;

    private MInputManager() { }

    public static MInputManager SharedInstance
    {
        get
        {
            if (instance == null)
            {
                return new MInputManager();
            }
            return instance;
        }
    }

    public void SetPlayerInput(PlayerInput playerInput)
    {
        this.playerInput = playerInput;
        InitActions();
    }

    private void InitActions()
    {
        if (this.playerInput == null) { return; }

        this.playerInput.enabled = true;
        this.moveAction = this.playerInput.actions["Movement"];
        this.sprintAction = this.playerInput.actions["Sprint"];
        this.jumpAction = this.playerInput.actions["Jump"];
        this.aimAction = this.playerInput.actions["Aim"];
        this.shootAction = this.playerInput.actions["Shoot"];

        this.moveAction.performed += HandleMovement;
        this.moveAction.canceled += HandleMovement;

        this.sprintAction.performed += (_) => HandleSprintStart();
        this.sprintAction.canceled += (_) => HandleSprintEnd();

        this.jumpAction.performed += (_) => HandleJump();

        this.aimAction.performed += (_) => HandleAim();
        this.aimAction.canceled += (_) => HandleAimCancel();

        this.shootAction.performed += (_) => HandleShootStart();
        this.shootAction.canceled += (_) => HandleShootEnd();
    }

    private void HandleMovement(InputAction.CallbackContext context)
    {
        movementPressed?.Invoke(context.ReadValue<Vector2>());
    }

    private void HandleSprintStart()
    {
        sprintEngaged.Invoke();
    }

    private void HandleSprintEnd()
    {
        sprintDisengaged.Invoke();
    }

    private void HandleJump()
    {
        jumpTriggered.Invoke();
    }

    private void HandleAim()
    {
        aimEngaged.Invoke();
    }

    private void HandleAimCancel()
    {
        aimCancelled.Invoke();
    }

    private void HandleShootStart()
    {
        shootingStarted.Invoke();
    }

    private void HandleShootEnd()
    {
        shootingCancelled.Invoke();
    }
}