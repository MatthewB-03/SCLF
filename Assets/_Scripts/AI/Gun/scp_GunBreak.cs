using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Destroys the gun's game object when the AI dies
/// </summary>
public class scp_GunBreak : MonoBehaviour
{
    public int threshhold;
    private scp_HealthTracker myAiHealth;
    public GameObject destroyEffect;
    public GameObject myAi;

    // Start is called before the first frame update
    void Start()
    {
        myAiHealth = myAi.GetComponent<scp_HealthTracker>();
    }

    // Update is called once per frame
    void Update()
    {
        // Destroy this gun when the AI health drops low enough
        if (myAiHealth.myHealth <= threshhold)
        {
            Instantiate(destroyEffect, GetComponent<Transform>().position, Quaternion.identity);
            Destroy(gameObject);
        }
    }
}
