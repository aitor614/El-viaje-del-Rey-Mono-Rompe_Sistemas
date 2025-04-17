using UnityEngine;

public class SceneAudioTrigger : MonoBehaviour
{
    public AudioClip soundToPlay;
    public float volume = 1f;

    private AudioSource audioSource;

    void Start()
    {
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.playOnAwake = false;

        if (soundToPlay != null)
        {
            audioSource.PlayOneShot(soundToPlay, volume);
        }
    }
}
