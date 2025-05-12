using UnityEngine;

public class Boss : MonoBehaviour
{
    public int health = 100; // Example health for the boss

    // Initialize the boss (could set up health, abilities, etc.)
    public void Initialize()
    {
        // Example initialization, you could set specific properties or AI here
        health = 100;
        Debug.Log("Boss Initialized with " + health + " health.");
    }

    // Call this to damage the boss
    public void TakeDamage(int amount)
    {
        health -= amount;
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