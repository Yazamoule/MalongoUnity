using UnityEngine;
using static Movement;

public class MoveInAirQuake : CoreMoveState
{
    [SerializeField] float speedMax = 320;
    [SerializeField] float accelMax = 0.7f * 60f;


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




    public override void Exit()
    {
        base.Exit();
    }
}
