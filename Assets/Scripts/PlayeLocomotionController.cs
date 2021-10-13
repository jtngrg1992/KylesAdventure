using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(AnimationStateController))]
public class PlayeLocomotionController : MonoBehaviour
{
    private ThirdPersonInputActions inputActions;
    private Vector2 movementDirection;
    private bool isSprinting;
    private CharacterController characterController;
    private AnimationStateController animationStateController;
    private Vector2 smoothInputVelocity;

    private void OnEnable()
    {
        inputActions.Player.Enable();
    }

    private void OnDisable()
    {
        inputActions.Player.Disable();
    }

    private void Awake()
    {
        inputActions = new ThirdPersonInputActions();
        characterController = GetComponent<CharacterController>();
        animationStateController = GetComponent<AnimationStateController>();
    }


    void Start()
    {
        inputActions.Player.Movement.started += HandleMovementInput;
        inputActions.Player.Movement.canceled += HandleMovementInput;
        inputActions.Player.Movement.performed += HandleMovementInput;
        inputActions.Player.Sprint.performed += HandleSprint;
    }

    void FixedUpdate()
    {
        animationStateController.ProcessDirectionalVector(movementDirection, isSprinting);
    }


    private void HandleMovementInput(InputAction.CallbackContext context)
    {
        movementDirection = context.ReadValue<Vector2>();
    }

    private void HandleSprint(InputAction.CallbackContext context)
    {
        float value = context.ReadValue<float>();
        isSprinting = Mathf.Approximately(value, 1.0f);
    }
}
