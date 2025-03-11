using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Sets the player's checkpoint when they enter it's trigger collider
/// </summary>
public class scp_Checkpoint : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Destroy(GetComponent<SpriteRenderer>());
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        // Set this as the checkpoint
        GameObject colliding = collision.gameObject;
        if (colliding.tag == "Player")
        {
            scp_CheckpointManager.Instance.SetCheckpoint(transform.position);
        }
    }
}
