using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Stores and updates something's health
/// </summary>
public class scp_HealthTracker : MonoBehaviour
{

    public bool iAmVulnerable;
    public float myHealth;
    public float waitUntilVulnerable;

    private Rigidbody2D myRigidbody;
    private Transform myTransform;

    public bool onlyTakeBombDamage = false;

    // Start is called before the first frame update
    void Start()
    {
        myRigidbody = GetComponent<Rigidbody2D>();
        myTransform = GetComponent<Transform>();
        iAmVulnerable = true;
    }

    // Waits a delay, then becomes vulnerable
    IEnumerator WaitThenBecomeVulnerable()
    {
        yield return new WaitForSeconds(waitUntilVulnerable);
        iAmVulnerable = true;
    }

    /// <summary>
    /// Deal damage and add a knockback force
    /// </summary>
    /// <param name="damage">The amount of damage dealt</param>
    /// <param name="damageDealer">The gameObject dealing the damage</param>
    /// <param name="knockbackForce">The knockback force</param>
    public void Hurt(float damage, GameObject damageDealer, float knockbackForce)
    {
        //Only take damage if currently vunerable
        if (iAmVulnerable & !onlyTakeBombDamage | onlyTakeBombDamage & damageDealer.tag == "Bomb")
        {
            if (myHealth > 0)
            {
                //Reduce health, get knocked back in the same direction as the damage.
                myHealth -= damage;
                iAmVulnerable = false;

                Transform dealerTransform = damageDealer.GetComponent<Transform>();

                Vector2 knockback = -knockbackForce * new Vector2(dealerTransform.position.x - myTransform.position.x, dealerTransform.position.y - myTransform.position.y);
                myRigidbody.AddForce(knockback);

                StartCoroutine(WaitThenBecomeVulnerable());

            }
        }
    }
}
