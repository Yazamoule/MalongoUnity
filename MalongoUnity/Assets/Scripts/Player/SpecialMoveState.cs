using UnityEngine;

public abstract class SpecialMoveState : MonoBehaviour
{
    GameManager gm;
    LevelManager lm;

    protected SpecialMoveStateMachine stateMachine;
    protected Feet feet;

    [HideInInspector] public Movement.SpecialMoveEnum stateEnum;

    public virtual void Init() 
    {
        gm = GameManager.Instance;
        lm = LevelManager.Instance;

        stateMachine = lm.player.move.specialMove;
        feet = lm.player.move.feet;
    }
    public virtual void Enter(){}
    public virtual void Execute(){}
    public virtual void Exit(){}
}
