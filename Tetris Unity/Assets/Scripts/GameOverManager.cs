using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameOverManager : MonoBehaviour
{
    public GameObject gameOverOverlay;
    public Button playAgainButton;
    public Button exitButton;

    void Start()
    {
        if (playAgainButton != null)
            playAgainButton.onClick.AddListener(RestartGame);

        if (exitButton != null)
            exitButton.onClick.AddListener(ExitToMenu);
    }

    public void ShowGameOverOverlay()
    {
        Time.timeScale = 0f; // Optional: pause the game
        if (gameOverOverlay != null)
            gameOverOverlay.SetActive(true);
    }

    public void RestartGame()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void ExitToMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("Menu Screen"); // Replace with your actual menu scene name
    }
}
