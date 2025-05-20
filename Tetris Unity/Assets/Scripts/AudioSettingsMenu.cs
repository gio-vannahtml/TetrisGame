using UnityEngine;
using UnityEngine.UI;

public class AudioSettingsMenu : MonoBehaviour
{
    public Button musicToggleButton;
    public Button soundToggleButton;

    public Sprite musicOnSprite;
    public Sprite musicOffSprite;
    public Sprite soundOnSprite;
    public Sprite soundOffSprite;

    private bool musicOn = true;
    private bool soundOn = true;

    private Image musicImage;
    private Image soundImage;

    void Start()
    {
        // Get image components
        musicImage = musicToggleButton.GetComponent<Image>();
        soundImage = soundToggleButton.GetComponent<Image>();

        // Add listeners
        musicToggleButton.onClick.AddListener(ToggleMusic);
        soundToggleButton.onClick.AddListener(ToggleSound);

        UpdateUI();
    }

    void ToggleMusic()
    {
        musicOn = !musicOn;
        SoundManager.Instance.ToggleMusic(musicOn);
        UpdateUI();
    }

    void ToggleSound()
    {
        soundOn = !soundOn;
        SoundManager.Instance.ToggleBrickSounds(soundOn);
        UpdateUI();
    }

    void UpdateUI()
    {
        if (musicImage != null)
            musicImage.sprite = musicOn ? musicOnSprite : musicOffSprite;

        if (soundImage != null)
            soundImage.sprite = soundOn ? soundOnSprite : soundOffSprite;
    }
}

