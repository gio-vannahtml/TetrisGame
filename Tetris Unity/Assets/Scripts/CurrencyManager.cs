using UnityEngine;

// Manages the in-game currency system for the roguelike run
// Handles currency initialization, addition, and spending
public class CurrencyManager : MonoBehaviour
{
    // Singleton instance
    public static CurrencyManager Instance { get; private set; }

    // Current amount of currency the player has in this run
    public int currency;

    private void Awake()
    {
        // Singleton pattern setup
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Keep the currency manager between scenes
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // Called when the GameObject is first enabled
    // Initializes the currency for a new run
    void Start()
    {
        // Initialize currency for a new run
        ResetRun();
    }

    // Resets the currency to 0 for a new roguelike run
    public void ResetRun()
    {
        currency = 0;
        Debug.Log("Currency reset to: " + currency);
    }

    // Adds the specified amount to the player's currency
    public void AddCurrency(int amount)
    {
        currency += amount;
        Debug.Log("Currency added: +" + amount + " (Total: " + currency + ")");
    }

    // Deducts the specified amount from the player's currency
    public void SpendCurrency(int amount)
    {
        currency -= amount;
        Debug.Log("Currency spent: -" + amount + " (Total: " + currency + ")");
    }
}