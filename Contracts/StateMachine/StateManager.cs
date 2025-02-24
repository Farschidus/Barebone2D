using System;
using System.Collections.Generic;

namespace Barebone.Contracts.StateMachine;

public abstract class StateManager<T> where T : Enum
{

    public State<T> State { get => _currentState; }
    protected Dictionary<T, State<T>> _states = [];
    protected T _currentStateKey;
    protected State<T> _currentState;

    private bool _isInTransition;


    protected void AddState(T key, State<T> state)
    {
        _states.Add(key, state);
    }
    protected void ChangeState(T state)
    {
        if(!_states.TryGetValue(state, out var newState) || _isInTransition) return;

        _isInTransition = true;

        _currentState?.ExitState();
        _currentState = newState;
        _currentStateKey = newState.StateKey;
        _currentState.EnterState();

        _isInTransition = false;
    }
    protected void RemoveState(T state)
    {
        if(_states.ContainsKey(state))
            _states.Remove(state);
    }
    protected void Update()
    {
        _currentState.UpdateState();
    }
    protected void OnTriggerEnter() {
        _currentState.OnTriggerEnter();
    }
    protected void OnTriggerStay() {
        _currentState.OnTriggerStay();
    }
    protected void OnTriggerExit() {
        _currentState.OnTriggerExit();
    }
    protected T GetNextState(T type) {
        return _currentState.GetNextState();
    }
}