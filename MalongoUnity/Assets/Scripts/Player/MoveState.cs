using System;
using UnityEngine;
using UnityEngine.Events;

public abstract class MoveState<TEnum> : MonoBehaviour where TEnum : Enum
{
    GameManager gm;
    LevelManager lm;

    protected MoveStateMachine<TEnum> stateMachine;
    protected Feet feet;

    public TEnum stateEnum;

    [SerializeField] bool IsCoreMove = true;

    private void Awake()
    {
        gm = GameManager.Instance;
        lm = LevelManager.Instance;
    }

    private void Start()
    {
        if (IsCoreMove)
            stateMachine = lm.player.move.corMov as MoveStateMachine<TEnum>;
        else
            stateMachine = lm.player.move.specialMov as MoveStateMachine<TEnum>;

        feet = lm.player.move.feet;
    }
    public virtual void Enter(){}
    public virtual void Execute(){}
    public virtual void Exit(){}
}
