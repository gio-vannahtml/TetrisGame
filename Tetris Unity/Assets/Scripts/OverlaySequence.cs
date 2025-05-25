using System.Collections;
using UnityEngine;

public class OverlaySequence : MonoBehaviour
{
    public GameObject overlay1;
    public GameObject overlay2;
    public GameObject overlay3;

    void Start()
    {
        StartCoroutine(ShowOverlays());
    }

    IEnumerator ShowOverlays()
    {
        // Show overlay 1
        overlay1.SetActive(true);
        yield return new WaitForSeconds(3f);
        overlay1.SetActive(false);

        // Show overlay 2
        overlay2.SetActive(true);
        yield return new WaitForSeconds(3f);
        overlay2.SetActive(false);

        // Show overlay 3
        overlay3.SetActive(true);
        yield return new WaitForSeconds(3f);
        overlay3.SetActive(false);

        // Now game starts (you can call any game start logic here)
        Debug.Log("Game starts!");
    }
}