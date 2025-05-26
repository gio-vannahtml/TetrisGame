using System.Collections.Generic;
using UnityEngine;

//TODO: 
/// MetaprogressionManager handles persistent player progression,
/// such as unlocked features, goals, and saved game data.
/// It is separate from in-run logic and tracks long-term progress.
public class MetaprogressionManager
{
    // Stores serialized player progress data (could be JSON, XML, etc.)
    private string savedProgress;

    /// Unlocks a new feature or goal based on player achievement.
    /// This could update the savedProgress or trigger new content.

    // A list of all goals the player can achieve
    public List<Goal> goals = new List<Goal>();

    /// Called by a goal when its progress is completed.
    /// This method could trigger unlockables, UI updates, or saving.
    public void UnlockFeature(Goal goal)
    {
        Debug.Log($"Goal Unlocked: {goal.name}");
        // TODO: Add unlock logic (e.g., enable new game modes, pieces, UI feedback)
        SaveProgress(); // Save unlock state
    }

    /// Saves the player's metaprogression data to persistent storage.
    public void SaveProgress()
    {
        // TODO: Implement actual save logic (e.g., PlayerPrefs or file IO)
        Debug.Log("Progress Saved.");
    }

    /// Loads previously saved metaprogression data.
    public void LoadProgress()
    {
        // TODO: Load saved state from storage
        Debug.Log("Progress Loaded.");
    }

    /// Check all goals against the player's current progress.
    /// Useful after a game or at checkpoints.
    public void CheckAllGoals(int playerProgress)
    {
        foreach (Goal goal in goals)
        {
            goal.CheckProgress(playerProgress, this);
        }
    }
}