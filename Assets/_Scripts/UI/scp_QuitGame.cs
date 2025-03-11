using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Closes the game when a button is pressed
/// </summary>
public class scp_QuitGame : MonoBehaviour
{
    /// <summary>
    /// Called by the quit button
    /// </summary>
    public void quitGame()
    {
        Application.Quit();
    }
}
