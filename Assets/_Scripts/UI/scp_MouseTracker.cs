using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Follows the mouse position
/// </summary>
public class scp_MouseTracker : MonoBehaviour
{
    // Mouse
    private Vector2 mousePosition;
    private Transform myTransform;
    private SpriteRenderer mySprite;

    // Player
    public GameObject player;
    private scp_PlayerMovement playerScript;
    private Transform playerTransform;
    private Vector2 playerPos;

    // Camera
    public GameObject camera;
    private Transform cameraTransform;
    private Vector2 cameraPos;

    // Start is called before the first frame update
    void Start()
    {
        // Initialise variables
        myTransform = GetComponent<Transform>();
        mySprite = GetComponent<SpriteRenderer>();

        playerTransform = player.GetComponent<Transform>();
        playerPos = playerTransform.position;
        playerScript = player.GetComponent<scp_PlayerMovement>();


        cameraTransform = camera.GetComponent<Transform>();
        cameraPos = cameraTransform.position;


        myTransform.position = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        Cursor.visible = false;
    }

    // Update is called once per frame
    void Update()
    {
       // Move tracker to mouse position, disable sprite when in drone mode
        if (playerScript.playerState == scp_PlayerMovement.playerMode.droneMode)
        {
            mySprite.enabled = false;
        }
        else
        {
            mySprite.enabled = true;

            //Add player movement because Input.mousePosition tends to lag behind.
            mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            float movex = (playerTransform.position.x - playerPos.x) - (cameraTransform.position.x - cameraPos.x);
            float movey = (playerTransform.position.y - playerPos.y) - (cameraTransform.position.y - cameraPos.y);

            myTransform.position = new Vector2(mousePosition.x + movex, mousePosition.y + movey);

            playerPos = playerTransform.position;
            cameraPos = cameraTransform.position;
        }
    }

}
