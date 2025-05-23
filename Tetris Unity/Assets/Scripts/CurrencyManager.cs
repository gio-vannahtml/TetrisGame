using UnityEngine;

// Manages the in-game currency system for the roguelike run
// Handles currency initialization, addition, and spending
public class CurrencyManager : MonoBehaviour
{
    // Singleton instance
    public static CurrencyManager Instance { get; private set; }

    // Current amount of currency the player has in this run
    public int currency;
    public int combos;

    public event System.Action OnCurrencyChanged;


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
        combos = 0;
        Debug.Log("Currency reset to: " + currency);
    }

    public void SetCombos(int value)
    {
        combos = value;
        OnCurrencyChanged?.Invoke(); // Notify UI listeners
    }
    
    // Resets combos (e.g., on fail or level reset)
    public void ResetCombos()
    {
        combos = 0;
        Debug.Log("Combos reset.");
        OnCurrencyChanged?.Invoke();
    }

    // Adds the specified amount to the player's currency
    public void AddCurrency(int amount)
    {
        currency += amount;
        Debug.Log("Currency added: +" + amount + " (Total: " + currency + ")");
        OnCurrencyChanged?.Invoke(); // Notify listeners
    }

    // Adds a combo (can customize to increase by more than 1 if needed)
    public void AddCombo(int amount)
    {
        combos += amount;
        Debug.Log("Combo added: +" + amount + " (Total: " + combos + ")");
        OnCurrencyChanged?.Invoke(); // Reuse event if UI is listening
    }

    // Optional: Save and load combos
    public void SaveCombos()
    {
        PlayerPrefs.SetInt("Combos", combos);
        PlayerPrefs.Save();
    }

    public void LoadCombos()
    {
        combos = PlayerPrefs.GetInt("Combos", 0);
        Debug.Log("Combos loaded: " + combos);
    }

    // Deducts the specified amount from the player's currency
    public void SpendCurrency(int amount)
    {
        currency -= amount;
        Debug.Log("Currency spent: -" + amount + " (Total: " + currency + ")");
        OnCurrencyChanged?.Invoke(); // Notify listeners
    }

    public void OnLevelComplete(int pointsEarned)
    {
        Instance.AddCurrency(pointsEarned);
        // Load next level or show win screen
    }
        
    public void SaveCurrency()
    {
        PlayerPrefs.SetInt("Currency", currency);
        PlayerPrefs.Save();
    }

    public void LoadCurrency()
    {
        currency = PlayerPrefs.GetInt("Currency", 0);
        Debug.Log("Currency loaded: " + currency);
    }


}