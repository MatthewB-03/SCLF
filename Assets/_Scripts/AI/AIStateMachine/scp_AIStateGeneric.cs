using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Generic state for the other AI states to inherit from
/// </summary>
public class scp_AIStateGeneric : scp_AIState
{

    // Start is called before the first frame update
    public override void Start()
    {
        Random.seed = scp_TrueRandom.Instance.GetRandomSeed();
    }
    protected float GetAngle(Vector2 direction)
    {
        return Mathf.Atan2(direction.y, direction.x);
    }

    public override void Update()
    {
        if (stateMachine.aiScript.healthTracker.myHealth > 0)
        {
            stateMachine.aiScript.animationScript.AliveAnimation(stateMachine.aiScript.movementScript.previousMovement, 1);
        }
        else
        {
            stateMachine.EnterState(stateMachine.dead);
        }
    }

    public override void OnTriggerStay2D(Collider2D collision)
    {
        GameObject colliding = collision.gameObject;

        if (((colliding.tag == "Player") | (colliding.tag == "PlayerGun") | (colliding.tag == "PlayerDrone")))
        {
            stateMachine.aiScript.visionScript.CreateRayCone(GetAngle(-(stateMachine.aiScript.transform.position - colliding.transform.position)));
        }
    }
    public override void OnCollisionEnter2D(Collision2D collision)
    {
    }
}
