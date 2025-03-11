using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Sets the timescale on scene load
/// </summary>
public class scp_TimeSetter : MonoBehaviour
{
    [Header("Extra options")]
    public bool cursorVisible = false;
    public bool resetCheckpoints = false;

    // Start is called before the first frame update
    void Start()
    {
        // Fixes pause menu related issues when loading scenes
        Time.timeScale = 1;
        if (cursorVisible)
        {
            Cursor.visible = true;
        }
        if (resetCheckpoints)
        {
            scp_CheckpointManager.Instance.SetCheckpoint(new Vector3(0, 0, 0));
        }
    }

}
