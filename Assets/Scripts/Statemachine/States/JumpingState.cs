using UnityEngine;

public class JumpingState : State
{
    public JumpingState(Character character, StateMachine stateMachine) : base(character, stateMachine)
    {

    }

    public override void Enter()
    {
        base.Enter();
        float jumpHeight = Mathf.Sqrt(2 * -2f * character.gravity);
        character.animator.SetTrigger(character.jumpHash);
        character.JumpToHeight(jumpHeight);
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void HandlePhysicsUpdate()
    {
        base.HandlePhysicsUpdate();
        if (character.Grounded)
        {
            character.animator.SetTrigger(character.groundedHash);
            stateMachine.GoBackToPrevState();
        }
    }


}