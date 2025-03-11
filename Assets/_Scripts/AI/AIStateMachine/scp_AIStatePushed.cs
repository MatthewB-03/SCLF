using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// AI has been pushed back by an attack
/// </summary>
public class scp_AIStatePushed : scp_AIStateGeneric
{
    public override void Update()
    {
        if (stateMachine.aiScript.healthTracker.myHealth > 0)
        {
            // Wait until slowed down to change state
            Vector2 movement = stateMachine.aiScript.movementScript.previousPosition - stateMachine.transform.position;
            if (movement.magnitude < 0.1f)
            {
                stateMachine.EnterState(stateMachine.pursuingTarget);
            }
        }
        else
        {
            stateMachine.EnterState(stateMachine.dead);
        }
    }
}
