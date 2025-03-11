using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// A bomb that can be launched, and explodes when it hits an enemy
/// </summary>
public class scp_Bomb : MonoBehaviour
{
    public GameObject explosionPrefab;
    private bool iHaveBeenLaunched;
    private Rigidbody2D myRigidbody;
    private Animator myAnimator;

    private bool onLand = true;
    [HideInInspector] public bool pooledBomb = false;

    // Start is called before the first frame update
    void Start()
    {
        // Initialise variables
        myAnimator = GetComponent<Animator>();
        myRigidbody = GetComponent<Rigidbody2D>();
        iHaveBeenLaunched = false;
    }

    private void Update()
    {
        //If i've slowed down enough, I must be launched again.
        if (myRigidbody.velocity.magnitude < 1)
        {
            //Should not float in the air, so explode if this is the case.
            if (onLand)
            {
                myAnimator.SetBool("launched", false);
                iHaveBeenLaunched = false;
                myRigidbody.drag = 10;
            }
            else
            {
                Kaboom();
            }
        }
    }
    // Launches the bomb
    public void Launch()
    {
        myAnimator.SetBool("launched", true);
        iHaveBeenLaunched = true;
        myRigidbody.drag = 0.01f;
    }

    /// <summary>
    /// Starts the explosion coroutine
    /// </summary>
    public void  Kaboom()
    {
        StartCoroutine(WaitThenExplode());
    }

    // Explodes the bomb
    IEnumerator WaitThenExplode()
    {
        //Wait for next fixed update so that all collisions are dealt with.
        yield return new WaitForFixedUpdate();
        Explode();
    }

    // Spawns the explosion prefab, then destroys the bomb unless it is pooled
    void Explode()
    {
        GameObject explosion = scp_ObjectPooler.Instance.GetObjectFromPool(explosionPrefab.name);
        explosion.transform.position = transform.position;
        explosion.GetComponent<ParticleSystem>().Play();
        explosion.GetComponent<AudioSource>().Play();

        if (pooledBomb)
        {
            iHaveBeenLaunched = false;
            onLand = true;
            myRigidbody.drag = 10;
            gameObject.SetActive(false);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void OnTriggerStay2D(Collider2D collision)
    {
        GameObject colliding = collision.gameObject;
        if ((colliding.tag == "Enemy" | colliding.tag == "Doomsday") & iHaveBeenLaunched)
        {
            colliding.GetComponent<scp_HealthTracker>().Hurt(2, gameObject, 1000);
            Kaboom();
        }
    }
    void OnCollisionEnter2D(Collision2D collision)
    {
        GameObject colliding = collision.gameObject;
        if (colliding.tag == "Floor")
        {
            onLand = true;
        }
    }
    void OnCollisionExit2D(Collision2D collision)
    {
        GameObject colliding = collision.gameObject;
        if (colliding.tag == "Floor")
        {
            onLand = false;
        }
    }
}
