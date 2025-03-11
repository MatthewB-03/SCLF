using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

/// <summary>
/// Starts the next scene fadeout when a public method is called
/// </summary>
public class scp_NextSceneOnEvent : MonoBehaviour
{
    // Fade out
    public GameObject fadeOutImage;
    private Color fadeColour;
    private bool ready=false;

    private AudioSource mySound;

    // Start is called before the first frame update
    private void Start()
    {
        fadeColour = fadeOutImage.GetComponent<Image>().color;
        mySound = GetComponent<AudioSource>();
    }

    private void FixedUpdate()
    {
        // If set as ready, fade out the scene
        if (ready)
        {
            if (fadeColour.a >= 1)
            {
                NextScene();
            }
            else
            {
                fadeColour.a += 0.02f;
                fadeOutImage.GetComponent<Image>().color = fadeColour;
            }
        }
    }

    // Loads the next scene
    void NextScene()
    {
        // Destroy the current checkpoint manager so it isn't used in the next level
        Destroy(GameObject.Find("CheckpointManager"));
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex+1);
    }

    /// <summary>
    /// Starts the scene fadeout
    /// </summary>
    public void SetReady()
    {
        ready = true;
    }

    /// <summary>
    /// Plays the victory sound
    /// </summary>
    public void PlaySound()
    {
        mySound.Play();
        GameObject.Find("BGMusic").SetActive(false);
    }
}
