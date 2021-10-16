public class StateMachine
{
    private State currentState;

    public State CurrentState
    {
        get
        {
            return currentState;
        }
    }

    private State previousState;

    public void ChangeState(State nextState)
    {
        previousState = currentState;
        currentState.Exit();
        currentState = nextState;
        currentState.Enter();
    }

    public void Initialize(State startingState)
    {
        currentState = startingState;
        startingState.Enter();
    }

    public void GoBackToPrevState()
    {
        State temp = previousState;
        previousState = currentState;
        currentState = temp;

        previousState.Exit();
        currentState.Enter();
    }
}