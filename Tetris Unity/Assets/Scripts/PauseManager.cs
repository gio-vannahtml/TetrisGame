using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseManager : MonoBehaviour
{
    public GameObject pauseOverlay;

    public void TogglePause()
    {
        bool isPaused = pauseOverlay.activeSelf;
        pauseOverlay.SetActive(!isPaused);
        Time.timeScale = isPaused ? 1f : 0f;
    }

    public void ResumeGame()
    {
        pauseOverlay.SetActive(false);
        Time.timeScale = 1f;
    }

    public void RestartGame()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void ExitToMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenu"); // replace with your menu scene name
    }
}