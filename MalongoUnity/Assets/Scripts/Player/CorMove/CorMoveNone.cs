using MoveEnum = Movement.CoreMoveEnum;
using SpecialEnum = Movement.SpecialMoveEnum;

public class CorMoveNone : CoreMoveState
{
    public override void Init()
    {
        base.Init();
        stateEnum = MoveEnum.None;
        stateMachine.AddState(this);
    }

    public override void Enter()
    {
        base.Enter();
    }

    public override void Execute()
    {
        base.Execute();

        //air
    }

    public override void Exit()
    {
     base.Exit();
    }
}
