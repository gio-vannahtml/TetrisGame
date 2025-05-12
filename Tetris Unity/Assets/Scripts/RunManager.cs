using UnityEngine;

/// RunManager handles the logic for a single gameplay session (a "run").
/// It manages active gameplay modifiers, difficulty scaling, score tracking,
/// and transitions between game phases like gameplay and shop.
public class RunManager
{
    public int Level { get; private set; }  // Add a level property
    public int ScoreAchieved => scoreAchieved;  // Make it public for access

    // Array of gameplay modifiers that affect the current run (e.g., faster speed, harder blocks)
    public Modifier[] activeModifiers;

    // Placeholder for the grid size (replace with actual grid size or reference)
    private int gridWidth = 10;
    private int gridHeight = 20;

    // Store the player's score for the current run
    private int scoreAchieved;

    public RunManager()
    {
        Level = 1;  // Start at level 1
    }

    // Increment level as needed
    public void IncreaseLevel()
    {
        Level++;
        Debug.Log("Level increased to " + Level);
    }

    /// Applies the effects of all active modifiers to the current run.
    /// This might affect gameplay mechanics, visual effects, etc.
    public void ApplyModifier()
    {
        // Loop through activeModifiers and apply their effects to the game state
        foreach (var modifier in activeModifiers)
        {
            modifier.Apply(null, gridWidth, gridHeight);  // Pass the current block grid (null for now)
        }
    }

    /// Resets the run to its starting state.
    /// Called at the beginning of a new game session.
    public void ResetRun()
    {
        // Clear score, reset modifiers, restart timers, etc.
        scoreAchieved = 0;  // Reset the score at the start of the run
        activeModifiers = new Modifier[0];  // Clear any active modifiers
    }

    /// Dynamically increases difficulty based on time, score, or other metrics.
    /// Helps to scale challenge as the player progresses.
    public void UpdateDifficulty()
    {
        // Adjust speed, spawn rates, etc.
        // You can scale difficulty based on score, time, level, etc.
        if (scoreAchieved > 1000)
        {
            // Increase difficulty, e.g., spawn faster enemies or increase game speed
            Debug.Log("Difficulty increased!");
        }
    }

    /// Tracks and stores progress metrics during the run,
    /// such as score, cleared lines, or achievements.
    public void TrackProgress(int scoreGained)
    {
        // Update score and check achievements, etc.
        GlobalGameManager.Instance.Meta.CheckAllGoals(scoreAchieved);

        // Update the score with the points earned in this call
        scoreAchieved += scoreGained;

        // Example: Update difficulty based on progress
        UpdateDifficulty();
    }

    public void TransitionToShop()
    {
        // Example of switching between gameplay and shop, if needed
        Debug.Log("Transition to shop.");
    }

    /// Example: Adjusting difficulty based on level
    public void CheckLevelDifficulty()
    {
        if (Level > 5)
        {
            // Increase difficulty, e.g., make enemies spawn faster, etc.
            Debug.Log("Difficulty adjusted for level " + Level);
        }
    }
}