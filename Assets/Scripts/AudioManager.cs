
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [Header("Audio Source")]
    [SerializeField] AudioSource AudioSource;
    [SerializeField] AudioSource SfxSource;
    [Header("Audio Clip")]
    public AudioClip spikeJump;
    public AudioClip jump;
    public AudioClip land;
    public AudioClip wallSlide;

    public void PlaySFX(AudioClip clip)
    {
        SfxSource.PlayOneShot(clip);
    }
}
