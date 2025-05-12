using UnityEngine;

/// Modifier represents a gameplay-altering effect that can be applied during a run.
public class Modifier
{
    public string name;  // The name of the modifier (e.g., "Tractor", "Crusher")
    public string description;  // A description explaining what the modifier does
    public int level;  // The level of the modifier, which can be used to scale its effect
    public ModifierType modifierType;  // The type of modifier (e.g., Tractor, Crusher, etc.)

    public Modifier(string name, string description, int level, ModifierType modifierType)
    {
        this.name = name;
        this.description = description;
        this.level = level;
        this.modifierType = modifierType;
    }

    public class Block
    {
        public Color color;  // Each block has a color, you can expand this with other properties like type, position, etc.
        public Vector2 position;  // Position of the block on the grid (optional)

        // Constructor for initializing a block
        public Block(Color blockColor, Vector2 blockPosition)
        {
            color = blockColor;
            position = blockPosition;
        }
    }


    // Apply the modifier’s effect to the game.
    public void Apply(Block[,] blockGrid, int gridWidth, int gridHeight)
    {
        switch (modifierType)
        {
            case ModifierType.Tractor:
                ApplyTractorEffect(blockGrid, gridWidth, gridHeight);
                break;
            case ModifierType.Crusher:
                ApplyCrusherEffect(blockGrid, gridWidth, gridHeight);
                break;
            case ModifierType.Bomb:
                ApplyBombEffect(blockGrid, gridWidth, gridHeight);
                break;
            case ModifierType.ColorPopper:
                ApplyColorPopperEffect(blockGrid, gridWidth, gridHeight);
                break;
        }
    }

// ----- Modifier Effects -----
// Modifier Effects: Tractor, Crusher, Bomb, ColorPopper
    private void ApplyTractorEffect(Block[,] blockGrid, int gridWidth, int gridHeight)
    {
        // Remove blocks in specific rows (e.g., based on modifier level)
        int rowToRemove = level - 1; 
        if (rowToRemove >= 0 && rowToRemove < gridHeight)
        {
            for (int x = 0; x < gridWidth; x++)
            {
                blockGrid[x, rowToRemove] = null; // Remove the block at (x, rowToRemove)
            }
        }
    }

    private void ApplyCrusherEffect(Block[,] blockGrid, int gridWidth, int gridHeight)
    {
        // Remove blocks in specific columns (e.g., based on modifier level)
        int colToRemove = level - 1;
        if (colToRemove >= 0 && colToRemove < gridWidth)
        {
            for (int y = 0; y < gridHeight; y++)
            {
                blockGrid[colToRemove, y] = null; // Remove the block at (colToRemove, y)
            }
        }
    }

    private void ApplyBombEffect(Block[,] blockGrid, int gridWidth, int gridHeight)
    {
        // Remove blocks around the bomb (e.g., in a 3x3 area centered on a specific location)
        int bombX = gridWidth / 2;
        int bombY = gridHeight / 2;
        for (int x = bombX - 1; x <= bombX + 1; x++)
        {
            for (int y = bombY - 1; y <= bombY + 1; y++)
            {
                if (x >= 0 && x < gridWidth && y >= 0 && y < gridHeight)
                {
                    blockGrid[x, y] = null; // Remove the block at (x, y)
                }
            }
        }
    }

    private void ApplyColorPopperEffect(Block[,] blockGrid, int gridWidth, int gridHeight)
    {
        // Remove blocks that match the color of the color popper
        Color colorToRemove = GetColorForPopperEffect(); 
        for (int x = 0; x < gridWidth; x++)
        {
            for (int y = 0; y < gridHeight; y++)
            {
                if (blockGrid[x, y] != null && blockGrid[x, y].color == colorToRemove)
                {
                    blockGrid[x, y] = null; // Remove the block that matches the color
                }
            }
        }
    }

    private Color GetColorForPopperEffect()
    {
        // Example: return a random color for the popper
        return Color.red; // Replace with actual logic to determine color
    }
}

// Enum to categorize the modifier types (e.g., Tractor, Crusher, Bomb, ColorPopper)
public enum ModifierType
{
    Tractor,
    Crusher,
    Bomb,
    ColorPopper
}