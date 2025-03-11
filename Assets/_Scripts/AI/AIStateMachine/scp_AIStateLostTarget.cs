using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Makes the AI look in each direction to try and see the player
/// </summary>
public class scp_AIStateLostTarget : scp_AIStateGeneric
{
    Vector3[] directions = new Vector3[8];
    int directionIndex;

    float timeSpentLooking;
    float lookTime;

    scp_AISound audioScript;

    // Start is called before the first frame update
    public override void Start()
    {
        // Define each direction
        directions[0] = new Vector3(1, 0, 0);
        directions[1] = new Vector3(1, 1, 0);
        directions[2] = new Vector3(0, 1, 0);
        directions[3] = new Vector3(-1, 1, 0);
        directions[4] = new Vector3(-1, 0, 0);
        directions[5] = new Vector3(-1, -1, 0);
        directions[6] = new Vector3(0, -1, 0);
        directions[7] = new Vector3(1, -1, 0);
    }
    public override void OnStateEntered()
    {
        directionIndex = 0;
        lookTime = 0;
        timeSpentLooking = 0;

        audioScript = stateMachine.aiScript.audioScript;
        stateMachine.StartCoroutine(audioScript.mySentences.SaySentence(audioScript.mySentences.lostPlayer));
    }
    public override void Update()
    {
        base.Update();

        timeSpentLooking += Time.deltaTime;
        if (timeSpentLooking >= lookTime)
        {
            // If looked in all directions, go back to idle, else look in the next direction
            if (directionIndex == 7)
            {
                stateMachine.EnterState(stateMachine.idleState);
            }
            else
            {
                stateMachine.aiScript.movementScript.LookInDirection(directions[directionIndex]);
                lookTime = Random.Range(0.25f, 0.75f);
                timeSpentLooking = 0;
                directionIndex += 1;
            }
        }
    }
}
