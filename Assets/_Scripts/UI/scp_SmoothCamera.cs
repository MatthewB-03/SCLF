using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Gives the camera smoother movement
/// </summary>
public class scp_SmoothCamera : MonoBehaviour
{
    public GameObject player;
    private Transform playerTransform;
    private Transform myTransform;
    public float pull;

    private Vector2 playerPosition;

    public Vector2 maxDistance;

    public bool frozen = false;

    // Start is called before the first frame update
    void Start()
    {
        myTransform = GetComponent<Transform>();
        playerTransform = player.GetComponent<Transform>();
        playerPosition = playerTransform.position;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (!frozen)
        {
            // Move back towards the player
            Vector2 offset = new Vector2(myTransform.localPosition.x - 0, myTransform.localPosition.y - 0) * -pull * Time.deltaTime;

            myTransform.localPosition = new Vector2(myTransform.localPosition.x + offset.x, myTransform.localPosition.y + offset.y);
        }

        // Stay behind player movement
        Vector2 movement = new Vector2(playerPosition.x - playerTransform.position.x, playerPosition.y - playerTransform.position.y);

        myTransform.localPosition = new Vector2(myTransform.localPosition.x + movement.x, myTransform.localPosition.y + movement.y);

        playerPosition = playerTransform.position;

        myTransform.localPosition = new Vector3(myTransform.localPosition.x, myTransform.localPosition.y, -10);
    }
}
