using UnityEngine;
using static Movement;

public class MoveInAirQuake : CoreMoveState
{
    [SerializeField] float speedMax = 100;
    [SerializeField] float accelMax = 10;

    Vector3 velocity = Vector3.zero;


    public override void Init()
    {
        base.Init();

        stateEnum = CoreEnum.RunInAir;
        stateMachine.AddState(this);


        {
            //createing Transition To Enter Run in air
            FeetEnum[] fromFeet = new[] { FeetEnum.OffGround, FeetEnum.ToSteepGround };
            CoreEnum[] fromCore = null;
            SpecialEnum[] fromSpecial = null;
            CoreEnum to = CoreEnum.RunInAir;
            int priority = 0;

            bool Condition()
            {
                if (to == move.coreMoveEnum)
                    return false;

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

        velocity = feet.OverrideVerticalAxis(move.rb.linearVelocity, false);

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

        Vector3 addSpeedVector = addSpeed * move.wishDir;

        move.rb.linearVelocity += addSpeedVector;

        gm.DebugLine(false, "AirMoveAccel", Color.green, addSpeedVector * 3f);
    }

    //private void Decelerate()
    //{
    //    float speed = move.rb.linearVelocity.magnitude;

    //    if (speed == 0)
    //        return;

    //    //dynamic friction and static friction, 2 in 1 good.
    //    //calculate the friction on a higher value if you go too slow
    //    float capedHSpeed = speed < frictionCap ? frictionCap : speed;

    //    //attention complicate try to think
    //    Vector3 frictionVector = feet.OverrideVerticalAxis(move.rb.linearVelocity.normalized, true) * capedHSpeed * -friction * Time.fixedDeltaTime;


    //    if ((frictionVector.SquaredLength() > move.rb.linearVelocity.SquaredLength()))
    //        return -_nextVelocity;

    //    move.rb.linearVelocity += frictionVector;
    //}


    public override void Exit()
    {
        base.Exit();
    }
}
