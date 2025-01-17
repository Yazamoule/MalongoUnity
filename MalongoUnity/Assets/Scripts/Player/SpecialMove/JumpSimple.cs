using UnityEngine;
using UnityEngine.InputSystem;
using static Movement;

public class JumpSimple : SpecialMoveState
{
    bool iWantToJump = false;
    [SerializeField] float force = 100;

    public override void Init()
    {
        base.Init();

        stateEnum = SpecialEnum.Jump;
        stateMachine.AddState(this);


        #region Transitions
        {//enter jump from any
            CoreEnum[] fromCore = null;
            SpecialEnum[] fromSpecial = null;
            SpecialEnum to = SpecialEnum.Jump;
            int priority = 0;

            bool Condition()
            {
                if (move.feetEnum == FeetEnum.OnGround && iWantToJump)
                    return true;

                //reset i want to jump
                iWantToJump = false;
                return false;
            }

            Transition<SpecialEnum> transition = new Transition<SpecialEnum>(to, priority, Condition, fromCore, fromSpecial);
            stateMachine.transitions.Add(transition);
        }
        {   //Exit Jump from any
            CoreEnum[] fromCore = null;
            SpecialEnum[] fromSpecial = { SpecialEnum.Jump };
            SpecialEnum to = SpecialEnum.None;
            int priority = 1;

            bool Condition()
            {
                return true;
            }

            Transition<SpecialEnum> transition = new Transition<SpecialEnum>(to, priority, Condition, fromCore, fromSpecial);
            stateMachine.transitions.Add(transition);
        }
        #endregion


        gm.input.actions.FindAction("Jump").performed += OnJump;
    }

    public override void Enter()
    {
        base.Enter();
        move.rb.linearVelocity += Vector3.up * force;
    }

    public override void Execute()
    {
        base.Execute();
    }

    public override void Exit()
    {
        //reset i want to jump
        iWantToJump = false;


        base.Exit();
    }

    private void Update()
    {

    }

    public void OnJump(InputAction.CallbackContext context)
    {
        iWantToJump = true;
    }
}
