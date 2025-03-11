using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Animates one of the health bar hearts
/// </summary>
public class scp_HealthUI : MonoBehaviour
{
    // Animation
    private Animator myAnimator;
    private Image myImage;

    // Health
    private scp_HealthTracker playerHealth;

    public int myHealthMarker;


    // Start is called before the first frame update
    void Start()
    {
        // Initialise variables
        playerHealth = GameObject.Find("Player").GetComponent<scp_HealthTracker>();
        myAnimator = GetComponent<Animator>();
        myImage = GetComponent<Image>();
    }

    // Update is called once per frame
    void Update()
    {
        // Set level of damage on this heart
        float level = 5+(playerHealth.myHealth - myHealthMarker);
        myAnimator.SetFloat("level", level);

        if (level <= 0)
        {
            myImage.enabled = false;
        }
    }
}
