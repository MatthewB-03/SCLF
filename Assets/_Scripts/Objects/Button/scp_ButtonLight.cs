using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

/// <summary>
/// A light that turns on when its button is on
/// </summary>
public class scp_ButtonLight : MonoBehaviour
{
    // Button
    public scp_Button button;
    private bool isOn;

    // Light
    private Light2D myLight;
    private SpriteRenderer buttonSprite;

    // Start is called before the first frame update
    void Start()
    {
        // Initialise variables
        buttonSprite = button.GetComponent<SpriteRenderer>();
        myLight = GetComponent<Light2D>();
        isOn = button.iAmOn;

        myLight.color = buttonSprite.color;
    }

    // Update is called once per frame
    void Update()
    {
        // Turn on or off light based on button state
        isOn = button.iAmOn;

        if (isOn)
        {
            myLight.enabled = true;
        }
        else
        {
            myLight.enabled = false;
        }
    }
}
