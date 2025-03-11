using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Singleton pattern for use in one instance managers
/// </summary>
/// <typeparam name="script">The script inheriting from singleton</typeparam>
public abstract class scp_Singleton<script> : MonoBehaviour where script : scp_Singleton<script>
{
    // Current instance of the inheriting script
    static script instance;

    /// <summary>
    /// The current singleton instance
    /// </summary>
    public static script Instance
    {
        get
        {
            if (instance == null)
            {
                // Attempt to find an instance
                instance = FindAnyObjectByType<script>();

                if (instance == null) 
                {
                    // Create a new instance
                    instance = new GameObject().AddComponent<script>();
                }
            }

            return instance;
        }
    }

    // Awake is called when the script instance is loaded
    protected virtual void Awake()
    {
        // If an instance already exists, destroy this one
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            // If no instance exists, make this the singleton instance
            instance = gameObject.GetComponent<script>();
            DontDestroyOnLoad(this);
        }
    }
}
