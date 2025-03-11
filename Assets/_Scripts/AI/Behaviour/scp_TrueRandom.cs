using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Make the game more unpredictable using a random seed
/// </summary>
public class scp_TrueRandom : scp_Singleton<scp_TrueRandom>
{
    protected static string instanceName = "RandomManager";

    [Header("SeedRolling")]
    public bool rollOnStart;
    public bool rerollEveryFrame;

    // Start is called before the first frame update
    void Start()
    {
        if (rollOnStart) 
        {
            Random.seed = System.DateTime.Now.Millisecond * System.DateTime.Now.Second * System.DateTime.Now.Day;
            StartCoroutine(Roll());
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (rerollEveryFrame)
        {
            StartCoroutine(Roll());
        }
    }

    /// <summary>
    /// After a random timeframe, reset the seed.
    /// </summary>
    IEnumerator Roll()
    {
        yield return new WaitForSeconds(Random.Range(0, 10) * 0.1f);
        Random.seed = System.DateTime.Now.Millisecond * System.DateTime.Now.Second * System.DateTime.Now.Day;
    }

    /// <summary>
    /// Get the current random seed 
    /// </summary>
    /// <returns>The current random seed</returns>
    public int GetRandomSeed()
    {
        return Random.seed;
    }
}
