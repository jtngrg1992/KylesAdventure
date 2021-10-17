using UnityEngine;
using UnityEngine.Animations.Rigging;
using UnityEngine.InputSystem;

public class Character : MonoBehaviour
{
    public float movementSpeed = 2.0f;
    public float sprintMultiplier = 2.0f;
    public float rotationSpeed = 20.0f;
    public float aimSpeed = 6.0f;
    public Rig weaponAimRigLayer;
    public float gravity = -9.8f;
    public LayerMask groundLayer;

    private CharacterController controller;
    private PlayerInput playerInput;
    private float aimSmoothVelocity;
    private Weapon weapon;
    private Vector3 playerVelocity;
    private float verticalVelocity;
    private StateMachine movementStateMachine;
    public MovingState moving;
    public JumpingState jumping;
    public AimingState aiming;
    public ShootingState shooting;

    [System.NonSerialized]
    public Camera mainCam;
    [System.NonSerialized]
    public Animator animator;
    [System.NonSerialized]
    public int velocityXHash;
    [System.NonSerialized]
    public int velocityYHash;
    [System.NonSerialized]
    public int jumpHash;
    [System.NonSerialized]
    public int groundedHash;

    public PlayerInput PlayerInput
    {
        get
        {
            return playerInput;
        }
    }

    public bool Grounded
    {
        get
        {
            return controller.isGrounded;
        }
    }



    private void Awake()
    {
        controller = GetComponent<CharacterController>();
        playerInput = GetComponent<PlayerInput>();
        mainCam = Camera.main;
        animator = GetComponent<Animator>();
        velocityXHash = Animator.StringToHash("VelocityX");
        velocityYHash = Animator.StringToHash("VelocityY");
        jumpHash = Animator.StringToHash("Jump");
        groundedHash = Animator.StringToHash("Grounded");
        weapon = GetComponentInChildren<Weapon>();
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        movementStateMachine = new StateMachine();
        moving = new MovingState(this, movementStateMachine);
        jumping = new JumpingState(this, movementStateMachine);
        aiming = new AimingState(this, movementStateMachine);
        shooting = new ShootingState(this, movementStateMachine);
        movementStateMachine.Initialize(moving);
        Debug.Log("Setting player Intut");
        MInputManager.SharedInstance.SetPlayerInput(playerInput);
    }


    private void Update()
    {
        movementStateMachine.CurrentState.HandleUpdate();
    }


    private void FixedUpdate()
    {
        movementStateMachine.CurrentState.HandlePhysicsUpdate();
        RotatePlayer();
    }

    private void RotatePlayer()
    {
        float cameraFace = mainCam.transform.rotation.eulerAngles.y;
        transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(0, cameraFace, 0), Time.deltaTime * rotationSpeed);
    }

    public void MoveGround(Vector2 input)
    {
        if (IsGrounded() && verticalVelocity < 0)
        {
            verticalVelocity = -2f;
        }

        playerVelocity = mainCam.transform.right * input.x + mainCam.transform.forward * input.y;
        playerVelocity.y = 0;
        playerVelocity *= movementSpeed;

        animator.SetFloat(velocityXHash, input.x);
        animator.SetFloat(velocityYHash, input.y);

        verticalVelocity += gravity * Time.deltaTime;
        controller.Move((playerVelocity + new Vector3(0, verticalVelocity, 0)) * Time.deltaTime);
    }

    public void JumpToHeight(float height)
    {
        verticalVelocity += height;
    }

    public void StartAiming()
    {
        weaponAimRigLayer.weight = Mathf.SmoothDamp(weaponAimRigLayer.weight, 1, ref aimSmoothVelocity, Time.deltaTime * aimSpeed);
    }

    public void EndAiming()
    {
        weaponAimRigLayer.weight = Mathf.SmoothDamp(weaponAimRigLayer.weight, 0, ref aimSmoothVelocity, Time.deltaTime * aimSpeed);
    }

    public bool IsGrounded()
    {
        Vector3 spherePosition = transform.position;
        bool grounded = Physics.CheckSphere(spherePosition, 0.28f, groundLayer, QueryTriggerInteraction.Ignore);
        return grounded;
    }

    public void StartShooting()
    {
        weapon.StartFiring();
    }

    public void StopShooting()
    {
        weapon.StopFiring();
    }

}