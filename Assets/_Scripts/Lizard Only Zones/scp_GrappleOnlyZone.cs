using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

/// <summary>
/// Disables the tilemap collider when the player is grappling
/// </summary>
public class scp_GrappleOnlyZone : MonoBehaviour
{

    public GameObject player;
    private scp_PlayerMovement playerScript;

    private TilemapCollider2D myCollider;
    private BoxCollider2D playerCollider;

    // Start is called before the first frame update
    void Start()
    {
        playerScript = player.GetComponent<scp_PlayerMovement>();
        myCollider = GetComponent<TilemapCollider2D>();
        playerCollider = player.GetComponent<BoxCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        // Disable when the player is in grapple mode
        if (playerScript.playerState == scp_PlayerMovement.playerMode.grappling)
        {
            playerCollider.enabled = false;
        }
        else
        {
            playerCollider.enabled = true;
        }
    }
}
