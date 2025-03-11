using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Keeps track of the player's checkpoint
/// </summary>
public class scp_CheckpointManager : scp_Singleton<scp_CheckpointManager>
{
    private Vector2 lastCheckpoint;
    private GameObject player;
    int currentScene;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player");
    }

    // Awake is called whe the script instance is loaded
    protected override void Awake()
    {
        base.Awake();
        currentScene = SceneManager.GetActiveScene().buildIndex;
    }

    // Called when a new scene is loaded
    void OnLevelWasLoaded(int level)
    {
        Start();

        // Only move player if there is a set checkpoint on this scene
        if (lastCheckpoint.x != 0 && lastCheckpoint.y != 0 && currentScene == SceneManager.GetActiveScene().buildIndex)
        {
            player.transform.position = lastCheckpoint;
        }
        currentScene = SceneManager.GetActiveScene().buildIndex;
    }

    /// <summary>
    /// Set the player's checkpoint.
    /// </summary>
    /// <param name="checkpoint">The position of the checkpoint</param>
    public void SetCheckpoint(Vector2 checkpoint)
    {
        lastCheckpoint = checkpoint;
    }
}
