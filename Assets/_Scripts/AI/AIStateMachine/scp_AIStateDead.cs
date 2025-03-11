using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// State entered when the AI is dead
/// </summary>
public class scp_AIStateDead : scp_AIStateGeneric
{
    public override void OnStateEntered()
    {
        stateMachine.aiScript.Die();
    }

    public override void Update()
    {
    }
    public override void OnCollisionEnter2D(Collision2D collision)
    {
    }
    public override void OnTriggerStay2D(Collider2D collision)
    {
    }
}
