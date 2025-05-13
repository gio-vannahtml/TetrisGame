using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridScript : MonoBehaviour
{
    // 2D array representing the game grid - stores references to block Transforms
    public Transform[,] grid;
    
    // Grid dimensions
    public int width, height;
    // Start is called before the first frame update
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
    
    public int CheckForLines()
    {
        int linesCleared = 0;

        for (int y = 0; y < height; y++)
        {
            if (LineIsFull(y))
            {
                DeleteLine(y);
                DecreaseRowsAbove(y + 1);
                y--;
                linesCleared++;
            }
        }

        return linesCleared;
    }

    // Checks if a row is completely filled with blocks
    bool LineIsFull(int y)
    {
        for (int x = 0; x < width; x++)
        {
            if (grid[x, y] == null)
            {
                return false;
            }
        }
        return true;
    }

    // Removes all blocks in a completed row
    void DeleteLine(int y)
    {
        for (int x = 0; x < width; x++)
        {
            Destroy(grid[x, y].gameObject);
            grid[x, y] = null;
        }
    }

    // Moves blocks down to fill in cleared rows
    void DecreaseRowsAbove(int startRow)
    {
        for (int y = startRow; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                if (grid[x, y] != null)
                {
                    grid[x, y - 1] = grid[x, y];
                    grid[x, y] = null;
                    grid[x, y - 1].position += Vector3.down;
                }
            }
        }
    }
}
