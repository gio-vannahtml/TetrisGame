using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{
    public GameObject pausePanel; // Drag your Pause Panel here
    public Button pauseButton;    // Drag your Pause Button here

    void Start()
    {
        pausePanel.SetActive(false);
        pauseButton.onClick.AddListener(PauseGame);
    }

    public void PauseGame()
    {
        Time.timeScale = 0f; // Freeze game time
        pausePanel.SetActive(true);
    }

    public void ResumeGame()
    {
        Time.timeScale = 1f;
        pausePanel.SetActive(false);
    }

    public void RestartGame()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void ExitToMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenu"); // Change to your menu scene name
    }
}