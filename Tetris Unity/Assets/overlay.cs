using System.Collections;
using UnityEngine;

public class Overlay : MonoBehaviour
{
    public GameObject tutorialOverlay1;
    public GameObject tutorialOverlay2;
    public GameObject overlay1;
    public GameObject overlay2;
    public GameObject overlay3;

    void Start()
    {
        Time.timeScale = 0f; // Pause the game at start
        StartCoroutine(ShowOverlays());
    }

    IEnumerator ShowOverlays()
    {
        // Pause the game globally
        Time.timeScale = 0f;

        // Show tutorial overlay 1
        tutorialOverlay1.SetActive(true);
        yield return new WaitForSecondsRealtime(2f);
        tutorialOverlay1.SetActive(false);

        // Show tutorial overlay 2
        tutorialOverlay2.SetActive(true);
        yield return new WaitForSecondsRealtime(2f);
        tutorialOverlay2.SetActive(false);

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
        yield return new WaitForSecondsRealtime(2f);
        overlay3.SetActive(false);

        // Resume the game
        Time.timeScale = 1f;
        Debug.Log("Game starts!");
    }
}