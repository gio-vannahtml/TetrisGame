using System.Collections;
using UnityEngine;

public class CombinedOverlayManager : MonoBehaviour
{
    [Header("Manual Tutorial Overlays (Click to advance)")]
    public GameObject[] tutorialOverlays;

    [Header("Timed Overlays (Auto-play after tutorial)")]
    public GameObject[] timedOverlays;

    private int tutorialIndex = 0;
    private bool tutorialDone = false;
    public static bool gameStarted = false;

    void Start()
    {
        Time.timeScale = 0f; // Pause all movement including Tetromino falling
        if (tutorialOverlays.Length > 0)
        {
            ShowTutorialOverlay(0);
        }
        else
        {
            tutorialDone = true;
            StartCoroutine(PlayTimedOverlays());
        }
    }

    void Update()
    {
        if (!tutorialDone && (Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.Space)))
        {
            ShowNextTutorialOverlay();
        }
    }

    void ShowTutorialOverlay(int index)
    {
        for (int i = 0; i < tutorialOverlays.Length; i++)
        {
            tutorialOverlays[i].SetActive(i == index);
        }
        tutorialIndex = index;
    }

    void ShowNextTutorialOverlay()
    {
        tutorialOverlays[tutorialIndex].SetActive(false);
        tutorialIndex++;

        if (tutorialIndex < tutorialOverlays.Length)
        {
            tutorialOverlays[tutorialIndex].SetActive(true);
        }
        else
        {
            tutorialDone = true;
            StartCoroutine(PlayTimedOverlays());
        }
    }

    IEnumerator PlayTimedOverlays()
    {
        foreach (GameObject overlay in timedOverlays)
        {
            overlay.SetActive(true);
            yield return new WaitForSecondsRealtime(3f); // Show each overlay for 3 seconds
            overlay.SetActive(false);
        }

        // ✅ Now resume gameplay after final overlay (e.g., overlay3)
        Time.timeScale = 1f;
        gameStarted = true;
        Debug.Log("Game starts!");

        SpawnTetromino();
    }

    void SpawnTetromino()
    {
        Debug.Log("SpawnTetromino called. Implement spawning logic here.");

        // Example:
        // Instantiate(tetrominoPrefab, spawnPosition, Quaternion.identity);
    }
}