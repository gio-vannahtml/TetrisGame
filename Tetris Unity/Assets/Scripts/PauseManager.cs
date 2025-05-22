using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PauseManager : MonoBehaviour
{
    public GameObject pauseOverlay;
    public Button pauseButton;
    public Button resumeButton;
    public Button restartButton;
    public Button exitButton;  // ⬅ Add this

    private bool isPaused = false;

    void Start()
    {
        if (pauseButton != null)
            pauseButton.onClick.AddListener(TogglePause);

        if (resumeButton != null)
            resumeButton.onClick.AddListener(ResumeGame);

        if (restartButton != null)
            restartButton.onClick.AddListener(RestartGame);

        if (exitButton != null)
            exitButton.onClick.AddListener(ExitToMenu);  // ⬅ Hook up Exit
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            TogglePause();
        }
    }

    public void TogglePause()
    {
        if (pauseOverlay == null) return;

        isPaused = !isPaused;
        pauseOverlay.SetActive(isPaused);
        Time.timeScale = isPaused ? 0f : 1f;
    }

    public void ResumeGame()
    {
        isPaused = false;
        pauseOverlay.SetActive(false);
        Time.timeScale = 1f;
    }

    public void RestartGame()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void ExitToMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("Menu Screen"); // Replace with your menu scene's exact name
    }
}