using UnityEngine;

/// Modifier represents a gameplay-altering effect that can be applied during a run.
/// Examples might include increased block fall speed, score multipliers, or visual changes.
public class Modifier
{
    // The name of the modifier (e.g., "Speed Boost", "Double Score")
    public string name;

    // A description explaining what the modifier does
    public string description;

    // The level of the modifier, which can be used to scale its effect
    public int level;

    /// Applies the modifier’s effect to the game.
    /// This would typically interact with gameplay systems like speed, spawn rate, etc.
    public void Apply()
    {
        // Implement logic to enable the modifier’s effect
    }


    /// Removes the modifier’s effect from the game.
    /// This restores the game to its previous state before the modifier was applied.
    public void Remove()
    {
        // Implement logic to disable or undo the modifier’s effect
    }
}