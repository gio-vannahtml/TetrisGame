using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Manages the Tetris game grid, handling block placement, position validation, and line clearing
public class GridScript : MonoBehaviour
{
    // 2D array representing the game grid - stores references to block Transforms
    public Transform[,] grid;
    
    // Grid dimensions
    public int width = 10, height = 20; // Set default grid size

    // Initialize the grid with specified dimensions
    void Start()
    {
        grid = new Transform[width, height];
    }

    // Updates the grid data when a Tetromino moves to a new position
    public void UpdateGrid(Transform tetromino)
    {
        // First clear any grid positions that were previously occupied by this tetromino
        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                if (grid[x, y] != null)
                {
                    if (grid[x, y].parent == tetromino)
                    {
                        grid[x, y] = null;
                    }
                }
            }
        }

        // Then register each block of the tetromino at its new position in the grid
        foreach (Transform mino in tetromino)
        {
            Vector2 pos = Round(mino.position);
            if (pos.y < height)
            {
                grid[(int)pos.x, (int)pos.y] = mino;
            }
        }
    }

    // Converts world coordinates to grid coordinates by rounding to nearest integer
    public static Vector2 Round(Vector2 v)
    {
        return new Vector2(Mathf.Round(v.x), Mathf.Round(v.y));
    }

    // Checks if the given position is within the grid boundaries
    public bool IsInsideBorder(Vector2 pos)
    {
        return (int)pos.x >= 0 && (int)pos.x < width && (int)pos.y >= 0 && (int)pos.y < height;
    }

    // Gets the block (if any) at a specific grid position
    public Transform GetTransformAtGridPosition(Vector2 pos)
    {
        if (pos.y > height - 1)
        {
            return null;
        }
        return grid[(int)pos.x, (int)pos.y];
    }

    // Checks if a tetromino can be placed at its current position
    // Returns true if position is valid (no collisions or out-of-bounds)
    public bool IsValidPosition(Transform tetromino)
    {
        foreach (Transform mino in tetromino)
        {
            Vector2 pos = Round(mino.position);
            // Check if block is inside the grid
            if (!IsInsideBorder(pos))
            {
                return false;
            }

            // Check if block collides with another tetromino
            if (GetTransformAtGridPosition(pos) != null && GetTransformAtGridPosition(pos).parent != tetromino)
            {
                return false;
            }
        }
        return true;
    }

    // Checks for completed lines and removes them
    public int CheckForLines()
    {
        int linesCleared = 0;

        // Loop through all rows in the grid and check if they are full
        for (int y = 0; y < height; y++)
        {
            if (LineIsFull(y))  // Check if a line is full
            {
                linesCleared++;
                ClearLine(y);  // Clear the line
                DecreaseRowsAbove(y);  // Shift lines down after clearing
            }
        }

        return linesCleared;
    }

    // Checks if a row is completely filled with blocks
    bool LineIsFull(int y)
    {
        for (int x = 0; x < width; x++)
        {
            if (grid[x, y] == null)  // If any cell in the row is empty
            {
                return false;
            }
        }
        return true;
    }

    // Removes all blocks in a completed row
    void ClearLine(int y)
    {
        for (int x = 0; x < width; x++)
        {
            Destroy(grid[x, y].gameObject);  // Destroy the block GameObject
            grid[x, y] = null;  // Set the grid cell to null
        }
    }

    // Moves blocks down to fill in cleared rows
    void DecreaseRowsAbove(int startRow)
    {
        for (int y = startRow; y < height - 1; y++)  // Iterate from the cleared row to the top
        {
            for (int x = 0; x < width; x++)
            {
                if (grid[x, y + 1] != null)
                {
                    grid[x, y] = grid[x, y + 1];  // Move the block down
                    grid[x, y + 1] = null;  // Set the original position to null
                    grid[x, y].position += Vector3.down;  // Move the GameObject down
                }
            }
        }
    }
}