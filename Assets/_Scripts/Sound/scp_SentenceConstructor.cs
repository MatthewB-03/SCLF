using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Sets up and plays various AI sentences
/// </summary>
public class scp_SentenceConstructor : MonoBehaviour
{
    [Header("Sentences")]
    public string[] seeFriend;
    public string[] seePlayer;
    public string[] seeLizard;

    public string[] lostPlayer;

    [Header("Sound IDs")]
    public string[] id;
    public AudioClip[] sound;

    private AudioSource mySource;

    [Header("Sentece cool down")]
    public float coolDown;
    public bool canSpeak;

    // Start is called before the first frame update
    void Start()
    {
        mySource = GetComponent<AudioSource>();
        
        scp_TrueRandom SeedManager = GameObject.Find("RandomManager").GetComponent<scp_TrueRandom>();
        Random.seed = SeedManager.GetRandomSeed();

        mySource.pitch = Random.Range(0.8f, 1.2f);

        canSpeak = true;
        coolDown = 10;
    }

    /// <summary>
    /// Plays a random sentence from the specified type, unless it's still on cooldown
    /// </summary>
    /// <param name="sentence">The sentece type that should be chosen from</param>
    public IEnumerator SaySentence(string[] sentence)
    {
        if (canSpeak && mySource != null)
        {
            // Pick random sentence and split it by id
            int randomIndex = Random.Range(0, sentence.Length);
            string[] sentence2 = sentence[randomIndex].Split(" ");

            // Play the correct sounds using the ids
            for (int i = 0; i < sentence2.Length; i++)
            {
                int idIndex = -1;
                bool found = false;

                while (idIndex < id.Length & found == false)
                {
                    idIndex++;
                    if (id[idIndex] == sentence2[i])
                    {
                        found = true;
                    }
                }
                if (found == true)
                {
                    // Play the sound, wait for it to finish
                    mySource.clip = sound[idIndex];
                    mySource.Play();
                    yield return new WaitForSecondsRealtime(sound[idIndex].length);
                }

            }
                
            // Cooldown to reduce the number of sentences
            canSpeak = false;

        }
        yield return new WaitForSeconds(coolDown);

        canSpeak = true;
    }
}
