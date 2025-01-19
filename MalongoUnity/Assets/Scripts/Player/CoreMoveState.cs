using UnityEngine;

public abstract class CoreMoveState : MonoBehaviour
{
    protected GameManager gm;
    protected LevelManager lm;

    protected CoreMoveStateMachine stateMachine;
    protected Feet feet;
    protected Movement move;

    [HideInInspector] public Movement.CoreEnum stateEnum;

    public virtual void Init() 
    {
        gm = GameManager.Instance;
        lm = LevelManager.Instance;

        stateMachine = lm.player.move.coreMove;
        move = lm.player.move;
        feet = move.feet;
    }
    public virtual void Enter(){}
    public virtual void Execute(){}
    public virtual void Exit(){}
}
