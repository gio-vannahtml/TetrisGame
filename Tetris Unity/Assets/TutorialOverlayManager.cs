using UnityEngine;

public class TutorialOverlayManager : MonoBehaviour
{
    public GameObject[] overlays;
    private int currentIndex = 0;

    void Start()
    {
        Time.timeScale = 0f; // Pause the game at start
        ShowOverlay(0); // Show the first overlay
    }

    public void ShowOverlay(int index)
    {
        for (int i = 0; i < overlays.Length; i++)
        {
            overlays[i].SetActive(i == index);
        }
        currentIndex = index;
    }

    public void NextOverlay()
    {
        int nextIndex = currentIndex + 1;
        if (nextIndex < overlays.Length)
        {
            ShowOverlay(nextIndex);
        }
        else
        {
            HideAll();
            Time.timeScale = 1f;
        }
    }

    public void HideAll()
    {
        foreach (var overlay in overlays)
        {
            overlay.SetActive(false);
        }
    }
}