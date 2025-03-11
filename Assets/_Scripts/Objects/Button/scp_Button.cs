using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// A button that can be pressed by the player, other objects can reference it to add functionality
/// </summary>
public class scp_Button : MonoBehaviour
{
    // Sprites
    public Sprite onSprite;
    public Sprite offSprite;
    private SpriteRenderer mySprite;

    // Sound
    public AudioClip onSound;
    public AudioClip offSound;
    private AudioSource myAudio;

    // Behaviour
    public bool iAmOn;
    public bool popBackOut = false;
    public bool popBackIn = false;
    public bool acceptsBox = true;

    // Player
    private GameObject player;
    private scp_PlayerMovement playerScript;

    private Transform playerTransform;
    private Transform myTransform;

    private bool playerPassedOver = false;

    private bool playerGrappled = false;

    private string pressingMe;


    // Start is called before the first frame update
    void Start()
    {
        // Initialise variables
        mySprite = GetComponent<SpriteRenderer>();
        player = GameObject.Find("Player");
        playerScript = player.GetComponent<scp_PlayerMovement>();
        playerTransform = player.GetComponent<Transform>();
        myTransform = GetComponent<Transform>();
        myAudio = GetComponent<AudioSource>();

        pressingMe = "";
    }

    // Update is called once per frame
    void Update()
    {
        // Keep track of if the player has grappled
        if (playerScript.playerState == scp_PlayerMovement.playerMode.grappling)
        {
            playerGrappled = true;
        }
    }

    void Toggle()
    {
        switch (iAmOn)
        {
            case true:
                iAmOn = false;
                mySprite.sprite = offSprite;
                myAudio.clip = offSound;
                myAudio.Play();
                break;

            case false:
                iAmOn = true;
                mySprite.sprite = onSprite;
                myAudio.clip = onSound;
                myAudio.Play();
                break;
        }
    }
    void SetPressing(GameObject colliding)
    {
        switch (iAmOn)
        {
            case true:
                pressingMe = colliding.tag;
                break;

            case false:
                pressingMe = "";
                break;
        }
    }

    private void FixedUpdate()
    {
        // When the player grapples their collision turns off, so use distance instead
        if (playerScript.playerState == scp_PlayerMovement.playerMode.grappling & !playerPassedOver)
        {
            float deltaX = playerTransform.position.x - myTransform.position.x;
            float deltaY = playerTransform.position.y - myTransform.position.y;

            float distance = Mathf.Sqrt(deltaX * deltaX + deltaY * deltaY);

            if (distance < 1)
            {
                Toggle();
                playerPassedOver = true;
            }
        }
        else if(playerScript.playerState != scp_PlayerMovement.playerMode.grappling)
        {
            playerPassedOver = false;
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        GameObject colliding = collision.gameObject;

        // Make sure the colliding object can press me
        if ((((colliding.tag == "Player") | (colliding.tag == "PlayerDrone") | (colliding.tag == "box" & acceptsBox)) & !(playerGrappled & popBackIn)))
        {
            // If i'm being pressed, make sure the colliding object is the one pressing me
            if (pressingMe == colliding.tag | pressingMe == "")
            {
                // Make sure the collision is not a trigger collider
                if (collision.isTrigger == false)
                {
                    Toggle();
                    SetPressing(colliding);
                }
            }
        }
        else if (playerGrappled & popBackOut & (colliding.tag == "Player"))
        {
            playerGrappled = false;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        GameObject colliding = collision.gameObject;

        // Do not pop back out if the player is grappling
        if (popBackOut & playerScript.playerState != scp_PlayerMovement.playerMode.grappling)
        {
            // If i'm being pressed, make sure the colliding object is the one pressing me
            if (pressingMe == colliding.tag | pressingMe == "")
            {
                // Make sure the colliding object can press me
                if ((colliding.tag == "Player") | (colliding.tag == "PlayerDrone") | (colliding.tag == "box" & acceptsBox) & (!playerGrappled))
                {
                    // Make sure the collision is not a trigger collider
                    if (collision.isTrigger == false)
                    {
                        Toggle();
                        SetPressing(colliding);
                    }
                }
            }

        }
    }
}
