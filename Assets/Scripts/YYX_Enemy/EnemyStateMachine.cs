using UnityEngine;
using System.Collections.Generic;
using System;



public enum EnemyState
{
    Idle,
    Move,
    Attack,
    Dead,
}

public interface IEnemyState
{
    EnemyState state { get; }
    string stateName { get; }
    public virtual void OnEnter()
    {
        
    }
    public virtual void OnExit()
    {

    }
    EnemyState OnUpdate();
}

public class EnemyStateMachine<T> where T : IEnemyState
{
    T currentState;
    public T CurrentState { get { return currentState; } }
    private Dictionary<EnemyState, T> states = new Dictionary<EnemyState, T>();
    public EnemyStateMachine()
    {
    }
    public void Initialize(T initialState)
    {   
        if (initialState == null)
        {
            initialState = states[EnemyState.Idle]; // Set to default state if not provided
            Debug.LogWarning("Initial state is null, setting to Idle state.");
        }
        currentState = initialState;
        currentState.OnEnter();
    }

    public void ChangeState(T newState)
    {
        currentState.OnExit();
        currentState = newState;
        currentState.OnEnter();
    }

    public void Update()
    {
        Debug.Log("Current State: " + currentState.stateName);
        EnemyState newState = currentState.OnUpdate();
        if (newState != currentState.state)
        {
            ChangeState(states[newState]);
        }
    }

    public void AddState(T state)
    {
        if (states.ContainsKey(state.state))
        {
            Debug.Log("State already exists: " + state.stateName);
            return;
        }
        states.Add(state.state, state);
    }

    public IEnemyState GetState(EnemyState state)
    {
        if (states.ContainsKey(state))
        {
            return states[state];
        }
        else
        {
            Debug.Log("State not found: " + state);
            return null;
        }
    }

}