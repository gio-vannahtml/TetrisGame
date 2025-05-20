using UnityEngine;

public class Boss : MonoBehaviour
{
    public BossType Type { get; set; }

    // Add spawn position data to the Boss
    [SerializeField] private Vector3 spawnPosition = new Vector3(5, 0, 0); // Default above the play area
    public Vector3 SpawnPosition => spawnPosition; // Accessor property

    // Remove this constructor - it causes issues with MonoBehaviour
    public Boss(BossType type)
    {
        Type = type;
    }

    [SerializeField] private BossType bossType; // Inspector-configurable
    
    public enum BossType
    {
        SpeedUp,
        BlockSpawn,
        Freeze,
        // Add more boss types if needed
    }

    public int health = 100; // Example health for the boss

    // Add this property
    public bool IsLocked { get; private set; } = false;
    
    // Method to lock the boss
    public void Lock()
    {
        IsLocked = true;
        Debug.Log("Boss has been locked!");
        // You can trigger special effects or behaviors when locked
    }
    
    // Method to unlock the boss if needed
    public void Unlock()
    {
        IsLocked = false;
    }
    
    // Start is called to set initial values
    private void Start()
    {
        Type = bossType; // Set the property from the inspector value
    }

    // Initialize the boss (could set up health, abilities, etc.)
    public void Initialize()
    {
        // Example initialization code remains the same
        health = 100;
        Debug.Log("Boss Initialized with " + health + " health.");
    }

    // Call this to damage the boss
    public void TakeDamage(int amount)
    {
        
        health -= amount;
        Debug.Log("Boss damaged by " + amount + ", health is currently: " + health);
        if (health <= 0)
        {
            Die();
        }
    }

    // Handle boss death
    private void Die()
    {
        // Play death animation, handle rewards, etc.
        Debug.Log("Boss has been defeated!");
        BossManager.Instance.RevertToNormal();  // Remove the boss once defeated
    }
}