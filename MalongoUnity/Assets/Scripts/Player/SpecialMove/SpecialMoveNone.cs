using MoveEnum = Movement.CoreMoveEnum;
using SpecialEnum = Movement.SpecialMoveEnum;

public class SpecialMoveNone : SpecialMoveState
{
    public override void Init()
    {
        base.Init();

  
        stateMachine.AddState(this);
    }

    public override void Enter()
    {
        base.Enter();
    }

    public override void Execute()
    {
        base.Execute();
    }

    public override void Exit()
    {
        base.Exit();
    }
}
