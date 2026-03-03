using System.Collections;
using System.Collections.Generic;
using UnityEngine;



[RequireComponent(typeof(AudioSource))]
public class musicController : MonoBehaviour
{
    [Header("The Playlist")]
    [Tooltip("Drag your music tracks here")]
    public AudioClip[] tracks;

    [Header("Settings")]
    [Tooltip("Time in seconds (Min, Max) to wait between tracks")]
    public Vector2 silenceGap = new Vector2(10f, 30f);

    [Range(0f, 1f)]
    public float volume = 0.5f;

    private AudioSource audioSource;
    private int lastTrackIndex = -1;

    void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        audioSource.playOnAwake = false;
        audioSource.loop = false; // Important: We handle the looping manually
        audioSource.volume = volume;
    }

    void Start()
    {
        if (tracks.Length > 0)
        {
            StartCoroutine(MusicLoop());
        }
        else
        {
            Debug.LogWarning("AmbientDJ: No tracks assigned in the inspector!");
        }
    }

    private IEnumerator MusicLoop()
    {
        // Give a small delay at the very start of the game so music doesn't blast instantly
        yield return new WaitForSeconds(2f);

        while (true) // Infinite loop that runs as long as the object exists
        {
            // 1. Pick a random track (that isn't the last one played)
            int randomIndex = GetRandomTrackIndex();
            AudioClip currentClip = tracks[randomIndex];

            // 2. Play the track
            audioSource.clip = currentClip;
            audioSource.Play();

            // Optional: Log what's playing
            // Debug.Log($"Now Playing: {currentClip.name}");

            // 3. Wait for the track duration to finish
            yield return new WaitForSeconds(currentClip.length);

            // 4. Wait for the silence period
            float waitTime = Random.Range(silenceGap.x, silenceGap.y);
            // Debug.Log($"Silence for {waitTime} seconds...");

            yield return new WaitForSeconds(waitTime);
        }
    }

    private int GetRandomTrackIndex()
    {
        // If there is only 1 track, just return it
        if (tracks.Length == 1) return 0;

        int newIndex;
        do
        {
            newIndex = Random.Range(0, tracks.Length);
        } while (newIndex == lastTrackIndex); // Keep rolling if we got the same track as before

        lastTrackIndex = newIndex;
        return newIndex;
    }
}