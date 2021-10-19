using UnityEngine;
using UnityEngine.Animations.Rigging;

public class CharacterStateMachine : MStateMachine
{
    public float walkingSpeed = 2.0f;
    public float sprintMultiplier = 3.0f;
    public float rotationSpeed = 20.0f;
    public float aimSpeed = 6.0f;
    public float gravity = -9.8f;
    public float movementInputSmoothTime = 0.2f;
    public Rig weaponAimRigLayer;

    [HideInInspector]
    public int velocityXHash;
    [HideInInspector]
    public int velocityYHash;
    [HideInInspector]
    public MIdle idleState;
    [HideInInspector]
    public MWalking walkingState;
    [HideInInspector]
    public MSprinting sprintingState;
    [HideInInspector]
    public MAiming aimingState;
    [HideInInspector]
    public Vector2 movementInput;
    [HideInInspector]
    public float verticalVelocity;
    [HideInInspector]
    public Vector3 playerVelocity;
    [HideInInspector]
    public Animator animator;

    private CharacterController controller;
    public CharacterController Controller { get { return controller; } }
    private Camera mainCam;
    public Camera MainCam { get { return mainCam; } }
    public BaseState ActiveState { get { return currentState; } }

    private void Awake()
    {
        idleState = new MIdle(this);
        walkingState = new MWalking(this);
        sprintingState = new MSprinting(this);
        aimingState = new MAiming(this);
        controller = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
        mainCam = Camera.main;
        velocityXHash = Animator.StringToHash("VelocityX");
        velocityYHash = Animator.StringToHash("VelocityY");
    }

    protected override BaseState GetInitialState()
    {
        return idleState;
    }
}