using UnityEngine;

/// Represents a long-term or short-term goal for the player to achieve,
/// such as reaching a score, surviving a number of rounds, or clearing a number of lines.
/// Used for metaprogression and unlockable features.
public class Goal
{
    // The name of the goal (e.g., "Line Master", "Score 1000 Points")
    public string name;

    // A description of the goal, explaining what the player must do
    public string description;

    // The target value the player must reach (e.g., 1000 points, 10 lines, etc.)
    public int target;

    public bool isCompleted = false; // Track if goal has already been completed

    /// Checks if the player's current progress meets or exceeds the target.
    /// This can be used to trigger feature unlocks or rewards.

    /// <param name="currentProgress">The current value the player has reached</param>
    public void CheckProgress(int currentProgress, MetaprogressionManager metaManager)
    {
        if (!isCompleted && currentProgress >= target)
        {
            isCompleted = true;
            metaManager.UnlockFeature(this);
        }
        // Example logic (not implemented):
        // if (currentProgress >= target) { UnlockFeature(); }
    }

        
    
}