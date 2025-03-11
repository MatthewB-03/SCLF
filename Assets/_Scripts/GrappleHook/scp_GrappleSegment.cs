using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// A segment in a grapple chain, the segment is pulled towards the previous segment
/// </summary>
public class scp_GrappleSegment : MonoBehaviour
{
    // Grapple point
    public bool iAmTheGrapplePoint;

    // Previous segment
    public scp_GrappleSegment previousSegment;
    private Transform previousTransform;
    private Rigidbody2D previousRigidbody;

    // Component References
    private Rigidbody2D myRigidbody;

    private SpriteRenderer mySprite;

    // Movement
    public float tension;
    public float myRadius;

    public float waitInSeconds;

    public bool waitOver = false;
    public bool reachedTheGrapplePoint = false;

    public float noFasterThanThis;

    // Transform
    public Transform playerTransform;
    public Transform grapplePointTransform;


    // Start is called before the first frame update
    void Start()
    {
        // Initialise variables
        myRigidbody = GetComponent<Rigidbody2D>();
        mySprite = GetComponent<SpriteRenderer>();

        if (!iAmTheGrapplePoint)
        {
            previousTransform = previousSegment.GetComponent<Transform>();
            previousRigidbody = previousSegment.GetComponent<Rigidbody2D>();
        }
        else
        {
            myRigidbody.constraints = RigidbodyConstraints2D.FreezePosition;
        }
        StartCoroutine(WaitThenAppear());

    }
    // Update is called once per frame
    void Update()
    {
        CheckIfAtGrapplePoint();

        //Don't pull until all segments are ready
        if (!iAmTheGrapplePoint && !waitOver && previousSegment.waitOver)
        {
                waitOver = true;
        }
        else if (!iAmTheGrapplePoint & waitOver)
        {
            // Pull towards the previous segment
            Vector2 distance = previousTransform.position - transform.position;
            myRigidbody.AddForce(tension * distance);

            // Make sure segments are not too far apart
            if (distance.magnitude > myRadius)
            {
                Vector3 difference = transform.position - previousTransform.position;
                difference *= myRadius / distance.magnitude;
                transform.position = previousTransform.position + difference;
            }

            previousRigidbody.velocity = myRigidbody.velocity;
        }
    }
    void CheckIfAtGrapplePoint()
    {
        //Check if I overshot the grapple point
        Vector2 toPlayer = transform.position - playerTransform.position;
        Vector2 toGPoint = transform.position - grapplePointTransform.position;

        if (((toGPoint.x > 0 & toPlayer.x > 0) | (toGPoint.x < 0 & toPlayer.x < 0) | (toGPoint.y > 0 & toPlayer.y > 0) | (toGPoint.y < 0 & toPlayer.y < 0)) | (toGPoint.magnitude < 0.1f))
        {
            mySprite.enabled = false;
            reachedTheGrapplePoint = true;
        }
    }

    // Waits for the set time, then appears
    IEnumerator WaitThenAppear()
    {
        mySprite.enabled = false;
        yield return new WaitForSecondsRealtime(waitInSeconds);

        if (!reachedTheGrapplePoint)
        {
            mySprite.enabled = true;
        }

        // If the grappling point has appeared, the segments can start moving
        if (iAmTheGrapplePoint)
        {
            waitOver = true;
        }
    }

    // Destroy all segments in the chain
    public void OnDestroy()
    {
        if (!iAmTheGrapplePoint)
        {
            Destroy(previousSegment.gameObject);
        }
    }
}
