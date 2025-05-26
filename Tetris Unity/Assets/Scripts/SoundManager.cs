using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance;

    public AudioSource musicSource;
    public AudioSource sfxSource;

    public AudioClip backgroundMusic;
    public AudioClip brickSound;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        if (musicSource != null && backgroundMusic != null)
        {
            musicSource.clip = backgroundMusic;
            musicSource.loop = true;
            musicSource.Play();
            musicSource.volume = 0.05f;
        }
    }

    public void ToggleMusic(bool enabled)
    {
        Debug.Log("ToggleMusic called with enabled: " + enabled);
        if (musicSource == null)
        {
            Debug.LogWarning("Music source is not assigned.");
            return;
        }
        musicSource.mute = !enabled;     
    }

    public void ToggleBrickSounds(bool enabled)
    {
        Debug.Log("ToggleMusic called with enabled: " + enabled);
        if (musicSource == null)
        {
            Debug.LogWarning("Music source is not assigned.");
            return;
        }
        if (sfxSource != null)
            sfxSource.mute = !enabled;
    }

    public void PlayBrickSound()
    {
        if (sfxSource != null && brickSound != null)
            sfxSource.PlayOneShot(brickSound);
    }
}

