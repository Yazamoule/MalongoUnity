using MoveEnum = Movement.CoreEnum;
using SpecialEnum = Movement.SpecialEnum;

public class SpecialMoveNone : SpecialMoveState
{
    public override void Init()
    {
        base.Init();


        stateEnum = SpecialEnum.None;
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
