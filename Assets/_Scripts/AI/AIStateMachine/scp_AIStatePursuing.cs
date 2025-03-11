using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// AI is chasing the player or drone
/// </summary>
public class scp_AIStatePursuing : scp_AIStateGeneric
{
    // Start is called before the first frame update
    public override void Start()
    {
    }

    public override void Update()
    {
        base.Update();
        if (stateMachine.aiScript.movementScript.AtDestination())
        {
            stateMachine.EnterState(stateMachine.lostTarget);
        }
    }

    public override void OnCollisionEnter2D(Collision2D collision)
    {
        GameObject colliding = collision.gameObject;

        //If I'm touching the player, hurt them.
        if (colliding.tag == "Player")
        {
            stateMachine.aiScript.TouchingPlayer(colliding);
        }
        else if (colliding.tag == "PlayerGun")
        {
            stateMachine.aiScript.TouchingPlayer(GameObject.Find("Player"));
        }
    }
}
