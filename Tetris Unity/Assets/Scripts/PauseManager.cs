using UnityEngine;
using UnityEngine.UI;

public class PauseManager : MonoBehaviour
{
    public GameObject pauseOverlay;
    public Button pauseButton;

    void Start()
    {
        pauseButton.onClick.AddListener(PauseGame);
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