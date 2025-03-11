using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// State machine that controls an AI script
/// </summary>
public class scp_AIStateMachine : scp_StateMachine<scp_AIStateGeneric>
{
    [HideInInspector] public scp_AIStateGeneric idleState = new scp_AIStateIdle();
    [HideInInspector] public scp_AIStateGeneric pursuingTarget = new scp_AIStatePursuing();
    [HideInInspector] public scp_AIStateGeneric lostTarget = new scp_AIStateLostTarget();
    [HideInInspector] public scp_AIStateGeneric pushedBack = new scp_AIStatePushed();
    [HideInInspector] public scp_AIStateGeneric dead = new scp_AIStateDead();

    public scp_AIScript aiScript;

    private void Start()
    {
        aiScript = GetComponent<scp_AIScript>();
        idleState.stateMachine = this;
        pursuingTarget.stateMachine = this;
        lostTarget.stateMachine = this;
        pushedBack.stateMachine = this;
        dead.stateMachine = this;

        idleState.Start();
        pursuingTarget.Start();
        lostTarget.Start();
        pushedBack.Start();
        dead.Start();

        EnterState(idleState);
    }
    public override void EnterState(scp_AIStateGeneric nextState)
    {
        // Don't change state if dead
        if (currentState == dead)
            return;

        base.EnterState(nextState);
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        currentState.OnCollisionEnter2D(collision);
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        currentState.OnTriggerStay2D(collision);
    }

    public scp_AIStateGeneric GetCurrentState()
    {
        return currentState;
    }
}
