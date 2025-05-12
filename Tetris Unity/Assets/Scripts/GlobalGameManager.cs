using UnityEngine;

/// GlobalGameManager is the core controller for the game flow.
/// It manages overall game state (start, pause, end), and holds references
/// to other key systems like RunManager and MetaprogressionManager.

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
    }

    /// Ends the current run. Useful for triggering game over.
    public void EndRun() => isRunning = false;

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
}