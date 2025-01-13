using MoveEnum = Movement.CorMovStateEnum;

public class RunOnGround : MoveState<MoveEnum>
{
    private void Start()
    {
        stateEnum = MoveEnum.RunOnGround;
        stateMachine.AddState(this);


        //createing Transition
        MoveEnum[] from = new[] { MoveEnum.RunOnGround};
        MoveEnum to = MoveEnum.RunInAir;
        int priority = -5;
        bool IsReadyToRunOnGround()
        {
            return true;
        }
        Transition<MoveEnum> transition = new Transition<MoveEnum>(from, to, priority, IsReadyToRunOnGround);
        stateMachine.transitions.Add(transition);
    }

    public override void Enter()
    {
        throw new System.NotImplementedException();
    }

    public override void Execute()
    {
        throw new System.NotImplementedException();
    }

    public override void Exit()
    {
        throw new System.NotImplementedException();
    }
}
