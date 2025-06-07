using UnityEngine;
using TMPro;
using System.Collections;

public class FloatingTextManager : MonoBehaviour
{
    [Header("Prefab References")]
    public GameObject floatingTextPrefab;

    [Header("Animation Settings")]
    public float floatSpeed = 1.0f;
    public float fadeTime = 1.0f;
    public float scaleMultiplier = 1.2f;
    
    [Header("Text Styling")]
    public Color pointsColor = Color.yellow;
    public Color comboColor = new Color(1f, 0.5f, 0f); // Orange
    public Color multiplierColor = new Color(0f, 1f, 0.5f); // Green

    private static FloatingTextManager _instance;
    public static FloatingTextManager Instance
    {
        get { return _instance; }
    }

    void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
        }
        else if (_instance != this)
        {
            Destroy(gameObject);
        }
    }

    public void ShowPointsText(Vector3 position, int points, int multiplier = 1)
    {
        if (floatingTextPrefab == null) return;
        
        // Create the points text
        GameObject pointsTextObj = Instantiate(floatingTextPrefab, position, Quaternion.identity);
        TextMeshPro pointsText = pointsTextObj.GetComponent<TextMeshPro>();
        
        // Set the sorting layer to always be on top
        if (pointsText != null)
        {
            pointsText.text = "+" + points;
            pointsText.color = pointsColor;
            //pointsText.sortingLayerName = "UI_Floating_Text";
            pointsText.sortingOrder = 100;  // High order in the layer
            StartCoroutine(AnimateFloatingText(pointsTextObj, Vector3.up, 1.0f));
        }
        
        // Create the multiplier text if > 1
        if (multiplier > 1)
        {
            GameObject multiplierTextObj = Instantiate(floatingTextPrefab, position + new Vector3(0, -0.5f, 0), Quaternion.identity);
            TextMeshPro multiplierText = multiplierTextObj.GetComponent<TextMeshPro>();
            
            if (multiplierText != null)
            {
                multiplierText.text = "x" + multiplier;
                multiplierText.color = multiplierColor;
                multiplierText.fontSize = multiplierText.fontSize * 0.8f; // Smaller font
                StartCoroutine(AnimateFloatingText(multiplierTextObj, Vector3.up * 0.7f, 1.3f));
            }
        }
    }
    
    public void ShowComboText(Vector3 position, int comboCount)
    {
        if (floatingTextPrefab == null || comboCount <= 1) return;
        
        GameObject comboTextObj = Instantiate(floatingTextPrefab, position + new Vector3(0, 1f, 0), Quaternion.identity);
        TextMeshPro comboText = comboTextObj.GetComponent<TextMeshPro>();
        
        if (comboText != null)
        {
            comboText.text = "COMBO x" + comboCount + "!";
            comboText.color = comboColor;
            comboText.fontSize = comboText.fontSize * 1.2f; // Bigger font for emphasis
            StartCoroutine(AnimateFloatingText(comboTextObj, Vector3.up * 1.3f, 1.5f));
        }
    }
    
    private IEnumerator AnimateFloatingText(GameObject textObj, Vector3 direction, float duration)
    {
        TextMeshPro tmp = textObj.GetComponent<TextMeshPro>();
        float elapsed = 0;
        Color startColor = tmp.color;
        Vector3 startScale = textObj.transform.localScale;
        Vector3 targetScale = startScale * scaleMultiplier;
        
        // First phase: scale up
        while (elapsed < duration * 0.3f)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / (duration * 0.3f);
            textObj.transform.localScale = Vector3.Lerp(startScale, targetScale, t);
            textObj.transform.position += direction * floatSpeed * Time.deltaTime;
            yield return null;
        }
        
        // Second phase: float and fade
        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = (elapsed - duration * 0.3f) / (duration * 0.7f);
            tmp.color = new Color(startColor.r, startColor.g, startColor.b, Mathf.Lerp(1f, 0f, t));
            textObj.transform.position += direction * floatSpeed * Time.deltaTime;
            textObj.transform.localScale = Vector3.Lerp(targetScale, startScale * 0.8f, t);
            yield return null;
        }
        
        Destroy(textObj);
    }
}