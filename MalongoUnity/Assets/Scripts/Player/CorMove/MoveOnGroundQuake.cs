using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.Windows;
using static Movement;

public class MoveOnGroundQuake : CoreMoveState
{
    [SerializeField] float speedMax = 100;
    [SerializeField] float accelMax = 10;

    Vector3 velocity = Vector3.zero;

    [SerializeField] float frictionCap = 20;
    [SerializeField] float friction = 1f;


    public override void Init()
    {
        base.Init();

        stateEnum = CoreEnum.RunOnGround;
        stateMachine.AddState(this);


        {
            //createing Transition To Enter Run On Ground
            FeetEnum[] fromFeet = new[] { FeetEnum.OnGround };
            CoreEnum[] fromCore = null;
            SpecialEnum[] fromSpecial = null;
            CoreEnum to = CoreEnum.RunOnGround;
            int priority = 0;

            bool Condition()
            {
                return true;
            }

            Transition<CoreEnum> transition = new Transition<CoreEnum>(to, priority, Condition, fromFeet, fromCore, fromSpecial);
            stateMachine.transitions.Add(transition);
        }
    }

    public override void Enter()
    {
        base.Enter();
    }

    public override void Execute()
    {
        base.Execute();

        Vector3 velocityRaw = move.rb.linearVelocity;
        gm.DebugLine(false, "rawVelocity", Color.magenta, velocityRaw * 0.3f);
        gm.DebugLine(false, "Velocity minus SpringSpeed", Color.red, (velocityRaw - Vector3.up * feet.verticalSpringSpeed) * 0.3f);
        gm.DebugLine(false, "Velocity AcountForGround", Color.yellow, feet.OverrideVerticalAxis(move.rb.linearVelocity, true) * 0.3f);
        gm.DebugLine(false, "Velocity AcountForGround minus SpringSpeed", Color.blue, (feet.OverrideVerticalAxis(move.rb.linearVelocity, true) - Vector3.up * feet.verticalSpringSpeed) * 0.3f);



        velocity = feet.OverrideVerticalAxis(move.rb.linearVelocity, true) - Vector3.up * feet.verticalSpringSpeed;


        Decelerate();
        Accelerate();
    }

    private void Accelerate()
    {
        //scale wishDir so you can calculate current speed
        Vector3 wishVel = (feet.groundRQuat * move.wishDir) * speedMax;

        //thanks for this line, Amen. 
        //Current Speed is not rly the speed it's the projection of you wishVel(input * speedMax), on to your current velocity.
        //We yeet your current vertical velocity so going up or down slop wont make you gain speed, the only diference will be gravity.
        float currentSpeed = Vector3.Dot(velocity, wishVel) / speedMax;

        //Unclamped add speed is just the diference btw current speed and max speed
        float unclampedAddSpeed = speedMax - currentSpeed;

        //Clamp addSpeed so it dont exede max ground accel
        float addSpeed = Mathf.Clamp(unclampedAddSpeed, 0, accelMax * Time.fixedDeltaTime);

        Vector3 addSpeedVector = addSpeed * (feet.groundRQuat * move.wishDir);

        move.rb.linearVelocity += addSpeedVector;

        gm.DebugLine(false, "GroundAccel", Color.green, addSpeedVector * 3f);
    }

    private void Decelerate()
    {
        float sqrSpeed = velocity.sqrMagnitude;

        if (sqrSpeed == 0)
            return;

        //dynamic friction and static friction, 2 in 1 good.
        //calculate the friction on a higher value if you go too slow
        float capedHSpeed = sqrSpeed < frictionCap * frictionCap ? frictionCap : Mathf.Sqrt(sqrSpeed);

        //attention complicate try to think
        Vector3 frictionVector = (move.rb.linearVelocity - Vector3.up * feet.verticalSpringSpeed).normalized * capedHSpeed * -friction * Time.fixedDeltaTime;

        //static friction wen the friciton is to strong just stop
        if ((frictionVector.sqrMagnitude > sqrSpeed))
            frictionVector = -velocity;
     
        move.rb.linearVelocity += frictionVector;

        gm.DebugLine(false, "friction", Color.cyan, frictionVector * 3f);
    }


    public override void Exit()
    {
        base.Exit();
    }
}
