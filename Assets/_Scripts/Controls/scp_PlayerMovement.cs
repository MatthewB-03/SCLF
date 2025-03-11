using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Main player control script
/// </summary>
public class scp_PlayerMovement : MonoBehaviour
{
    [Header("Lizard Mode Objects")]
    public GameObject dronePrefab;
    public GameObject grappleVisual;
    public GameObject playerGun;
    public GameObject playerGunCollision;

    // Lizard mode sprites
    private SpriteRenderer playerGunSprite;
    private SpriteRenderer playerGrappleSprite;

    // Grappling
    [HideInInspector] public GameObject myGrappleSegment;
    private Rigidbody2D grapplSegmentRigidbody;
    private Transform grapplSegmentTransform;

    // Animation
    private SpriteRenderer mySprite;
    private Animator myAnimator;


    [Header("DroneDeployCheck")]
    [SerializeField] LayerMask droneLayerMask;

    [Header("Movement")]
    public float runningForce;

    private Rigidbody2D myRigidbody;

    Vector2 direction;
    int playerDirection = 1;

    // Lizard mode
    [HideInInspector] public playerMode playerState;
    public enum playerMode
    {
        gunMode,
        droneMode,
        grappleMode,
        grappling
    }

    // Start is called before the first frame update
    void Start()
    {
        myRigidbody = GetComponent<Rigidbody2D>();
        mySprite = GetComponent<SpriteRenderer>();
        myAnimator = GetComponent<Animator>();

        playerGunSprite = playerGun.GetComponent<SpriteRenderer>();
        playerGrappleSprite = grappleVisual.GetComponent<SpriteRenderer>();
        grappleVisual.SetActive(false);

        scp_SpriteStacker.Instance.AddSprite(mySprite, 2, 2);

        StartCoroutine(InitialGunSetup());
    }

    // Makes sure the gun starts on the correct sorting order
    IEnumerator InitialGunSetup()
    {
        yield return new WaitForEndOfFrame();
        direction = new Vector2(0, -1);
        SetGunSortingOrder();
    }

    // Update is called once per frame
    void Update()
    {
        if ((playerState == playerMode.gunMode) | (playerState == playerMode.grappleMode))
        {
            // Move in the input direction
            direction = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")).normalized;
            myRigidbody.AddForce(runningForce * direction * Time.deltaTime * 0.02f/Time.fixedDeltaTime);

            ModeInputs();
        }
    }

    // LateUpdate is called after update
    private void LateUpdate()
    {
        if ((playerState == playerMode.gunMode) | (playerState == playerMode.grappleMode))
        {
            AnimatePlayerMovement();
            SetGunSortingOrder();
        }
    }

    // Animates the player based on their movement
    void AnimatePlayerMovement()
    {
        // Perform relevant animations to movement 
        if (direction.magnitude > 0)
        {
            myAnimator.SetBool("walking", true);

            if (direction.x < 0)
            {
                mySprite.flipX = true;
                myAnimator.SetFloat("movex", -direction.x);
            }
            else
            {
                mySprite.flipX = false;
                myAnimator.SetFloat("movex", direction.x);
            }

            myAnimator.SetFloat("movey", direction.y);
        }
        else
        {
            myAnimator.SetBool("walking", false);
        }
    }

    // Makes sure the gun is behind the player if facing backwards
    void SetGunSortingOrder()
    {
        if (direction.magnitude > 0)
        {
            // Face backwards if moving up the screen
            playerDirection = 1;
            if (direction.y > 0)
            {
                playerDirection *= -1;
            }

            playerGunSprite.sortingOrder = mySprite.sortingOrder + 2 * playerDirection;
            playerGrappleSprite.sortingOrder = mySprite.sortingOrder + 2 * playerDirection;
        }
    }

    // Enters lizard modes if the appropriate input is made
    void ModeInputs()
    {
        if (Input.GetKeyDown(KeyCode.L))
        {
            EnterDroneMode();
        }
        else if (Input.GetKeyDown(KeyCode.G))
        {
            playerState = playerMode.grappleMode;
        }
        else if (Input.GetKeyUp(KeyCode.G))
        {
            playerState = playerMode.gunMode;
        }

        grappleVisual.SetActive(playerState == playerMode.grappleMode);
        playerGun.SetActive(playerState == playerMode.gunMode);
        playerGunCollision.SetActive(playerState == playerMode.gunMode);
    }

   // FixedUpdate is called when physics updates
    void FixedUpdate()
    {
        if (playerState == playerMode.grappling & myGrappleSegment != null)
        {
            transform.position = new Vector3(grapplSegmentTransform.position.x, grapplSegmentTransform.position.y, transform.position.z);

            myRigidbody.velocity = grapplSegmentRigidbody.velocity;
        }
    }

    // Enters drone mode after a collision check
    void EnterDroneMode()
    {
        RaycastHit2D hit;
        if (mySprite.flipX)
        {
            hit = Physics2D.Raycast(transform.position - new Vector3(0, 0.5f, 0), new Vector3(-1, 0, 0), 3, droneLayerMask);
        }
        else
        {
            hit = Physics2D.Raycast(transform.position - new Vector3(0, 0.5f, 0), new Vector3(1, 0, 0), 3, droneLayerMask);
        }

        if (hit.collider == null)
        {
            myRigidbody.constraints = RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezePositionY | RigidbodyConstraints2D.FreezeRotation;
            grappleVisual.SetActive(false);
            playerState = playerMode.droneMode;
            myAnimator.SetBool("DroneMode", true);
            GameObject drone = Instantiate(dronePrefab, transform.position, Quaternion.identity);
        }
    }

    /// <summary>
    /// Called when the player starts grappling
    /// </summary>
    public void StartGrappling(GameObject grappleSegment)
    {     
        myGrappleSegment = grappleSegment;
        grapplSegmentRigidbody = myGrappleSegment.GetComponent<Rigidbody2D>();
        grapplSegmentTransform = myGrappleSegment.GetComponent<Transform>();

        grappleVisual.GetComponent<Animator>().SetTrigger("Grapple");
    }
    
    /// <summary>
    /// Called when the player reaches the grapple point
    /// </summary>
    public void StopGrappling()
    {
        Destroy(myGrappleSegment);
        playerState = playerMode.gunMode;
        grappleVisual.SetActive(false);
        playerGun.SetActive(true);
        playerGunCollision.SetActive(true);
    }
}
