using System.Collections;
using UnityEngine;

public class Overlay : MonoBehaviour
{
    public GameObject overlay1;
    public GameObject overlay2;
    public GameObject overlay3;

void Start()
    {
        Time.timeScale = 0f;
        StartCoroutine(ShowOverlays());
    }

    IEnumerator ShowOverlays()
    {
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

        // Now game starts (you can call any game start logic here)
        Time.timeScale = 1f;
        Debug.Log("Game starts!");
    }
}