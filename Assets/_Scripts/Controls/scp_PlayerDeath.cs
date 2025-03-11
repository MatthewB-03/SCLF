using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Starts the death sequence when the player dies
/// </summary>
public class scp_PlayerDeath : MonoBehaviour
{
    [Header("References")]
    private scp_HealthTracker playerHealth;
    private scp_PlayerMovement playerScript;
    public BoxCollider2D playerCollider;

    public GameObject deadLizardPrefab;

    [Header("On player death")]
    public GameObject[] activateOnDeath;
    public GameObject[] deActivateOnDeath;

    private bool dead = false;
    public float waitBeforeReload;


    // Start is called before the first frame update
    void Start()
    {
        playerHealth = GetComponent<scp_HealthTracker>();
        playerScript = GetComponent<scp_PlayerMovement>();
    }

    // Update is called once per frame
    void Update()
    {
        if (playerHealth.myHealth <= 0 & !dead)
        {
            // Play death animation
            GetComponent<Animator>().SetTrigger("Die");

            dead = true;

            ActivateOnDeath();

            CreateDeadLizard();

            playerScript.enabled = false;

            StartCoroutine(WaitThenReloadScene());
        }
    }

    // Activates and deactivates specified objects
    void ActivateOnDeath()
    {
        for (int i = 0; i < activateOnDeath.Length; i++)
        {
            activateOnDeath[i].SetActive(true);
        }

        for (int i = 0; i < deActivateOnDeath.Length; i++)
        {
            deActivateOnDeath[i].SetActive(false);
        }
    }

    // Instanciates the dead lizard prefab
    void CreateDeadLizard()
    {
        // If the lizard is currently with the player, spawn a dead lizard
        if (playerScript.playerState != scp_PlayerMovement.playerMode.droneMode)
        {
            GameObject lizard = Instantiate(deadLizardPrefab, GetComponent<Transform>().position, Quaternion.identity);
            lizard.GetComponent<Rigidbody2D>().velocity = GetComponent<Rigidbody2D>().velocity;
            scp_SpriteStacker.Instance.AddSprite(lizard.GetComponent<SpriteRenderer>(), 0, 0);
        }
        playerScript.enabled = false;
    }

    // Reloads the scene after a delay
    IEnumerator WaitThenReloadScene()
    {
        yield return new WaitForSeconds(waitBeforeReload); 
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
