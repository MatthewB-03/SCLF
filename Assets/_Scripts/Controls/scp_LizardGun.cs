using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Pointa at the mouse and attacks when clicking
/// </summary>
public class scp_LizardGun : MonoBehaviour
{
    // Player references
    public GameObject player;
    private Rigidbody2D playerRigidBody;
    private Rigidbody2D myRigidBody;
    private SpriteRenderer mySprite;
    private Animator myAnimator;

    [SerializeField] private SpriteRenderer armSprite;
    [SerializeField] private SpriteRenderer playerSprite;

    public float yOffset;

    // Attacks
    public float pushForce;
    public float pushCoolDown;

    public float attackForce;
    public float attackDamage;

    private bool pushedOnThisFrame;
    private bool attackedOnThisFrame;

    // Audio
    private AudioSource myAudio;
    public AudioClip biteSound;
    public AudioClip shoutSound;


    // Start is called before the first frame update
    void Start()
    {
        // Get references
        playerRigidBody = player.GetComponent<Rigidbody2D>();
        myRigidBody = GetComponent<Rigidbody2D>();
        mySprite = GetComponent<SpriteRenderer>();
        myAnimator = GetComponent<Animator>();
        myAudio = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        PointTowardsMouse();
        SetPosition();
        GetAttackInputs();
    }

    // Points towards the mouse
    void PointTowardsMouse()
    {
        // Point towards mouse
        Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 direction = mousePosition - myRigidBody.position;

        myRigidBody.rotation = Mathf.Atan2(direction.y, direction.x) * 180 / Mathf.PI;
    }

    // Sets the gun's position
    void SetPosition()
    {
        Vector2 newPos;
        armSprite.sortingOrder = playerSprite.sortingOrder - 1;
        if (mySprite.sortingOrder == playerSprite.sortingOrder - 2)
        {
            // Make sure the gun isn't below the player when facing backwards
            newPos = OffsetGunYPosition();
        }
        else
        {
            newPos = new Vector2(playerRigidBody.position.x, playerRigidBody.position.y);
        }

        myRigidBody.position = newPos;
        transform.position = newPos;

        // Make sure the sprite points upwards
        if ((myRigidBody.rotation > -90) & (myRigidBody.rotation < 90))
        {
            mySprite.flipY = false;
        }
        else
        {
            mySprite.flipY = true;
        }
    }

    // Adds a Y offset to prevent the sprite from passing under the player
    Vector2 OffsetGunYPosition()
    {
        float angle = myRigidBody.rotation;
        if (angle > 90)
        {
            angle -= 180;
        }
        else if (angle < -90)
        {
            angle += 180;
        }
        float gunYOffset = yOffset * (1 - Mathf.Cos(angle * Mathf.PI / 180));
        return new Vector2(playerRigidBody.position.x, playerRigidBody.position.y + gunYOffset);
    }

    // Starts an attack if an input was made
    void GetAttackInputs()
    {
        if (Input.GetKeyDown(KeyCode.Mouse1))
        {
            myAnimator.SetTrigger("Shout");
            StartCoroutine(WaitThenResetPush());
        }

        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            myAnimator.SetTrigger("Bite");
            StartCoroutine(WaitThenResetAttack());
        }
    }

    // Activates and resets the push attack
    IEnumerator WaitThenResetPush()
    {
        myAudio.clip = shoutSound;
        myAudio.Play();

        yield return new WaitForSeconds(0.1f);
        pushedOnThisFrame = true;

        yield return new WaitForSeconds(0.1f);
        pushedOnThisFrame = false;
    }

    // Activates and resets the bite attack
    IEnumerator WaitThenResetAttack()
    {
        yield return new WaitForSeconds(0.25f);
        attackedOnThisFrame = true;

        myAudio.clip = biteSound;
        myAudio.Play();

        yield return new WaitForSeconds(0.1f);
        attackedOnThisFrame = false;
    }

    void OnTriggerStay2D(Collider2D collision)
    {
        if (!collision.isTrigger)
        {
            GameObject colliding = collision.gameObject;

            // Push attack
            if (pushedOnThisFrame)
            {
                if (colliding.tag == "Enemy")
                {
                    // Push enemy
                    colliding.GetComponent<Rigidbody2D>().AddForce(transform.right * pushForce);
                    scp_AIStateMachine enemyStateMachine = colliding.GetComponent<scp_AIStateMachine>();
                    enemyStateMachine.EnterState(enemyStateMachine.pushedBack);
                }
                if (colliding.tag == "Bomb")
                {
                    // Launch bomb
                    colliding.GetComponent<scp_Bomb>().Launch();
                    colliding.GetComponent<Rigidbody2D>().AddForce(transform.right * pushForce);
                }
            }

            // Bite attack
            if (attackedOnThisFrame)
            {
                if (colliding.tag == "Enemy")
                {
                    // Damage enemy
                    colliding.GetComponent<scp_HealthTracker>().Hurt(attackDamage, gameObject, attackForce);
                    scp_AIStateMachine enemyStateMachine = colliding.GetComponent<scp_AIStateMachine>();
                    enemyStateMachine.EnterState(enemyStateMachine.pushedBack);
                }
            }
        }
    }
    
}
