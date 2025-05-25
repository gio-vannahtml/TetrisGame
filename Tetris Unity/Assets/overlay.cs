using System.Collections;
using UnityEngine;

public class Overlay : MonoBehaviour
{
    public static bool gameStarted = false;
    public bool gameReady = false; // Added declaration for gameReady

    public GameObject overlay1;
    public GameObject overlay2;
    public GameObject overlay3;
    public GameObject overlays; // Add this line to declare the overlays GameObject

    void Start()
    {
        Time.timeScale = 0f; // Pause the game at start
        StartCoroutine(ShowOverlays());

        IEnumerator ShowOverlays()
        {
            // Show your overlay
            overlays.SetActive(true);

            // Wait for it to finish
            yield return new WaitForSeconds(3.5f);

            overlays.SetActive(false);

            gameReady = true; // Now the game can start

            // Pause the game globally
            Time.timeScale = 0f;
            // Show overlay 1
            overlay1.SetActive(true);
            yield return new WaitForSecondsRealtime(2f);
            overlay1.SetActive(false);

            // Show overlay 2
            overlay2.SetActive(true);
            yield return new WaitForSecondsRealtime(2f);
            overlay2.SetActive(false);

            // Show overlay 3
            overlay3.SetActive(true);
            yield return new WaitForSecondsRealtime(1f);
            overlay3.SetActive(false);

            // Resume the game
            Time.timeScale = 2f;
            gameStarted = true; // ✅ Signal game can begin
            Debug.Log("Game starts!");

            SpawnTetromino();
        }
    }

    // Placeholder for SpawnTetromino method
    private void SpawnTetromino()
    {
        // TODO: Implement Tetromino spawning logic here
        Debug.Log("SpawnTetromino called.");
    }
    
}