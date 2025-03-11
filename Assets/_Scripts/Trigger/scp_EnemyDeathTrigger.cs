using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Activates and deactivates objects when a set number of specified enemies have died
/// </summary>
public class scp_EnemyDeathTrigger : MonoBehaviour
{
    // Activate and deactivate
    public GameObject[] gameObjectsToActivate;
    public GameObject[] gameObjectsToDeactivate;

    // Enemies
    public GameObject[] enemiesThatMustDie;
    private scp_AIStateMachine[] enemyStateMachines;

    public int minimumDead = -1;

    private int deathCount;

    // Start is called before the first frame update
    void Start()
    {
        GetComponent<SpriteRenderer>().enabled = false;

        // If no number was specified, assume all enemies must die
        if (minimumDead == -1)
        {
            minimumDead = enemiesThatMustDie.Length;
        }

        // Get enemy scripts
        enemyStateMachines = new scp_AIStateMachine[enemiesThatMustDie.Length];

        for (int i = 0; i < enemiesThatMustDie.Length; i++)
        {
            enemyStateMachines[i] = enemiesThatMustDie[i].GetComponent<scp_AIStateMachine>();
        }
    }

    // Update is called once per frame
    void Update()
    {
        // Count the number of dead enemies
        deathCount = 0;
        for (int i = 0; i < enemyStateMachines.Length; i++)
        {
            if (enemyStateMachines[i].GetCurrentState() == enemyStateMachines[i].dead)
            {
                deathCount += 1;
            }
        }

        if (deathCount >= minimumDead)
        {
            // Activate and deactivate specified objects
            for (int i = 0; i < gameObjectsToActivate.Length; i++)
            {
                gameObjectsToActivate[i].SetActive(true);
            }

            for (int i = 0; i < gameObjectsToDeactivate.Length; i++)
            {
                gameObjectsToDeactivate[i].SetActive(false);
            }

            // Destroy this game object because it's no longer needed 
            Destroy(gameObject);
        }
    }
}
