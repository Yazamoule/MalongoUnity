using NUnit.Framework;
using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class SpecialMoveStateMachine
{
    Movement move;

    SpecialMoveState[] states;
    SpecialMoveState currentState = null;

    public List<Transition<Movement.CoreMoveEnum>> transitions = new List<Transition<Movement.CoreMoveEnum>>();

    public SpecialMoveStateMachine()
    {
        // Initialize IMoveState array
        states = new SpecialMoveState[(int)Movement.SpecialMoveEnum.Max];
    }

    public void LateInit()
    {
        move = LevelManager.Instance.player.move;
        currentState = states[(int)move.specialMoveEnum];
    }

    public void AddState(SpecialMoveState _state)
    {
        states[(int)_state.stateEnum] = _state;
    }

    public void ChangeState(Movement.CoreMoveEnum newStateEnum)
    {
        // Exit the current state
        currentState.Exit();

        // Update state enums
        move.backCoreEnum = move.coreMoveEnum;
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
        ProcessTransition();

        currentState.Execute();
    }
}