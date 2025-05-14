using UnityEngine;
using UnityEngine.UI;

public class SpriteToggleSwap : MonoBehaviour
{
    public Sprite onSprite;
    public Sprite offSprite;
    private SpriteRenderer sr;
    private Toggle toggle;

    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        toggle = GetComponent<Toggle>();
        toggle.onValueChanged.AddListener(UpdateSprite);
        UpdateSprite(toggle.isOn);
    }

    void UpdateSprite(bool isOn)
    {
        if (sr != null)
            sr.sprite = isOn ? onSprite : offSprite;
    }
}
