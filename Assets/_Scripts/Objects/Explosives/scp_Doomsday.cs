using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Plays explosion effects and starts the victory sequence on death
/// </summary>
public class scp_Doomsday : MonoBehaviour
{
    // Idle animations
    public string[] idleAnimationTriggers;
    public float[] idleAnimationLengths;

    // Death animations
    public string[] deathAnimationTriggers;

    // Animation
    private Animator myAnimator;
    public GameObject screen;

    // Explosions
    public GameObject[] deathExplosions;
    public float waitBetweenExplosions;

    // Health
    private scp_HealthTracker myHealth;
    private bool dead = false;

    // Activate and deactivate
    public GameObject[] activateOnDeath;
    public GameObject[] deActivateOnDeath;

    // Start is called before the first frame update
    void Start()
    {
        myAnimator = GetComponent<Animator>();
        myHealth = GetComponent<scp_HealthTracker>();
        StartCoroutine(RandomAnimation());
    }

    // Plays a random idle animation
    IEnumerator RandomAnimation()
    {
        // Play a random idle animation if I haven't died
        if (myHealth.myHealth > 0)
        {
            int index = Random.Range(0, idleAnimationTriggers.Length);
            myAnimator.SetTrigger(idleAnimationTriggers[index]);
            yield return new WaitForSeconds(idleAnimationLengths[index]);
            StartCoroutine(RandomAnimation());
        }
    }

    // Plays a random death animation and plays explosion effects
    IEnumerator Die()
    {
        dead = true;

        // Play a random death animation
        int index = Random.Range(0, deathAnimationTriggers.Length);
        myAnimator.SetTrigger(deathAnimationTriggers[index]);

        // Play explosion effects
        for (int i=0; i < deathExplosions.Length; i++)
        {
            yield return new WaitForSeconds(waitBetweenExplosions);
            deathExplosions[i].GetComponent<ParticleSystem>().Play();
            deathExplosions[i].GetComponent<AudioSource>().Play();
        }

        // Destroy the animated screen
        Destroy(screen);

        // Activate and deactivate specified objects
        for (int i = 0; i < activateOnDeath.Length; i++)
        {
            activateOnDeath[i].SetActive(true);
        }

        for (int i = 0; i < deActivateOnDeath.Length; i++)
        {
            deActivateOnDeath[i].SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (myHealth.myHealth <= 0 & !dead)
        {
            StartCoroutine(Die());
        }
    }
}
