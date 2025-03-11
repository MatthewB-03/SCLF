using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Disables or destroys the explosion after the set time expires
/// </summary>
public class scp_Explosion : MonoBehaviour
{
    [SerializeField] bool destroy;
    [SerializeField] float delay;

    // Called when the script is enabled or the object is activated
    void OnEnable()
    {
        StartCoroutine(WaitForDelay());
    }

    IEnumerator WaitForDelay()
    {
        yield return new WaitForSeconds(delay);

        if (destroy)
        {
            Destroy(gameObject);
        }
        else
        {
            gameObject.SetActive(false);
        }
    }
}
