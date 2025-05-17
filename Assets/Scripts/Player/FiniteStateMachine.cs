using UnityEngine;
using System.Collections.Generic;
using System;

class FiniteStateMachine<T> where T : IState {
    T currentState;
    public T CurrentState { get { return currentState; } } 
    private Dictionary<State , T> states = new Dictionary<State,T>();
    public FiniteStateMachine() {
    }
    public void Initialize(T initialState = null) {
        if (initialState == null) {
            initialState = states[State.Idle]; // Set to default state if not provided
        }
        currentState = initialState;
        currentState.OnEnter();
    }

    public void ChangeState(T newState) {
        currentState.OnExit();
        currentState = newState;
        currentState.OnEnter(); 
    }

    public void Update() {
        Debug.Log("Current State: " + currentState.stateName);
        State newState = currentState.OnUpdate();
        if (newState != currentState.state) {
            ChangeState(states[newState]); 
        } 
    }

    public void AddState(T state) {
        if (states.ContainsKey(state.state)) {
            Debug.Log("State already exists: " + state.stateName);
            return; 
        }
        states.Add(state.state, state); 
    }

}