using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridScript : MonoBehaviour
{
    public Transform[,] grid;
    
    // Grid dimensions
    public int width = 10, height = 20; // Set default grid size

    public GameObject blockDestroyEffectPrefab; // Drag prefab in Inspector


    // Initialize the grid with specified dimensions
    void Start()
    {
        grid = new Transform[width, height];
    }

    public void UpdateGrid(Transform tetromino)
    {
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

        foreach (Transform mino in tetromino)
        {
            Vector2 pos = Round(mino.position);
            if (pos.y < height)
            {
                grid[(int)pos.x, (int)pos.y] = mino;
            }
        }
    }

    public void UpdateGridWithBoss(Transform boss)
    {
        // Clear the grid of blocks that belong to the boss
        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                if (grid[x, y] != null && grid[x, y].parent == boss)
                {
                    grid[x, y] = null;
                }
            }
        }

        Debug.Log("Locking the boss...");
        foreach (Transform mino in boss)
        {
            Vector2 pos = Round(mino.position);
            if (pos.y < height)
            {
                grid[(int)pos.x, (int)pos.y] = mino;
            }
        }
        
        // Get the Boss component and set it as locked
        Boss bossComponent = boss.GetComponent<Boss>();
        if (bossComponent != null)
        {
            bossComponent.Lock();
        }
    }

    public static Vector2 Round(Vector2 v)
    {
        return new Vector2(Mathf.Round(v.x), Mathf.Round(v.y));
    }

    public bool IsInsideBorder(Vector2 pos)
    {
        return (int)pos.x >= 0 && (int)pos.x < width && (int)pos.y >= 0 && (int)pos.y < height;
    }

    public Transform GetTransformAtGridPosition(Vector2 pos)
    {
        if (pos.y > height - 1)
        {
            return null;
        }
        return grid[(int)pos.x, (int)pos.y];
    }

    public bool IsValidPosition(Transform tetromino)
    {
        foreach (Transform mino in tetromino)
        {
            Vector2 pos = Round(mino.position);
            if (!IsInsideBorder(pos))
            {
                return false;
            }

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
                DeleteLine(y); // Clear the line
                linesCleared++;
                DecreaseRowsAbove(y + 1); // Shift lines down after clearing
                y--;
                
            }
        }

        return linesCleared;
    }

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

    void DeleteLine(int y)
    {
        for (int x = 0; x < width; x++)
        {
            if (grid[x, y] != null)
            {
                Transform block = grid[x, y];
                Vector3 spawnPos = block.position;

                // Spawn particle effect exactly where the block was
                GameObject effect = Instantiate(blockDestroyEffectPrefab, spawnPos, Quaternion.identity);
                effect.transform.localScale = Vector3.one * 1.5f; // Scale if needed

                ParticleSystem ps = effect.GetComponent<ParticleSystem>();
                if (ps != null) ps.Play();

                // Detach particle effect from block and delay block destruction slightly to sync visuals
                grid[x, y] = null; // Clear grid reference
                Destroy(effect, 1f);
                Destroy(block.gameObject, 0.01f); // Slight delay so particles aren't cut off
            }

        }
    }

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

    /* Giovanna:
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
     */
}
