using System.Collections;
using UnityEngine;

public class overlay : MonoBehaviour
{
    public GameObject overlay1;
    public GameObject overlay2;
    public GameObject overlay3;

    public static bool gameStarted = false;

    void Start()
    {
        Time.timeScale = 0f; // Pause gameplay
        StartCoroutine(PlayOverlays());
    }

    IEnumerator PlayOverlays()
    {
        yield return null;

        overlay1.SetActive(true);
        yield return new WaitForSecondsRealtime(3f);
        overlay1.SetActive(false);

        overlay2.SetActive(true);
        yield return new WaitForSecondsRealtime(3f);
        overlay2.SetActive(false);

        overlay3.SetActive(true);
        yield return new WaitForSecondsRealtime(3f);
        overlay3.SetActive(false);

        gameStarted = true;

        Debug.Log("Game starts!");
    }
}
