using UnityEngine;

public class PauseManager : MonoBehaviour
{
    public GameObject pauseOverlay;

    public void PauseGame()
    {
        pauseOverlay.SetActive(true);
        Time.timeScale = 0f; // Freeze game
    }

    public void ResumeGame()
    {
        pauseOverlay.SetActive(false);
        Time.timeScale = 1f; // Resume game
    }
}