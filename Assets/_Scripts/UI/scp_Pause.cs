using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Pauses the game and activates a pause menu when escape is pressed
/// </summary>
public class scp_Pause : MonoBehaviour
{
    public GameObject pauseMenu;
    private bool paused = false;

    // Update is called once per frame
    void Update()
    {
        // Stop time and activate menu when paused
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (paused)
            {
                Time.timeScale = 1;
                pauseMenu.SetActive(false);
                paused = false;
            }
            else
            {
                Time.timeScale = 0;
                pauseMenu.SetActive(true);
                paused = true;
            }
        }
    }
}
