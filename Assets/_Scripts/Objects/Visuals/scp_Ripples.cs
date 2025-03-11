using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Plays a rippling animation on its sprite renderer
/// </summary>
public class scp_Ripples : MonoBehaviour
{
    public Sprite[] sprites;
    public bool randomSprite;
    public bool randomTime;

    public float secondsMin;
    public float secondsMax;

    int spriteIndex = 0;

    private SpriteRenderer mySprite;

    // Start is called before the first frame update
    void Start()
    {
        mySprite = GetComponent<SpriteRenderer>();

        scp_TrueRandom SeedManager = GameObject.Find("RandomManager").GetComponent<scp_TrueRandom>();
        Random.seed = SeedManager.GetRandomSeed();

        if (sprites.Length != 0)
        {
            mySprite.sprite = sprites[0];
            StartCoroutine(SwapSprite());
        }
    }

    // Animates rippling or flowing liquid sprites
    IEnumerator SwapSprite()
    {
        // Wait a delay
        if (randomTime) 
        {
            yield return new WaitForSeconds(Random.Range(secondsMin, secondsMax));
        }
        else
        {
            yield return new WaitForSeconds(secondsMin);
        }

        // Pick the next sprite
        if (!randomSprite)
        {
            // Set the next sprite
            if (spriteIndex < sprites.Length-1)
            {
                spriteIndex++;
            }
            else
            {
                spriteIndex = 0;
            }
            mySprite.sprite = sprites[spriteIndex];
        }
        else
        {
            // Pick a random sprite
            mySprite.sprite = sprites[Random.Range(0, sprites.Length - 1)];
        }
        StartCoroutine(SwapSprite());
    }
}
