using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// A point that the player can grapple to
/// </summary>
public class scp_GrapplePoint : MonoBehaviour
{

    [SerializeField] GameObject player;
    private scp_PlayerMovement playerScript;


    [Header("Grapple Segments")]
    [SerializeField] GameObject grappleSegmentPrefab;
    [SerializeField] float grappleSegmentLength;
    [SerializeField] int grappleSegmentsPerUnit;

    private bool playerIsGrapplingToMe = false;

    [Header("Grapple Pull Delay")]
    [SerializeField] float totalGrappleTime = 0.1f;


    // Start is called before the first frame update
    void Start()
    {
        playerScript = player.GetComponent<scp_PlayerMovement>();
    }

    private void Update()
    {
        if (playerIsGrapplingToMe)
        {
            // Distance from player to grapple point
            Vector3 distance = player.transform.position - transform.position;

            //If the player has reached the grapple point, stop them from grappling
            if (distance.magnitude < 1)
            {
                playerIsGrapplingToMe = false;
                playerScript.StopGrappling();
            }
        }
    }
    /// <summary>
    /// Creates a chain of segments from the grapple point to the player
    /// </summary>
    /// <param name="sortingOrder">The sprite sorting order of the segments</param>
    void CreateGrappleChain(int sortingOrder)
    {
        // Distance from player to grapple point
        Vector3 distance = player.transform.position - transform.position;

        // Number of segments to spawn
        int number = Mathf.RoundToInt(distance.magnitude / grappleSegmentLength) * grappleSegmentsPerUnit;

        // Initial values
        Vector2 currentPosition = transform.position;
        float size = 0.5f;
        float delay = totalGrappleTime;
        scp_GrappleSegment previousSegment = null;

        // Value Increments
        Vector2 positionIncrement = new Vector2(distance.x / number, distance.y / number);
        float sizeIncrement = 0.5f / number;
        float delayDecrement = totalGrappleTime / number;

        for (int i = 0; i < number; i++, currentPosition += positionIncrement, delay -= delayDecrement, size += sizeIncrement)
        {
            // Instantiate prefab
            GameObject segment = Instantiate(grappleSegmentPrefab, currentPosition, Quaternion.identity);

            // Set script variables
            scp_GrappleSegment segmentScript = segment.GetComponent<scp_GrappleSegment>();
            segmentScript.waitInSeconds = delay;
            segmentScript.playerTransform = player.transform;
            segmentScript.grapplePointTransform = transform;

            // Set other values
            segment.transform.localScale = new Vector2(size, size);
            segment.GetComponent<SpriteRenderer>().sortingOrder = sortingOrder;

            // Start grappling when all segements have been created
            if (i == number - 1)
            {
                segmentScript.previousSegment = previousSegment;
                playerScript.StartGrappling(segment);
                playerIsGrapplingToMe = true;
            }
            else if (i == 0)
            {
                segmentScript.iAmTheGrapplePoint = true;
            }
            else
            {
                segmentScript.previousSegment = previousSegment;

            }
            // Set this segment as the previous segment
            previousSegment = segmentScript;
        }
    }
    void OnTriggerStay2D(Collider2D collision)
    {
        GameObject colliding = collision.gameObject;

        //If the player has reached the grapple point, stop them from grappling
        if (colliding.tag == "Player" & playerIsGrapplingToMe)
        {

            playerIsGrapplingToMe = false;
            playerScript.StopGrappling();
        }

        if ((colliding.tag == "Mouse") & (Input.GetKey(KeyCode.Mouse0)) & (playerScript.playerState == scp_PlayerMovement.playerMode.grappleMode))
        {
            GameObject lizardGrapple = GameObject.Find("LizardGrapple");

            if (lizardGrapple != null)
            {
                // Set player state
                playerScript.playerState = scp_PlayerMovement.playerMode.grappling;

                CreateGrappleChain(lizardGrapple.GetComponent<SpriteRenderer>().sortingOrder);
            }

        }
    }
}
