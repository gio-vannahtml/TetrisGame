using UnityEngine;

/// GlobalGameManager is the core controller for the game flow.
/// It manages overall game state (start, pause, end), and holds references
/// to other key systems like RunManager, MetaprogressionManager, and BossManager.
public class GlobalGameManager : MonoBehaviour
{
    // Singleton instance for easy global access
    public static GlobalGameManager Instance;

    // Game state flags
    private bool isPaused = false;
    private bool isRunning = false;

    // Core managers for gameplay and progression
    private RunManager runManager;
    private MetaprogressionManager metaManager;

    // Public accessors to managers (read-only outside this class)
    public RunManager Run => runManager;
    public MetaprogressionManager Meta => metaManager;

    // Game state variables
    private int currentLevel = 1;  // Current level of the game

    /// Called when the object is first initialized.
    /// Ensures only one instance exists (Singleton pattern),
    /// and initializes the managers.
    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;

            // Create new instances of managers
            runManager = new RunManager();
            metaManager = new MetaprogressionManager();
        }
        else
        {
            // Prevent duplicate game managers
            Destroy(gameObject);
        }
    }

    /// Called on the first frame.
    /// Automatically starts the game run.
    void Start()
    {
        StartRun();
    }

    /// Begins a new gameplay session (run).
    /// Resets the run state and applies any modifiers.
    public void StartRun()
    {
        isRunning = true;
        isPaused = false;

        runManager.ResetRun();       // Reset the run state (score, board, etc.)
        runManager.ApplyModifier();  // Apply active gameplay modifiers

        // Example: If the level is 5, trigger the boss fight
        if (currentLevel == 5)
        {
            TriggerBossFight();
        }
    }

    /// Ends the current run. Useful for triggering game over.
    public void EndRun()
    {
        isRunning = false;
        // Optionally add game over logic here
    }

    /// Toggles the game's paused state.
    /// Pauses or resumes time flow using Time.timeScale.
    public void Pause()
    {
        isPaused = !isPaused;

        // Freezes or unfreezes the game based on pause state
        Time.timeScale = isPaused ? 0 : 1;
    }

    /// Checks if the game is currently running and not paused.
    /// Useful for gameplay systems that depend on active state.
    public bool IsGameRunning() => isRunning && !isPaused;

    // Method to calculate points based on cleared blocks
    public int CalculatePoints(int clearedBlocks)
    {
        int pointsPerBlock = 10;
        int difficultyMultiplier = 1;  // Default multiplier

        // For example: increase points based on difficulty level or other criteria
        if (runManager.Level > 5)
        {
            difficultyMultiplier = 2;  // Double points if the level is greater than 5
        }

        return clearedBlocks * pointsPerBlock * difficultyMultiplier;
    }

    // Method to be called when blocks are cleared
    public void OnBlockCleared(int clearedBlocks)
    {
        int pointsEarned = CalculatePoints(clearedBlocks);
        runManager.TrackProgress(pointsEarned);  // Update the score in the run manager

        // Example: Increase level after a certain amount of cleared blocks or score
        if (runManager.Level < 10)  // Example: Level up after each 100 blocks
        {
            runManager.IncreaseLevel();
        }

        // Check if the boss should be triggered after a level up
        if (runManager.Level == 10)  // Example: Trigger the boss after level 10
        {
            TriggerBossFight();
        }
    }

    // Trigger the boss fight (for example, after a certain level is reached)
    private void TriggerBossFight()
    {
        BossManager.Instance.ApplyBoss();  // Activate the boss
    }

    // Example: Call when a player defeats the boss
    public void OnBossDefeated()
    {
        // Proceed with normal game flow after boss defeat
        // You might move to the next level or perform other actions
        currentLevel++;
        Debug.Log("Boss Defeated! Moving to next level.");

        // Optionally, trigger a new boss fight for the next level
        if (currentLevel % 5 == 0)  // For example, every 5th level triggers a boss fight
        {
            TriggerBossFight();
        }
    }
}