using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Disables the grapple point until the button is on
/// </summary>
public class scp_ButtonGrapple : MonoBehaviour
{
    // Button
    private bool isOn;
    public scp_Button myButton;

    // Grapple point
    private scp_GrapplePoint myGrappleScript;
    private BoxCollider2D myCollider;

    // Sprites
    private SpriteRenderer mySprite;
    public Sprite onSprite;
    public Sprite offSprite;

    private void Start()
    {
        // Get references
        mySprite = GetComponent<SpriteRenderer>();
        myGrappleScript = GetComponent<scp_GrapplePoint>();
        myCollider= GetComponent<BoxCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        // Turn on or off the grapple point based on the button
        isOn = myButton.iAmOn;

        if (isOn)
        {
            myGrappleScript.enabled = true;
            myCollider.enabled = true;
            mySprite.sprite = onSprite;
        }
        else
        {
            myGrappleScript.enabled = false;
            myCollider.enabled = false;
            mySprite.sprite = offSprite;
        }
    }
}
