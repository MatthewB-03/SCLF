using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Generic state machine implementing Start and Update
/// </summary>
/// <typeparam name="state">A script inheriting from scp_State</typeparam>
public abstract class scp_StateMachine<state> : MonoBehaviour where state : scp_State
{
    [SerializeField] protected state initialState;
    protected state currentState;

    private void Start()
    {
        EnterState(initialState);
        initialState.Start();
    }

    // Enter the specified state
    public virtual void EnterState(state nextState)
    {
        currentState = nextState;
        currentState.OnStateEntered();
    }

    // Update the current state
    private void Update()
    {
        currentState.Update();
    }
}
