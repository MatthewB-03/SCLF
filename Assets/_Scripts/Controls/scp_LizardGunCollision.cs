using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Adds collision to the lizard gun
/// </summary>
public class scp_LizardGunCollision : MonoBehaviour
{
    public GameObject LizardGun;
    private Transform myTransform;
    private Transform gunTransform;
    // Start is called before the first frame update
    void Start()
    {
        myTransform = GetComponent<Transform>();
        gunTransform = LizardGun.GetComponent<Transform>();
    }

    // Update is called once per frame
    void Update()
    {
        myTransform.position = gunTransform.position;
        myTransform.rotation = gunTransform.rotation;
    }
}
