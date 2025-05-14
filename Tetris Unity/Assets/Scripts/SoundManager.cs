using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance;

    public AudioSource audioSource;
    public AudioClip brickSound;
    public AudioSource musicSource;
public AudioClip backgroundMusic;

   private void Awake()
{
    if (Instance == null)
        Instance = this;
    else
        Destroy(gameObject);

    // Start background music
    if (musicSource != null && backgroundMusic != null)
    {
        musicSource.clip = backgroundMusic;
        musicSource.loop = true;
        musicSource.Play();
        musicSource.volume = 0.06f;
    }
}

public void ToggleBrickSounds(bool enabled)
{
    audioSource.mute = !enabled;
}

public void ToggleMusic(bool enabled)
{
    musicSource.mute = !enabled;
}

    public void PlayBrickSound()
    {
        audioSource.PlayOneShot(brickSound);
    }
    
}
