using UnityEngine;
using UnityEngine.InputSystem;


[RequireComponent(typeof(PlayerInput))]
public class MStateMachine : MonoBehaviour
{
    protected BaseState currentState;
    protected BaseState previousState;
    private PlayerInput playerInput;


    void Start()
    {
        playerInput = GetComponent<PlayerInput>();
        MInputManager.SharedInstance.SetPlayerInput(playerInput);

        var cs = GetInitialState();
        if (cs != null)
        {
            ActivateState(cs);
        }
    }

    void Update()
    {
        currentState.Update();
    }

    void FixedUpdate()
    {
        currentState.PhysicsUpdate();
    }


    public void ActivateState(BaseState state)
    {

        if (currentState == null || currentState != state)
        {
            if (currentState != null)
            {
                previousState = currentState;
                currentState.Exit();
            }

            state.Enter();
            currentState = state;
        }
    }

    public void UnWind()
    {
        if (previousState != null)
        {
            ActivateState(previousState);
        }
    }

    private void OnGUI()
    {
        string content = currentState != null ? currentState.name : "(no current state)";
        GUILayout.Label($"<color='black'><size=40>{content}</size></color>");
    }

    protected virtual BaseState GetInitialState() { return null; }
}