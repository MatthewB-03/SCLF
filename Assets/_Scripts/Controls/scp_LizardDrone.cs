using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Deploys a controlable drone that must be near the player for retrieval
/// </summary>
public class scp_LizardDrone : MonoBehaviour
{
    // Cameras
    private GameObject myCamera;
    private GameObject mainCamera;

    // Player
    private GameObject player;
    private GameObject playerGun;
    private GameObject playerGunCollision;

    // Bools
    private bool iAmDoneDeploying = false;
    private bool iAmTouchingPlayer = true;

    // Movement
    public float runningForce;
    private Rigidbody2D myRigidbody;

    // Animation
    private Animator myAnimator;
    private SpriteRenderer mySprite;
    public float deployWait;

    // Start is called before the first frame update
    void Start()
    {
        // Activate drone camera
        mainCamera = GameObject.Find("Main Camera");
        myCamera = GameObject.Find("Drone Camera");
        mainCamera.SetActive(false);
        myCamera.SetActive(true);

        // Get component references
        myRigidbody = transform.parent.GetComponent<Rigidbody2D>();
        myAnimator = GetComponent<Animator>();
        mySprite = GetComponent<SpriteRenderer>();


        // Get player references
        player = GameObject.Find("Player");
        playerGun = player.GetComponent<scp_PlayerMovement>().playerGun;
        playerGunCollision = player.GetComponent<scp_PlayerMovement>().playerGunCollision;

        playerGun.SetActive(false);
        playerGunCollision.SetActive(false);

        StartCoroutine(WaitUntilDeployed());

    }

    // Waits for the deploy animation to finish
    IEnumerator WaitUntilDeployed()
    {
        // Freeze drone for animation
        Vector2 pos = new Vector2(1.3f, -0.45f);
        myRigidbody.constraints = RigidbodyConstraints2D.FreezeAll;
        mySprite.sortingOrder = player.GetComponent<SpriteRenderer>().sortingOrder + 2;

        // Deploy in the correct direction
        mySprite.flipX = player.GetComponent<SpriteRenderer>().flipX;
        GetComponent<Animator>().SetBool("flip", mySprite.flipX);
        if (mySprite.flipX)
        {
            pos.x *= -1;
        }

        // Wait for animation
        yield return new WaitForSeconds(deployWait);

        // Make drone playable
        transform.localPosition = pos;
        myRigidbody.constraints = RigidbodyConstraints2D.FreezeRotation;
        scp_SpriteStacker.Instance.AddSprite(mySprite, 0, 0);

        iAmDoneDeploying = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (iAmDoneDeploying)
        {
            // Move the drone in the input direction
            Vector2 direction = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")).normalized;
            myRigidbody.AddForce(runningForce * direction * Time.deltaTime);

            // Animation
            AnimateDrone(direction);

            // Only exit drone mode if touching the player
            if ((Input.GetKey(KeyCode.L) & iAmTouchingPlayer))
            {
                ExitDroneMode();
            }
        }
        else
        {
            mySprite.sortingOrder = player.GetComponent<SpriteRenderer>().sortingOrder + 2;
        }
    }
   
    // Plays the running animation if moving
    private void AnimateDrone(Vector2 direction)
    {
        if ((direction.x != 0) | (direction.y != 0))
        {
            myAnimator.SetBool("running", true);

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
            myAnimator.SetBool("running", false);
        }
    }

    // Exits drone mode
    private void ExitDroneMode()
    {
        // Play player retrieve animation
        player.GetComponent<scp_PlayerMovement>().playerState = scp_PlayerMovement.playerMode.gunMode;
        player.GetComponent<Rigidbody2D>().constraints =  RigidbodyConstraints2D.FreezeRotation;
        player.GetComponent<Animator>().SetBool("DroneMode", false);

        // Deactivate and destroy the drone, activate the player
        myCamera.SetActive(false);
        mainCamera.SetActive(true);
        playerGun.SetActive(true);
        playerGunCollision.SetActive(true);

        Destroy(transform.parent.gameObject);
    }

    // Called when something enters the trigger collider
    private void OnTriggerEnter2D(Collider2D collision)
    {
        GameObject colliding = collision.gameObject;
        if (colliding.tag == "Player")
        {
            iAmTouchingPlayer = true;
        }

    }

    // Called when something exits the trigger collider
    private void OnTriggerExit2D(Collider2D collision)
    {
        GameObject colliding = collision.gameObject;
        if (colliding.tag == "Player")
        {
            iAmTouchingPlayer = false;
        }
    }
}