using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

/// <summary>
/// Disables the tilemap collider when the player is in drone mode
/// </summary>
public class scp_DroneOnlyZone : MonoBehaviour
{
    public GameObject player;
    private scp_PlayerMovement playerScript;

    private TilemapCollider2D myCollider;

    // Start is called before the first frame update
    void Start()
    {
        playerScript = player.GetComponent<scp_PlayerMovement>();
        myCollider = GetComponent<TilemapCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        // Disable when the player is in drone mode
        if (playerScript.playerState == scp_PlayerMovement.playerMode.droneMode)
        {
            myCollider.enabled = false;
        }
        else
        {
            myCollider.enabled = true;
        }
    }
}

