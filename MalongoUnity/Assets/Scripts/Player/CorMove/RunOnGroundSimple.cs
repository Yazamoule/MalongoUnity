using UnityEngine;
using static Movement;

using MoveEnum = Movement.CoreEnum;
using SpecialEnum = Movement.SpecialEnum;

public class RunOnGroundSimple : CoreMoveState
{
    [SerializeField] float acceleration = 10;
    [SerializeField] float resistence = 3;
    [SerializeField] float maxSpeed = 300f;
    public override void Init()
    {
        base.Init();

        stateEnum = MoveEnum.RunOnGround;
        stateMachine.AddState(this);


        //createing Transition
        MoveEnum[] fromCore = new[] { MoveEnum.None };
        SpecialEnum[] fromSpecial = null;
        MoveEnum to = MoveEnum.RunOnGround;
        int priority = -10;

        bool Condition()
        {
            if (move.feetEnum == FeetEnum.OnGround)
                return true;

            return false;
        }

        Transition<MoveEnum> transition = new Transition<MoveEnum>(to, priority, Condition, fromCore, fromSpecial);
        stateMachine.transitions.Add(transition);
    }

    public override void Enter()
    {
        base.Enter();
    }

    public override void Execute()
    {
        base.Execute();

        Accelerate();
        Decelerate();
    }

    private void Accelerate()
    {
        Vector3 velocity = move.rb.linearVelocity + move.wishDir * acceleration * Time.fixedDeltaTime;

        if (velocity.sqrMagnitude > maxSpeed * maxSpeed)
        {
            //Clamp velocity
            velocity = velocity.normalized * maxSpeed;
        } 

        move.rb.linearVelocity = velocity;
    }

    private void Decelerate()
    {
        move.rb.linearVelocity += move.rb.linearVelocity * -resistence * Time.fixedDeltaTime;
    }

    public override void Exit()
    {
        base.Exit();
    }
}
