 using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Plays the start of and loops the set music track
/// </summary>
public class scp_MusicSource : MonoBehaviour
{
    public AudioClip introSection;
    public AudioClip mainSection;

    private AudioSource mySource;


    // Start is called before the first frame update
    void OnEnable()
    {
        mySource = GetComponent<AudioSource>();
        StartCoroutine(PlayMusic());
    }

    // Play intro section once, then swap to looping section
    IEnumerator PlayMusic()
    {
        mySource.clip = introSection;
        mySource.Play();
        yield return new WaitForSecondsRealtime(introSection.length);
        mySource.clip = mainSection;
        mySource.Play();
    }
}
