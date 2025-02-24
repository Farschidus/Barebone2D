using System;

namespace Barebone.Contracts.StateMachine;

public abstract class State<T> where T : Enum
{
    public T StateKey { get; private set; }

    public State(T key)
    {
        StateKey = key;
    }

    public abstract void EnterState();
    public abstract void ExitState();
    public abstract void UpdateState();
    public abstract T GetNextState();
    public abstract void OnTriggerEnter();
    public abstract void OnTriggerStay();
    public abstract void OnTriggerExit();
}