using MoveEnum = Movement.CorMoveStateEnum;

public class CorMoveNone : MoveState<MoveEnum>
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
    }

    public override void Exit()
    {
     base.Exit();
    }
}
