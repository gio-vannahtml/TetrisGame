using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance;

<<<<<<< HEAD
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
=======
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
        if (musicSource != null)
            musicSource.mute = !enabled;
    }

    public void ToggleBrickSounds(bool enabled)
    {
        if (sfxSource != null)
            sfxSource.mute = !enabled;
    }

    public void PlayBrickSound()
    {
        if (sfxSource != null && brickSound != null)
            sfxSource.PlayOneShot(brickSound);
    }
}

>>>>>>> 51e8c47080d3ad4fe23a37069134019be402f7d3
