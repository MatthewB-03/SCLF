using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Activates and deactivates specified objects when the player enters its trigger
/// </summary>
public class scp_Trigger : MonoBehaviour
{
    public GameObject[] gameObjectsToActivate;
    public GameObject[] gameObjectsToDeactivate;
    public bool oneTime = true;
    public bool acceptsDrone = true;

    void Start()
    {
        Destroy(GetComponent<SpriteRenderer>());
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        GameObject colliding = collision.gameObject;

        if ((colliding.tag == "Player") | (colliding.tag == "PlayerDrone" & acceptsDrone))
        {
            // Activate relevant objects
            for (int i = 0; i < gameObjectsToActivate.Length; i++)
            {
                gameObjectsToActivate[i].SetActive(true);
            }

            // De-activate relevant objects
            for (int i = 0; i < gameObjectsToDeactivate.Length; i++)
            {
                gameObjectsToDeactivate[i].SetActive(false);
            }

            // Destroy if it's a one time trigger
            if (oneTime)
            {
                Destroy(gameObject);
            }
        }
    }
}
