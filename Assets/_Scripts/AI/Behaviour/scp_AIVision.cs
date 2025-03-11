using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Simulates AI line of sight
/// </summary>
public class scp_AIVision : MonoBehaviour
{
    scp_AIScript aiScript;
    bool iHaveLooked;

    [Header("VisionRangeCollider")]
    [HideInInspector] public CircleCollider2D visionCollider;

    [Header("Raycasting")]
    public float raycastRange;
    public LayerMask rayLayermask;
    public int rayConesPerLook = 3;
    int conesLeftInLook;

    [Header("Cone")]
    public int numberOfRays;
    public float coneAngle;

    float maxVisionAngle;
    float currentVisionAngle;
    float visionAngleIncrement;

    float timeToNextLook = 0;

    // Awake is called when the script instance is loaded
    private void Awake()
    {
        aiScript = GetComponent<scp_AIScript>();
        visionCollider = GetComponentInChildren<CircleCollider2D>();

        conesLeftInLook = rayConesPerLook;
        visionAngleIncrement = (coneAngle / numberOfRays)*Mathf.PI/180;

        if (visionCollider != null)
            visionCollider.enabled = false;
    }

    // Update is called once per frame
    private void Update()
    {
        if (visionCollider != null)
        {
            timeToNextLook -= Time.deltaTime;

            // Only enable vision colliders occasionally, and one at a time, to keep performance in check
            if (timeToNextLook < 0 && !SquadMateIsLooking() && visionCollider.enabled == false && !iHaveLooked)
            {
                StartCoroutine(Look());
            }
            else if (SquadMatesHaveLooked())
            {
                iHaveLooked = false;
                foreach (scp_AIScript squadMate in aiScript.mySquadScripts)
                {
                    squadMate.visionScript.iHaveLooked = false;
                }
            }

            if (aiScript.movementScript.previousMovement.magnitude > Time.deltaTime)
            {
                visionCollider.transform.eulerAngles = new Vector3(0, 0, aiScript.movementScript.myRotation);
            }
        }
    }

    // Returns true if a squadmate is looking
    bool SquadMateIsLooking()
    {
        foreach (scp_AIScript squadMate in aiScript.mySquadScripts)
        {
            if (squadMate.visionScript.visionCollider != null)
                if (squadMate.visionScript.visionCollider.enabled == true)
                    return true;
        }
        return false;
    }

    // Returns false if a squadmate hasn't looked
    bool SquadMatesHaveLooked()
    {
        if (aiScript.mySquad.Length != 0)
        {
            foreach (scp_AIScript squadMate in aiScript.mySquadScripts)
            {
                if (squadMate.stateMachine.GetCurrentState() != squadMate.stateMachine.dead)
                    if (!squadMate.visionScript.iHaveLooked)
                        return false;
            }
        }
        return true;
    }

    // Check if the seen object is the player
    private void IHaveSeenThis(GameObject seenObject)
    {
        if (seenObject.tag == "Player")
        {

            SeenPlayer(seenObject, aiScript.audioScript.mySentences.seePlayer);

        }
        else if (seenObject.tag == "PlayerGun")
        {

            seenObject = GameObject.Find("Player");
            SeenPlayer(seenObject, aiScript.audioScript.mySentences.seePlayer);

        }
        else if ((seenObject.tag == "PlayerDrone") & (aiScript.pursueDrone))
        {

            SeenPlayer(seenObject, aiScript.audioScript.mySentences.seeLizard);
        }
    }

    // Called if the seen object is the player
    private void SeenPlayer(GameObject player, string[] sentence)
    {
        // If appropriate say I've seen the player
        if (!aiScript.iCanSeeThePlayer)
        {
            StartCoroutine(aiScript.audioScript.mySentences.SaySentence(sentence));
        }
        aiScript.TellSquadmatesToPursue(player);
        aiScript.PursueSeenObject(player);
    }

    //Send out a raycast, check if it has collided with something.
    private void CreateRaycast(Vector2 rayDirection)
    {
        RaycastHit2D ray = Physics2D.Raycast(transform.position, rayDirection, raycastRange, rayLayermask);
        Debug.DrawRay(transform.position, rayDirection*raycastRange, Color.red, 0.05f);

        if (ray.collider != null)
        {
            IHaveSeenThis(ray.collider.gameObject);
        }
    }

    /// <summary>
    /// Creates a cone of raycasts in the specified direction
    /// </summary>
    /// <param name="angle">The cone direction</param>
    public void CreateRayCone(float angle)
    {
        // Prevent too many raycasts from being made
        if (conesLeftInLook <= 0)
            return;

        conesLeftInLook--;

        //Create a cone of raycasts

        maxVisionAngle = angle + coneAngle * 0.5f * Mathf.PI / 180;

        currentVisionAngle = angle - coneAngle * 0.5f * Mathf.PI / 180;

        while (currentVisionAngle < maxVisionAngle)
        {
            CreateRaycast(new Vector2(Mathf.Cos(currentVisionAngle), Mathf.Sin(currentVisionAngle)));

            currentVisionAngle += visionAngleIncrement;
        }
    }

    // Enables the vision collider for a delay
    IEnumerator Look()
    {
        visionCollider.enabled = true;
        conesLeftInLook = rayConesPerLook;

        yield return new WaitForFixedUpdate();
        yield return new WaitForEndOfFrame();
        yield return new WaitForFixedUpdate();

        iHaveLooked = true;
        visionCollider.enabled = false;
        timeToNextLook = 0.05f;
    }
}
