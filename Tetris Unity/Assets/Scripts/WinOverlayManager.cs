using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class WinOverlayManager : MonoBehaviour
{
    public GameObject winOverlay;
    public Button continueButton;
    public Button exitButton;

    void Start()
    {
        if (continueButton != null)
            continueButton.onClick.AddListener(LoadNextLevel);

        if (exitButton != null)
            exitButton.onClick.AddListener(ExitToMenu);
    }

    public void ShowWinOverlay()
    {
        Time.timeScale = 0f; // Optional: Pause the game when won
        if (winOverlay != null)
            winOverlay.SetActive(true);
    }

    public void LoadNextLevel()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("AllLevels"); // Replace with actual next scene name
    }

    public void ExitToMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("Menu Screen"); // Replace with your menu scene name
    }
}