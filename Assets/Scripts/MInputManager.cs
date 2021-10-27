using UnityEngine.InputSystem;
using UnityEngine;



public delegate void NotifyMovement(Vector2 movement);
public delegate void NotifyAction();

class MInputManager
{
    private PlayerInput playerInput;
    private static MInputManager instance;

    private InputAction moveAction;
    private InputAction sprintAction;
    private InputAction jumpAction;
    private InputAction aimAction;
    private InputAction shootAction;
    private InputAction holsterAction;
    private InputAction switchAction;

    public static event NotifyMovement movementPressed;
    public static event NotifyAction sprintEngaged;
    public static event NotifyAction sprintDisengaged;
    public static event NotifyAction jumpTriggered;
    public static event NotifyAction aimEngaged;
    public static event NotifyAction aimCancelled;
    public static event NotifyAction shootingStarted;
    public static event NotifyAction shootingCancelled;
    public static event NotifyAction holsterRequested;
    public static event NotifyAction switchRequested;

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
        this.holsterAction = this.playerInput.actions["HolsterWeapon"];
        this.switchAction = this.playerInput.actions["SwitchWeapon"];

        this.moveAction.performed += HandleMovement;
        this.moveAction.canceled += HandleMovement;

        this.sprintAction.performed += (_) => HandleSprintStart();
        this.sprintAction.canceled += (_) => HandleSprintEnd();

        this.jumpAction.performed += (_) => HandleJump();

        this.aimAction.performed += (_) => HandleAim();
        this.aimAction.canceled += (_) => HandleAimCancel();

        this.shootAction.performed += (_) => HandleShootStart();
        this.shootAction.canceled += (_) => HandleShootEnd();

        this.holsterAction.performed += (_) => HandleHolster();
        this.switchAction.performed += (_) => HandleSwitch();
    }

    private void HandleMovement(InputAction.CallbackContext context)
    {
        movementPressed?.Invoke(context.ReadValue<Vector2>());
    }

    private void HandleSprintStart()
    {
        sprintEngaged?.Invoke();
    }

    private void HandleSprintEnd()
    {
        sprintDisengaged?.Invoke();
    }

    private void HandleJump()
    {
        jumpTriggered?.Invoke();
    }

    private void HandleAim()
    {
        aimEngaged?.Invoke();
    }

    private void HandleAimCancel()
    {
        aimCancelled?.Invoke();
    }

    private void HandleShootStart()
    {
        shootingStarted?.Invoke();
    }

    private void HandleShootEnd()
    {
        shootingCancelled?.Invoke();
    }

    private void HandleHolster()
    {
        holsterRequested?.Invoke();
    }

    private void HandleSwitch()
    {
        switchRequested?.Invoke();
    }
}