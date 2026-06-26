using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance;

    [Header("Audio")]
    public AudioSource audioSource;

    [Header("Sounds")]
    public AudioClip clickSound;
    public AudioClip successSound;
    public AudioClip failSound;
    public AudioClip waterSound;

    private void Awake()
    {
        Instance = this;

        if (audioSource == null)
            audioSource = GetComponent<AudioSource>();
    }

    public void PlayClick()
    {
        PlaySound(clickSound, 1.5f);
    }

    public void PlaySuccess()
    {
        PlaySound(successSound, 1.2f);
    }

    public void PlayFail()
    {
        PlaySound(failSound, 1.2f);
    }

    public void PlayWater()
    {
        PlaySound(waterSound, 1.3f);
    }

    private void PlaySound(AudioClip clip, float volume)
    {
        if (audioSource == null || clip == null)
            return;

        audioSource.PlayOneShot(clip, volume);
    }
}