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

    public List<Transition<Movement.SpecialEnum>> transitions = new List<Transition<Movement.SpecialEnum>>();

    public SpecialMoveStateMachine()
    {
        // Initialize IMoveState array
        states = new SpecialMoveState[(int)Movement.SpecialEnum.Max];
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

    public void ChangeState(Movement.SpecialEnum newStateEnum)
    {
        // Exit the current state
        currentState.Exit();

        // Update state enums
        move.specialMoveEnum = newStateEnum;

        // Enter the new state
        currentState = states[(int)move.specialMoveEnum];
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
        move.backSpecialMoveEnum = move.specialMoveEnum;

        ProcessTransition();

        currentState.Execute();
    }
}