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
        Vector3 accel = (feet.groundRQuat * move.wishDir) * acceleration * Time.fixedDeltaTime;
        Vector3 velocity = move.rb.linearVelocity + accel;

        //DebugCustom.DrawRay(accel, 5, Color.blue);

        if (velocity.sqrMagnitude > maxSpeed * maxSpeed)
        {
            //Clamp velocity
            velocity = velocity.normalized * maxSpeed;
        }

        move.rb.linearVelocity = velocity;
    }

    private void Decelerate()
    {
        Vector3 horizontalVelocity = feet.OverrideVerticalAxis(move.rb.linearVelocity - feet.verticalSpringSpeed * Vector3.up, true);
        move.rb.linearVelocity += horizontalVelocity * -resistence * Time.fixedDeltaTime;
    }

    public override void Exit()
    {
        base.Exit();
    }
}
