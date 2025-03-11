using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Spawns a new bomb when it's bomb is displaced
/// </summary>
public class scp_BombSpawn : MonoBehaviour
{
    public GameObject bombPrefab;
    public float spawnDelay;

    private Transform myTransform;
    private Animator myAnimator;
    private GameObject myBomb;

    // Start is called before the first frame update
    void Start()
    {
        myTransform = GetComponent<Transform>();
        myAnimator = GetComponent<Animator>();

        StartCoroutine(WaitThenSpawnNew());
    }

    // Starts the spawning animation after a delay
    IEnumerator WaitThenSpawnNew()
    {
        yield return new WaitForSeconds(spawnDelay);
        myAnimator.SetTrigger("spawn");
    }

    // Creates a new bomb, called in the spawning animation
    public void CreateBomb()
    {
        myBomb = scp_ObjectPooler.Instance.GetObjectFromPool(bombPrefab.name);
        myBomb.GetComponent<scp_Bomb>().pooledBomb = true;
        myBomb.transform.position = transform.position;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        // Spawn a new bomb when the old one is moved
        GameObject colliding = collision.gameObject;
        if (colliding == myBomb)
        {
            myBomb = null;
            StartCoroutine(WaitThenSpawnNew());
        }
    }
}
