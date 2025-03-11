using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Plays an explosion effect when the AI dies
/// </summary>
public class scp_ExplodeOnDeath : MonoBehaviour
{
    public scp_HealthTracker myHealth;
    public GameObject explosion;
    private bool exploded = false;


    // FixdedUpdate is called every physics update
    void FixedUpdate()
    {
        if (myHealth.myHealth <= 0 & !exploded)
        {
            explosion.SetActive(true);
            explosion.GetComponent<ParticleSystem>().Play();
            exploded = true;
        }
    }
}
