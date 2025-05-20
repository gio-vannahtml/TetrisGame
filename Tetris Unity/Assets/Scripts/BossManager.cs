using UnityEngine;

public class BossManager : MonoBehaviour
{
    // Singleton pattern for easy access across the game
    public static BossManager Instance;

    // Reference to the boss object (you can replace this with a class that represents the boss)
    private GameObject bossObject;

    // The boss prefab to spawn (you'll need to assign this in the Unity inspector)
    public GameObject bossPrefab;

    public BossPool bossPool;

    public Boss CurrentBoss { get; private set; } // Expose current boss

    // Tracks whether the boss is currently active in the game
    public bool IsBossActive => CurrentBoss != null;

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
        Debug.Log("Applying boss...");
        if (!IsBossActive)
        {
            Debug.Log("Boss is NOT active " + IsBossActive);

            // Get a random boss from the pool
            if (bossPool == null)
            {
                Debug.LogError("BossPool is not assigned!");
                return;
            }
            GameObject selectedBossPrefab = bossPool.GetRandomBoss();
            
            if (selectedBossPrefab == null) return;

            // Get spawn position from the boss prefab itself
            Vector3 bossPosition = selectedBossPrefab.GetComponent<Boss>()?.SpawnPosition ?? Vector3.zero;
            
            // Spawn the boss prefab using its own spawn position
            bossObject = Instantiate(selectedBossPrefab, bossPosition, Quaternion.identity);
            Debug.Log("Boss instantiated at " + bossPosition);

            // Set up the boss
            Boss bossComponent = SetupBoss(bossObject);
            CurrentBoss = bossComponent; // Set the current boss reference

            Debug.Log("Boss activated");
        }
    }

    // Revert the game back to normal (removes the boss and resets the state)
    public void RevertToNormal()
    {
        if (IsBossActive)
        {
            // Destroy the boss object from the scene
            Destroy(bossObject);

            // Additional clean-up logic can be added here (e.g., resetting game difficulty, etc.)
        }
    }

    // Set up the boss (e.g., health, special abilities, etc.)
    private Boss SetupBoss(GameObject boss)
    {
        // Example: You can assign health, behavior, or other properties here
        Debug.Log("Setting up boss...");
        Boss bossScript = boss.GetComponent<Boss>();
        if (bossScript == null)
        {
            Debug.LogError("Boss component not found on the boss GameObject!");
            return null;
        }

        bossScript.Initialize(); // Initialize the boss's properties
        Debug.Log("Boss properties initialized from script");
        
        return bossScript;
    }

    // Call when boss is defeated
    public void ClearBoss()
    {
        CurrentBoss = null;
    }
}