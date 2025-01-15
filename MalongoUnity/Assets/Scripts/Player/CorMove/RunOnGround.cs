using MoveEnum = Movement.CorMoveStateEnum;
using SpecialEnum = Movement.SpecialMoveStateEnum;

public class RunOnGround : MoveState<MoveEnum>
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
        Transition<MoveEnum> transition = new Transition<MoveEnum>(fromCore, fromSpecial, to, priority, IsReadyToRunOnGround);
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
