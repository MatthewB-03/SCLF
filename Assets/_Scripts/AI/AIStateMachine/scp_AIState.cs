using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// State incorporating OnTriggerStay2D and OnCollisionEnter2D for use in AI
/// </summary>
public abstract class scp_AIState : scp_State
{
    public scp_AIStateMachine stateMachine;
    public override void Update()
    {

    }
    public abstract void OnTriggerStay2D(Collider2D collision);
    public abstract void OnCollisionEnter2D(Collision2D collision);
}
