using System.Collections;
using UnityEngine;

public class CombinedOverlayManager : MonoBehaviour
{
    public GameObject[] tutorialOverlays;  // Manual overlays shown on click
    public GameObject[] timedOverlays;     // Auto overlays shown with delay
    private int tutorialIndex = 0;
    private bool tutorialDone = false;

    void Start()
    {
        Time.timeScale = 0f;
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
            yield return new WaitForSecondsRealtime(2f);
            overlay.SetActive(false);
        }

        Time.timeScale = 1f; // Resume the game
        Debug.Log("Game starts!");
    }
}