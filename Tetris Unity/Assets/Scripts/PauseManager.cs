using UnityEngine;
using UnityEngine.UI;

public class PauseManager : MonoBehaviour
{
    public GameObject pauseOverlay;
    public Button pauseButton;

    void Start()
    {
        if (pauseButton != null)
        {
            pauseButton.onClick.AddListener(PauseGame);
        }
        else
        {
            Debug.LogError("PauseManager: pauseButton is not assigned in the Inspector!");
        }

    }

    public void PauseGame()
    {
        Debug.Log("PauseGame triggered by button!");
        pauseOverlay.SetActive(true);
        Time.timeScale = 0f;
    }

    public void ResumeGame()
    {
        pauseOverlay.SetActive(false);
        Time.timeScale = 1f;
    }
}