using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

/// <summary>
/// Generic AI behaviour script with references to the other specialised scripts
/// </summary>
public class scp_AIScript : MonoBehaviour
{
    // Specialised Scripts
    [HideInInspector] public scp_AIVision visionScript;
    [HideInInspector] public scp_AIAnimation animationScript;
    [HideInInspector] public scp_AIMovement movementScript;
    [HideInInspector] public scp_AISound audioScript;

    [HideInInspector] public scp_AIStateMachine stateMachine;
    [HideInInspector] public scp_HealthTracker healthTracker;

    [Header("Behaviour")]
    public GameObject[] idlePatrolNodes;
    public GameObject[] mySquad;
    public scp_AIScript[] mySquadScripts;

    public bool pursueDrone;
    [HideInInspector] public bool iCanSeeThePlayer = false;
    public bool disableColliderOnDeath = false;

    [HideInInspector] public GameObject patrolNode;

    [Header("Attack")]
    public float touchDamage;
    public float touchKnockback;

    // Start is called before the first frame update
    void Start()
    {
        // Initialise variables
        visionScript = GetComponent<scp_AIVision>();
        animationScript = GetComponent<scp_AIAnimation>();
        movementScript = GetComponent<scp_AIMovement>();
        audioScript = GetComponent<scp_AISound>();

        healthTracker = GetComponent<scp_HealthTracker>();
        stateMachine = GetComponent<scp_AIStateMachine>();

        Random.seed = scp_TrueRandom.Instance.GetRandomSeed();

        mySquadScripts = new scp_AIScript[mySquad.Length];
        for(int i=0; i < mySquad.Length; i++)
        {
            mySquadScripts[i] = mySquad[i].GetComponent<scp_AIScript>();
        }

    }
    public void TellSquadmatesToPursue(GameObject seenObject)
    {
        for (int i = 0; i < mySquad.Length; i++)
        {
            mySquadScripts[i].PursueSeenObject(seenObject);
        }
    }

    public void PursueSeenObject(GameObject seenObject)
    {
        if (stateMachine.GetCurrentState() == stateMachine.dead)
            return;

        stateMachine.EnterState(stateMachine.pursuingTarget);

        //I now know that the player has the lizard
        pursueDrone = true;
        iCanSeeThePlayer = true;

        //Go to the player's last known location, because I cannot see through walls if I lose my line of sight.
        movementScript.SetNewDestination(seenObject.transform.position);
        patrolNode = null;
    }

    // Sets appropriate variables on death
    public void Die()
    {
        patrolNode = null;
        movementScript.DisableMovement();
        animationScript.DeathAnimation();
        Destroy(visionScript.visionCollider.gameObject);
        StartCoroutine(audioScript.DeathAudio());

        if (disableColliderOnDeath)
        {
            StartCoroutine(DisableColliderAfterDelay());
        }

    }
    public IEnumerator DisableColliderAfterDelay()
    {
        yield return new WaitForSeconds(1);
        GetComponent<Rigidbody2D>().simulated = false;
        GetComponent<BoxCollider2D>().enabled = false;
    }
    public void TouchingPlayer(GameObject player)
    {
        if (player.GetComponent<scp_HealthTracker>().iAmVulnerable == true)
        {
            audioScript.audioSource.Play();
            player.GetComponent<scp_HealthTracker>().Hurt(touchDamage, gameObject, touchKnockback);
        }
    }

    // Sets a new random patrol node.
    public void SetIdlePatrolNode()
    {
        // Do not go to the same node as another squad mate
        List<GameObject> freePatrolNodes = new List<GameObject>();
        foreach (GameObject node in idlePatrolNodes)
        {
            freePatrolNodes.Add(node);
            bool found = false;
            foreach (scp_AIScript squadMate in mySquadScripts)
            {
                if (!found)
                {
                    if (node == squadMate.patrolNode && squadMate != this)
                    {
                        found = true;
                        freePatrolNodes.Remove(node);
                    }
                }
            }
        }

        // Move to a random node, unless none are available
        if (freePatrolNodes.Count > 0)
        {
            patrolNode = freePatrolNodes[Random.Range(0, freePatrolNodes.Count)];
            movementScript.SetNewDestination(patrolNode.transform.position);
        }
        else
        {
            // Make my current node free and try again
            patrolNode = null;
            WaitAndSetPatrolNode(0.5f);
        }
    }

    // Waits then picks a patrol node
    public IEnumerator WaitAndSetPatrolNode(float time)
    {
        yield return new WaitForSeconds(time);

        //If I have no idea where the player is, go to a random patrol node.
        if (!iCanSeeThePlayer)
        {
            SetIdlePatrolNode();
        }
    }
}