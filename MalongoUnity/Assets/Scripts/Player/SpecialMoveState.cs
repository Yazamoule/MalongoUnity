using UnityEngine;

public abstract class SpecialMoveState : MonoBehaviour
{
    protected GameManager gm;
    protected LevelManager lm;

    protected SpecialMoveStateMachine stateMachine;
    protected Feet feet;
    protected Movement move;


    [HideInInspector] public Movement.SpecialEnum stateEnum;

    public virtual void Init()
    {
        gm = GameManager.Instance;
        lm = LevelManager.Instance;

        move = lm.player.move;
        stateMachine = move.specialMove;
        feet = lm.player.move.feet;
    }
    public virtual void Enter() { }
    public virtual void Execute() { }
    public virtual void Exit() { }
}
