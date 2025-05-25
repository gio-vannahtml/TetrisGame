using System.Collections;
using UnityEngine;

public class CombinedOverlayManager : MonoBehaviour
{
    public GameObject overlay1;
    public GameObject overlay2;
    public GameObject overlay3;

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
         yield return null;

        overlay1.SetActive(true);
        yield return new WaitForSecondsRealtime(2.5f);
        overlay1.SetActive(false);

        overlay2.SetActive(true);
        yield return new WaitForSecondsRealtime(2.5f);
        overlay2.SetActive(false);

        overlay3.SetActive(true);
        yield return new WaitForSecondsRealtime(2.5f);
        overlay3.SetActive(false);

        // Now resume gameplay after final overlay
        Time.timeScale = 1f;
        yield return null; // let physics and deltaTime stabilize
        gameStarted = true;
        Debug.Log("Game starts!");
    }
}