using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Fires a projectile when its putton is pressed, resets when the button is released
/// </summary>
public class scp_TrapPojectile : MonoBehaviour
{
    // Projectile
    public GameObject projectilePrefab;
    private Transform transform;
    public Vector2 direction;
    public float force;

    // Button
    public scp_Button myButton;

    private bool readyToShoot = true;

    private void Update()
    {
        // Shoot when button is pressed, then wait for button to be released
        if (myButton.iAmOn & readyToShoot)
        {
            ShootProjectile();
        }
        else if (!myButton.iAmOn)
        {
            readyToShoot = true;
        }
    }

    // Shoots a projectile
    private void ShootProjectile()
    {
        GameObject projectile = scp_ObjectPooler.Instance.GetObjectFromPool(projectilePrefab.name);
        projectile.transform.position = transform.position;
        projectile.GetComponent<Rigidbody2D>().AddForce(force * direction);
        projectile.GetComponent<Rigidbody2D>().rotation = transform.eulerAngles.z;
        readyToShoot = false;
    }
    private void Start()
    {
        // Initialise variables
        transform = GetComponent<Transform>();
    }
}
