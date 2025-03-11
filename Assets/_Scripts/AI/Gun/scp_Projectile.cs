using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Projectile that damages the player, and becomes inactive when touching them
/// </summary>
public class scp_Projectile : MonoBehaviour
{
    public float damage;
    public float knockback;
    public void OnTriggerEnter2D(Collider2D collision)
    {
        GameObject colliding = collision.gameObject;

        //If I'm touching the player, hurt them.
        if ((colliding.tag == "Player"))
        {
            colliding.GetComponent<scp_HealthTracker>().Hurt(damage, gameObject, knockback);
            gameObject.SetActive(false);
        }
        else if (colliding.tag == "Wall")
        {
            gameObject.SetActive(false);
        }
        // If I'm touching a bomb, set it off
        else if (colliding.tag == "Bomb")
        {
            colliding.GetComponent<scp_Bomb>().Kaboom();
            gameObject.SetActive(false);
        }
    }
}
