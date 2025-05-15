using UnityEngine;

public class BossManager : MonoBehaviour
{
    // Singleton pattern for easy access across the game
    public static BossManager Instance;

    // Tracks whether the boss is currently active in the game
    public bool isBossActive = false;

    // Reference to the boss object (you can replace this with a class that represents the boss)
    private GameObject bossObject;

    // The boss prefab to spawn (you'll need to assign this in the Unity inspector)
    public GameObject bossPrefab;

    public BossPool bossPool;

    public Boss CurrentBoss { get; private set; } // Expose current boss
    public bool IsBossActive => CurrentBoss != null; // True if a boss is active


    void Awake()
    {
        // Ensure only one instance of BossManager exists
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // Apply the boss: spawns the boss in the game
    public void ApplyBoss()
    {
        if (!isBossActive)
        {
            isBossActive = true;

            // Get a random boss from the pool
            GameObject selectedBossPrefab = bossPool.GetRandomBoss();
            if (selectedBossPrefab == null) return;

            // Spawn the boss prefab (you can adjust position as necessary)
            bossObject = Instantiate(bossPrefab, new Vector3(0, 0, 0), Quaternion.identity);

            // Additional setup for the boss (health, behavior, etc.) can be done here
            SetupBoss(bossObject);

            Debug.Log("Boss activated: " + CurrentBoss.Type);
        }
    }

    // Revert the game back to normal (removes the boss and resets the state)
    public void RevertToNormal()
    {
        if (isBossActive)
        {
            isBossActive = false;

            // Destroy the boss object from the scene
            Destroy(bossObject);

            // Additional clean-up logic can be added here (e.g., resetting game difficulty, etc.)
        }
    }

    // Set up the boss (e.g., health, special abilities, etc.)
    private void SetupBoss(GameObject boss)
    {
        // Example: You can assign health, behavior, or other properties here
        Boss bossScript = boss.GetComponent<Boss>();
        if (bossScript != null)
        {
            bossScript.Initialize(); // Initialize the boss's properties
        }
    }

    // Call when boss is defeated
    public void ClearBoss()
    {
        CurrentBoss = null;
    }
}