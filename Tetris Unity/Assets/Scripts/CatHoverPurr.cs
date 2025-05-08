using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;

public class CatHoverPurr : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public AudioSource purrAudio;
    public TextMeshProUGUI purrText;

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (!purrAudio.isPlaying)
            purrAudio.Play();

        purrText.gameObject.SetActive(true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (purrAudio.isPlaying)
            purrAudio.Stop();

        purrText.gameObject.SetActive(false);
    }
}


