using UnityEngine;

public abstract class CoreMoveState : MonoBehaviour
{
    GameManager gm;
    LevelManager lm;

    protected CoreMoveStateMachine stateMachine;
    protected Feet feet;

    [HideInInspector] public Movement.CoreMoveEnum stateEnum;

    public virtual void Init() 
    {
        gm = GameManager.Instance;
        lm = LevelManager.Instance;

        stateMachine = lm.player.move.coreMove;
        feet = lm.player.move.feet;
    }
    public virtual void Enter(){}
    public virtual void Execute(){}
    public virtual void Exit(){}
}
