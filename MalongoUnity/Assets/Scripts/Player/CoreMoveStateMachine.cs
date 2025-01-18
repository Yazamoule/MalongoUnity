using NUnit.Framework;
using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class CoreMoveStateMachine
{
    Movement move;

    CoreMoveState[] states;
    CoreMoveState currentState = null;

    public List<Transition<Movement.CoreEnum>> transitions = new List<Transition<Movement.CoreEnum>>();

    public CoreMoveStateMachine()
    {
        // Initialize IMoveState array
        states = new CoreMoveState[(int)Movement.CoreEnum.Max];
    }

    public void LateInit()
    {
        move = LevelManager.Instance.player.move; 
        currentState = states[(int)move.coreMoveEnum];
    }

    public void AddState(CoreMoveState _state)
    {
        states[(int)_state.stateEnum] = _state;
    }

    public void ChangeState(Movement.CoreEnum newStateEnum)
    {
        // Exit the current state
        currentState.Exit();

        // Update state enums
        move.coreMoveEnum = newStateEnum;

        // Enter the new state
        currentState = states[(int)move.coreMoveEnum];
        currentState.Enter();
    }

    public void ProcessTransition()
    {
        foreach (var transition in transitions)
        {
            if (transition.Condition())
            {
                ChangeState(transition.to);
                return;
            }
        }

        currentState.Execute();
    }

    public void Update()
    {
        move.backCoreEnum = move.coreMoveEnum;

        ProcessTransition();
        currentState.Execute();
    }
}