using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Idle patroling state
/// </summary>
public class scp_AIStateIdle : scp_AIStateGeneric
{
    public override void OnStateEntered()
    {
        base.OnStateEntered();
        stateMachine.aiScript.iCanSeeThePlayer = false;
        stateMachine.StartCoroutine(stateMachine.aiScript.WaitAndSetPatrolNode(0.1f));
    }
    public override void Update()
    {
        base.Update();
        if (stateMachine.aiScript.movementScript.AtDestination() && stateMachine.aiScript.movementScript.previousMovement.magnitude > Time.deltaTime)
        {
            stateMachine.StartCoroutine(stateMachine.aiScript.WaitAndSetPatrolNode(Random.Range(1, 4)));
        }
    }
}
