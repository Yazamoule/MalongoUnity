using MoveEnum = Movement.CoreMoveEnum;
using SpecialEnum = Movement.SpecialMoveEnum;

public class RunOnGround : CoreMoveState
{

    public override void Init()
    {
        base.Init();

        stateEnum = MoveEnum.RunOnGround;
        stateMachine.AddState(this);


        //createing Transition
        MoveEnum[] fromCore = new[] { MoveEnum.RunOnGround };
        SpecialEnum[] fromSpecial = null;
        MoveEnum to = MoveEnum.None;
        int priority = -5;

        bool IsReadyToRunOnGround()
        {
            return true;
        }

        Transition<MoveEnum> transition = new Transition<MoveEnum>( to, priority, IsReadyToRunOnGround, fromCore, fromSpecial);
        stateMachine.transitions.Add(transition);
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
