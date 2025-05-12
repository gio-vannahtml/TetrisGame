using UnityEngine;

/// RunManager handles the logic for a single gameplay session (a "run").
/// It manages active gameplay modifiers, difficulty scaling, score tracking,
/// and transitions between game phases like gameplay and shop.
public class RunManager
{
    // An array of gameplay modifiers that affect the current run (e.g., faster speed, harder blocks)
    public Modifier[] activeModifiers;

    /// Applies the effects of all active modifiers to the current run.
    /// This might affect gameplay mechanics, visual effects, etc.
    public void ApplyModifier()
    {
        // Loop through activeModifiers and apply their effects to the game state
    }

    /// Resets the run to its starting state.
    /// Called at the beginning of a new game session.
    public void ResetRun()
    {
        // Clear score, reset modifiers, restart timers, etc.
    }

    /// Dynamically increases difficulty based on time, score, or other metrics.
    /// Helps to scale challenge as the player progresses.
    public void UpdateDifficulty()
    {
        // Adjust speed, spawn rates, etc.
    }

    /// Tracks and stores progress metrics during the run,
    /// such as score, cleared lines, or achievements.
 
    /// <param name="scoreAchieved">The amount of score gained or total score to record.</param>
    public void TrackProgress(int scoreAchieved)
    {
        // Update current score, statistics, or achievements
        GlobalGameManager.Instance.Meta.CheckAllGoals(scoreAchieved);

        // You can add more logic here like updating UI, adjusting difficulty, etc.
    }

    /// Transitions the game from the current run phase to a shop or upgrade screen.
    /// Useful in games with roguelike mechanics or progression between levels.
    public void TransitionToShop()
    {
        // Handle scene switch or UI change to the shop screen
    }
}