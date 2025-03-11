using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// A door that opens when its button is on
/// </summary>
public class scp_Door : MonoBehaviour
{
    // Sprites
    public scp_Button myButton;

    // Component References
    private SpriteRenderer mySprite;
    private Animator myAnimator;
    private BoxCollider2D myCollider;

    // Start is called before the first frame update
    void Start()
    {
        // Initialise variables
        mySprite = GetComponent<SpriteRenderer>();
        myCollider = GetComponent<BoxCollider2D>();
        myAnimator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        // Open or close based on button state
        if (myButton.iAmOn)
        {
            myCollider.enabled = false;
            myAnimator.SetBool("open", true);
        }
        else
        {
            myCollider.enabled = true;
            myAnimator.SetBool("open", false);
        }
    }
}
