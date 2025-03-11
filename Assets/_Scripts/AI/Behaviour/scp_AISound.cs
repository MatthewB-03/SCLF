using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Manages the AI's audio output
/// </summary>
public class scp_AISound : MonoBehaviour
{
    [HideInInspector] public AudioSource audioSource;

    public AudioClip[] deathSounds;
    public scp_SentenceConstructor mySentences;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    /// <summary>
    /// Coroutine that plays a death sound and destroys the sentence maker
    /// </summary>
    public IEnumerator DeathAudio()
    {
        // I can't speak if I'm dead
        Destroy(mySentences.gameObject);

        // Make a death sound
        audioSource.clip = deathSounds[Random.Range(0, deathSounds.Length)];
        audioSource.Play();

        yield return new WaitForSeconds(audioSource.clip.length);

        audioSource.enabled = false;
    }

}
