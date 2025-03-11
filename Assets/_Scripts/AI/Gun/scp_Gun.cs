using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Shoots at the player when pursuing them
/// </summary>
public class scp_Gun : MonoBehaviour
{
    [Header("AI")]
    public GameObject myAI;
    private scp_AIScript myAIScript;
    private scp_AIStateMachine myAIStateMachine;
    private Rigidbody2D myAiRigidbody;
    private SpriteRenderer myAISprite;

    // Components
    private Rigidbody2D myRigidBody;
    private SpriteRenderer mySprite;
    private AudioSource myAudioSource;
    private ParticleSystem myParticleSystem;

    [Header("Shooting")]
    public GameObject projectilePrefab;
    public GameObject endOfBarrel;

    public float shootForce;

    public float shootAfterSecondsMin;
    public float shootAfterSecondsMax;

    private Vector2 direction;
    private bool released = false;

    [Header("Position Offset")]
    public Vector2 offset = new Vector2(0, 0);

    // Start is called before the first frame update
    void Start()
    {
        myAIScript = myAI.GetComponent<scp_AIScript>();
        myAiRigidbody = myAI.GetComponent<Rigidbody2D>();
        myAISprite = myAI.GetComponent<SpriteRenderer>();
        myAIStateMachine = myAI.GetComponent<scp_AIStateMachine>();

        myRigidBody = GetComponent<Rigidbody2D>();
        mySprite = GetComponent<SpriteRenderer>();
        myAudioSource = GetComponent<AudioSource>();

        myParticleSystem = endOfBarrel.GetComponent<ParticleSystem>();

        Random.seed = scp_TrueRandom.Instance.GetRandomSeed();

        StartCoroutine(ShootTimer());
    }

    // LateUpdate is called after update
    private void LateUpdate()
    {
        // Rely on AI's sorting order if still held and moving
        if (!released && myAIScript.movementScript.previousMovement.magnitude > Time.deltaTime)
        {
            // Move behind AI if it is facing away
            if (myAIScript.movementScript.previousMovement.y > 0)
            {
                mySprite.sortingOrder = myAISprite.sortingOrder - 1;
            }
            else
            {
                mySprite.sortingOrder = myAISprite.sortingOrder + 1;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        // If not released, stay with AI
        if (!released)
        {
            Vector3 gunOffset = offset;
            // Set position to AI position
            transform.position = myAI.transform.position + gunOffset;
            myRigidBody.velocity = new Vector2(0, 0);

            // Release if AI is dead
            if (myAIStateMachine.GetCurrentState() == myAIStateMachine.dead)
            {
                myRigidBody.velocity = myAiRigidbody.velocity;
                released = true;
                FindObjectOfType<scp_SpriteStacker>().AddSprite(mySprite, 0, 0);
            }
            else
            {
                SetDirection();
            }
        }
    }

    // Sets the gun's rotation depending on if it's pursuing the player
    void SetDirection()
    {
        if (myAIStateMachine.GetCurrentState() == myAIStateMachine.pursuingTarget)
        {
            PointAtTarget();

        }
        else
        {
            PointWithMovement();
        }
    }

    // Points at the current destination
    void PointAtTarget()
    {
        direction = myAIScript.movementScript.GetDestination() - transform.position;

        float angle = Mathf.Atan2(direction.y, direction.x) * 180 / Mathf.PI;

        // Slerp to smooth out rotation over time
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(0, 0, angle), 2.5f*Time.deltaTime);

        if ((myRigidBody.rotation > -90) & (myRigidBody.rotation < 90))
        {
            mySprite.flipY = false;
        }
        else
        {
            mySprite.flipY = true;
        }
    }

    // Points in the AI's direction
    void PointWithMovement()
    {
        if (myAIScript.movementScript.previousMovement.x > 0)
        {
            myRigidBody.rotation = 0;
            mySprite.flipY = false;
        }
        else
        {
            myRigidBody.rotation = 180;
            mySprite.flipY = true;
        }
    }

    // Shoots after a random delay
    IEnumerator ShootTimer()
    {
        yield return new WaitForSeconds(Random.Range(shootAfterSecondsMin, shootAfterSecondsMax));

        if (myAIStateMachine.GetCurrentState() == myAIStateMachine.pursuingTarget)
        {
            Shoot();
        }
        StartCoroutine(ShootTimer());
    }

    // Shoots a projectile
    void Shoot()
    {
        // Shoot a projectile
        GameObject projectile = scp_ObjectPooler.Instance.GetObjectFromPool(projectilePrefab.name);
        projectile.transform.position = endOfBarrel.transform.position;
        projectile.GetComponent<Rigidbody2D>().AddForce(shootForce * direction.normalized);
        projectile.GetComponent<Rigidbody2D>().rotation = myRigidBody.rotation;

        // Shooting effects
        myAudioSource.Play();
        myParticleSystem.Play();
    }
}
