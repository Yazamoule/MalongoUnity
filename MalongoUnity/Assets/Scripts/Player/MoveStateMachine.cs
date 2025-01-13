using NUnit.Framework;
using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class MoveStateMachine<TEnum> where TEnum : Enum
{
    public TEnum currentStateEnum;
    public TEnum backStateEnum;

    MoveState<TEnum>[] states;
    MoveState<TEnum> currentState = null;

    public List<Transition<TEnum>> transitions = new List<Transition<TEnum>>();

    public MoveStateMachine()
    {
        // Initialize enums to default values
        currentStateEnum = default(TEnum);
        backStateEnum = currentStateEnum;
        // Initialize IMoveState array
        states = new MoveState<TEnum>[Enum.GetValues(typeof(TEnum)).Length];
        currentState = states[Convert.ToInt32(currentStateEnum)];
    }

    public void AddState(MoveState<TEnum> _state)
    {
        states[Convert.ToInt32(_state.stateEnum)] = _state;
    }

    public void ChangeState(TEnum newStateEnum)
    {
        if (EqualityComparer<TEnum>.Default.Equals(currentStateEnum, newStateEnum)) return;

        // Exit the current state
        currentState.Exit();

        // Update state enums
        backStateEnum = currentStateEnum;
        currentStateEnum = newStateEnum;

        // Enter the new state
        currentState = states[Convert.ToInt32(currentStateEnum)];
        currentState.Enter();
    }

    public void ProcessTransition()
    {
        foreach (var transition in transitions)
        {
            if (transition.Condition())
            {
                ChangeState(transition.To);
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