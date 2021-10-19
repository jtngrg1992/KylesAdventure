using UnityEngine;
using UnityEngine.InputSystem;
using Cinemachine;

public class SwitchVCam : MonoBehaviour
{
    [SerializeField]
    private PlayerInput playerInput;
    [SerializeField]
    private int priorityBoostAmount = 10;
    [SerializeField]
    private Canvas thirdPersonCanvas;
    [SerializeField]
    private Canvas aimCanvas;

    private InputAction aimAction;
    private CinemachineVirtualCamera virtualCamera;

    void Awake()
    {
        aimAction = playerInput.actions["Aim"];
        virtualCamera = GetComponent<CinemachineVirtualCamera>();
        aimCanvas.enabled = false;

    }

    void OnEnable()
    {
        aimAction.performed += _ => StartAim();
        aimAction.canceled += _ => CancelAim();
    }

    private void OnDisable()
    {
        aimAction.performed -= _ => StartAim();
        aimAction.canceled -= _ => CancelAim();

    }

    private void StartAim()
    {
        virtualCamera.Priority += priorityBoostAmount;
        thirdPersonCanvas.enabled = false;
        aimCanvas.enabled = true;
    }

    private void CancelAim()
    {
        virtualCamera.Priority -= priorityBoostAmount;
        thirdPersonCanvas.enabled = true;
        aimCanvas.enabled = false;
    }
}
