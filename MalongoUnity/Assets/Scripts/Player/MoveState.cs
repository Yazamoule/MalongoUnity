using System;
using UnityEngine;
using UnityEngine.Events;

public abstract class MoveState<TEnum> : MonoBehaviour where TEnum : Enum
{
    GameManager gm;
    LevelManager lm;

    protected MoveStateMachine<TEnum> stateMachine;
    protected Feet feet;

    [HideInInspector] public TEnum stateEnum;

    [SerializeField] bool IsCoreMove = true;

    public virtual void Init() 
    {
        gm = GameManager.Instance;
        lm = LevelManager.Instance;

        if (IsCoreMove)
            stateMachine = lm.player.move.corMove as MoveStateMachine<TEnum>;
        else
            stateMachine = lm.player.move.specialMove as MoveStateMachine<TEnum>;

        feet = lm.player.move.feet;
    }
    public virtual void Enter(){}
    public virtual void Execute(){}
    public virtual void Exit(){}
}
