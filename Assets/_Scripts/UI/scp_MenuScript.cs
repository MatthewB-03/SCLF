using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Can activate and deactivate menu items, or load a scene, when a UI button is pressed
/// </summary>
public class scp_MenuScript : MonoBehaviour
{
    public GameObject[] gameObjectsToActivate;
    public GameObject[] gameObjectsToDeactivate;
    public int sceneIndexToLoad = -1;

    /// <summary>
    /// Called by the pressed button
    /// </summary>
    public void activateAndDeactivateObjects()
    {
        // Activate and deactivate objects
        if (gameObjectsToActivate.Length != 0)
        {
            for (int i = 0; i < gameObjectsToActivate.Length; i++)
            {
                gameObjectsToActivate[i].SetActive(true);
            }
        }

        if (gameObjectsToDeactivate.Length != 0)
        {
            for (int i = 0; i < gameObjectsToDeactivate.Length; i++)
            {
                gameObjectsToDeactivate[i].SetActive(false);
            }
        }

        // Load scene
        if (sceneIndexToLoad != -1)
        {
            SceneManager.LoadScene(sceneIndexToLoad);
        }
    }
}
