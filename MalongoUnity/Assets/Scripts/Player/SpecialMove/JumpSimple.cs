using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;
using UnityEngine.InputSystem;
using static Movement;

public class JumpSimple : SpecialMoveState
{
    float bufferClock = 0f;
    [SerializeField] float bufferTime = 0.5f;


    float forceOffGroundClock = 0f;
    [SerializeField] float forceOffGroundTime = 0.5f;

    [SerializeField] float force = 100;

    public override void Init()
    {
        base.Init();

        stateEnum = SpecialEnum.Jump;
        stateMachine.AddState(this);


        #region Transitions
        {//enter jump from any
            FeetEnum[] fromFeet = { FeetEnum.OnGround };
            CoreEnum[] fromCore = null;
            SpecialEnum[] fromSpecial = null;
            SpecialEnum to = SpecialEnum.Jump;
            int priority = 0;

            bool Condition()
            {
                if (to == move.specialMoveEnum)
                    return false;

                if (bufferClock > 0)
                    return true;

                return false;
            }

            Transition<SpecialEnum> transition = new Transition<SpecialEnum>(to, priority, Condition, fromFeet, fromCore, fromSpecial);
            stateMachine.transitions.Add(transition);
        }
        {   //Exit Jump from any
            FeetEnum[] fromFeet = null;
            CoreEnum[] fromCore = null;
            SpecialEnum[] fromSpecial = { SpecialEnum.Jump };
            SpecialEnum to = SpecialEnum.None;
            int priority = 1;

            bool Condition()
            {
                if (to == move.specialMoveEnum)
                    return false;

                return true;

            }

            Transition<SpecialEnum> transition = new Transition<SpecialEnum>(to, priority, Condition, fromFeet, fromCore, fromSpecial);
            stateMachine.transitions.Add(transition);
        }
        #endregion


        gm.input.actions.FindAction("Jump").performed += OnJump;
    }

    public override void Enter()
    {
        base.Enter();
        move.rb.linearVelocity = feet.OverrideVerticalAxis(move.rb.linearVelocity, false, force);

        bufferClock = 0f;

        feet.forceOffGround = true;
        forceOffGroundClock = forceOffGroundTime;
    }

    public override void Execute()
    {
        base.Execute();


    }

    public override void Exit()
    {


        base.Exit();
    }


    private void FixedUpdate()
    {
        if (bufferClock > 0f)
            bufferClock -= Time.fixedDeltaTime;


        if (forceOffGroundClock > 0f)
            forceOffGroundClock -= Time.fixedDeltaTime;
        else if (forceOffGroundClock < 0f)
        {
            forceOffGroundClock = 0f;
            feet.forceOffGround = false;
        }


    }

    public void OnJump(InputAction.CallbackContext context)
    {
        bufferClock = bufferTime;
    }
}
