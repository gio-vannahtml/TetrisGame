using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class GridScript : MonoBehaviour

{
    public Sprite tractorEffectSprite; 
    public GameObject tractorEffectPrefab;
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

        foreach (Transform mino in boss)
        {
            Vector2 pos = Round(mino.position);
            if (pos.y < height)
            {
                grid[(int)pos.x, (int)pos.y] = mino;
            }
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
    // === Bombastic: destroy blocks in 3x3 area ===
    public void UseBombastic(Vector2 center)
    {
        int radius = 1; // 3x3 area
        Vector2Int centerInt = Vector2Int.RoundToInt(center);

        for (int dx = -radius; dx <= radius; dx++)
        {
            for (int dy = -radius; dy <= radius; dy++)
            {
                int x = centerInt.x + dx;
                int y = centerInt.y + dy;

                if (x >= 0 && x < width && y >= 0 && y < height)
                {
                    if (grid[x, y] != null)
                    {
                        Destroy(grid[x, y].gameObject);
                        grid[x, y] = null;
                    }
                }
            }
        }

        // Optional: make the grid fall down after destroying
        DecreaseRowsAbove(centerInt.y - radius);
    }
    // === Crusher: compact each column downward ===
    public void UseCrusher()
    {
        for (int x = 0; x < width; x++)
        {
            List<Transform> columnBlocks = new List<Transform>();

            for (int y = 0; y < height; y++)
            {
                if (grid[x, y] != null)
                {
                    columnBlocks.Add(grid[x, y]);
                    grid[x, y] = null;
                }
            }

            for (int y = 0; y < columnBlocks.Count; y++)
            {
                grid[x, y] = columnBlocks[y];
                grid[x, y].position = new Vector3(x, y, 0);
            }
        }
    }

    // === Tractor: removes bottom row ===
    public void UseTractor()
{
    StartCoroutine(PlayTractorEffectAndClear());
}

private IEnumerator PlayTractorEffectAndClear()
{
    float delay = 0.05f;

   for (int x = 0; x < width; x++)
{
    if (grid[x, 0] != null)
    {
        Transform block = grid[x, 0];
        SpriteRenderer sr = block.GetComponent<SpriteRenderer>();

        if (sr != null)
        {
            sr.sprite = tractorEffectSprite; // 👈 New field you'll assign
        }

        yield return new WaitForSeconds(0.2f);

        Destroy(block.gameObject);
        grid[x, 0] = null;
    }

    yield return new WaitForSeconds(delay);
}

    for (int y = 1; y < height; y++)
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

    // === Color Popper: removes all blocks of one random color ===
    public void UseColorPopper()
    {
        Dictionary<string, List<Vector2Int>> colorGroups = new Dictionary<string, List<Vector2Int>>();
        Dictionary<string, Color> colorMap = new Dictionary<string, Color>();

        // Step 1: Group blocks by color (as hex string)
        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                Transform block = grid[x, y];
                if (block != null)
                {
                    SpriteRenderer sr = block.GetComponent<SpriteRenderer>();
                    if (sr != null)
                    {
                        Color color = sr.color;
                        string hex = ColorUtility.ToHtmlStringRGB(color);

                        if (!colorGroups.ContainsKey(hex))
                            colorGroups[hex] = new List<Vector2Int>();

                        colorGroups[hex].Add(new Vector2Int(x, y));

                        if (!colorMap.ContainsKey(hex))
                            colorMap[hex] = color;
                    }
                }
            }
        }

        // Step 2: Find the color with the most blocks
        string mostCommonHex = "";
        int maxCount = 0;

        foreach (var kvp in colorGroups)
        {
            if (kvp.Value.Count > maxCount)
            {
                mostCommonHex = kvp.Key;
                maxCount = kvp.Value.Count;
            }
        }

        if (string.IsNullOrEmpty(mostCommonHex))
        {
            Debug.LogWarning("No blocks to pop!");
            return;
        }

        Color targetColor = colorMap[mostCommonHex];
        Debug.Log($"ColorPopper: Popping color {targetColor} with {maxCount} blocks!");

        // Step 3: Destroy blocks of that color
        foreach (Vector2Int pos in colorGroups[mostCommonHex])
        {
            Transform block = grid[pos.x, pos.y];
            if (block != null)
            {
                Destroy(block.gameObject);
                grid[pos.x, pos.y] = null;
            }
        }

        // Step 4: Drop blocks down
        DecreaseRowsAbove(0);
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
            UseBomb();

        if (Input.GetKeyDown(KeyCode.Alpha2))
            UseCrusher();

        if (Input.GetKeyDown(KeyCode.Alpha3))
            UseTractor();

        if (Input.GetKeyDown(KeyCode.Alpha4))
            UseColorPopper();
    }

    private void UseBomb()
    {
        throw new System.NotImplementedException();
    }
   

}
