using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Animations.Rigging;
using System.Collections;

[RequireComponent(typeof(PlayerInput))]
[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{
    [SerializeField]
    private float playerSpeed = 2.0f;
    [SerializeField]
    private float rotationSpeed = 10.0f;
    [SerializeField]
    private float aimSpeed = 6.0f;
    [SerializeField]
    private Rig weaponAimRigLayer;

    private CharacterController controller;
    private Vector3 verticalVelocity = Vector3.zero;
    private Vector3 playerVelocity = Vector3.zero;
    private Quaternion playerRotation = Quaternion.identity;
    private MoveInputProcessor moveInputProcessor;
    private PlayerInput playerInput;
    private bool groundedPlayer;
    private Camera mainCam;
    private Animator animator;
    private int velocityXHash;
    private int velocityYHash;
    private int jumpAnimationHash;
    private int groundedHash;
    private bool isSprinting;
    private bool isAiming = false;
    private Vector2 currentMovementBlend;
    private Vector2 movementBlendVelocity;


    private InputAction lookAction;
    private InputAction aimAction;
    private InputAction shootAction;
    private float aimSmoothVelocity;

    private bool didStartJumping = false;


    private void Awake()
    {
        controller = GetComponent<CharacterController>();
        playerInput = GetComponent<PlayerInput>();
        moveInputProcessor = new MoveInputProcessor(playerInput);
        animator = GetComponent<Animator>();
        BindInputActions();
        mainCam = Camera.main;
        velocityXHash = Animator.StringToHash("VelocityX");
        velocityYHash = Animator.StringToHash("VelocityY");
        groundedHash = Animator.StringToHash("Grounded");
        jumpAnimationHash = Animator.StringToHash("Jump");
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

    }

    private void OnEnable()
    {

        moveInputProcessor.EnableControls();
        aimAction.performed += _ => HandleAim();
        aimAction.canceled += _ => CancelAim();
    }

    private void OnDisable()
    {
        moveInputProcessor.DisableControls();
        aimAction.performed -= _ => HandleAim();
        aimAction.canceled -= _ => CancelAim();
    }


    private void BindInputActions()
    {
        lookAction = playerInput.actions["Look"];
        shootAction = playerInput.actions["Shoot"];
        aimAction = playerInput.actions["Aim"];
    }

    private void Update()
    {
        HandleMovement();
        Move();
        HandleRotation();
        Rotate();
        HandleAimAnimation();
    }

    private void HandleSprintInput(InputAction.CallbackContext context)
    {
        isSprinting = context.ReadValue<float>() == 1;
    }

    private void HandleAim()
    {
        isAiming = true;
    }

    private void CancelAim()
    {
        isAiming = false;
    }


    private void HandleMovement()
    {
        groundedPlayer = controller.isGrounded;
        if (groundedPlayer && verticalVelocity.y < 0)
        {
            verticalVelocity.y = -2f;
        }

        Vector2 smoothInput = moveInputProcessor.SmoothInput;
        playerVelocity = mainCam.transform.right * smoothInput.x + mainCam.transform.forward * smoothInput.y;
        playerVelocity.y = 0;
        playerVelocity *= playerSpeed;
    }

    private void Move()
    {
        animator.SetFloat(velocityXHash, moveInputProcessor.SmoothInput.x);
        animator.SetFloat(velocityYHash, moveInputProcessor.SmoothInput.y);

        if (moveInputProcessor.IsJumping && controller.isGrounded)
        {
            animator.SetTrigger(jumpAnimationHash);
            animator.SetBool(groundedHash, false);
            verticalVelocity.y += moveInputProcessor.JumpHeight;
            didStartJumping = true;

        }

        if (didStartJumping)
        {
            StartCoroutine(WaitForJumpToFinish());
        }

        // apply gravity
        verticalVelocity.y += moveInputProcessor.Gravity * Time.deltaTime;
        controller.Move((playerVelocity + new Vector3(0, verticalVelocity.y, 0)) * Time.deltaTime);
    }

    private IEnumerator WaitForJumpToFinish()
    {
        yield return new WaitForSeconds(0.8f);
        animator.SetBool(groundedHash, true);
        didStartJumping = false;
    }

    private void Rotate()
    {
        transform.rotation = playerRotation;
    }

    private void HandleRotation()
    {
        float cameraFacing = mainCam.transform.rotation.eulerAngles.y;
        playerRotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(0, cameraFacing, 0), Time.deltaTime * rotationSpeed);
    }

    private void HandleAimAnimation()
    {
        float targetValue = isAiming ? 1 : 0;
        if (weaponAimRigLayer.weight != targetValue)
        {
            weaponAimRigLayer.weight = Mathf.SmoothDamp(weaponAimRigLayer.weight, targetValue, ref aimSmoothVelocity, Time.deltaTime * aimSpeed);
        }
    }
}
