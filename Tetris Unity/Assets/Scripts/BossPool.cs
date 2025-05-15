using System.Collections.Generic;
using UnityEngine;

public class BossPool : MonoBehaviour
{
    public static BossPool Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
            // Optional: Keep across scenes
            // DontDestroyOnLoad(gameObject);
        }
    }
    public List<GameObject> availableBossPrefabs = new List<GameObject>();

    public void DisableBoss(GameObject bossPrefab)
    {
        if (availableBossPrefabs.Contains(bossPrefab))
        {
            availableBossPrefabs.Remove(bossPrefab);
        }
    }

    public void EnableBoss(GameObject bossPrefab)
    {
        if (!availableBossPrefabs.Contains(bossPrefab))
        {
            availableBossPrefabs.Add(bossPrefab);
        }
    }

    public GameObject GetRandomBoss()
    {
        if (availableBossPrefabs.Count == 0)
        {
            Debug.LogWarning("No available bosses in pool!");
            return null;
        }

        int index = Random.Range(0, availableBossPrefabs.Count);
        return availableBossPrefabs[index];
    }
}