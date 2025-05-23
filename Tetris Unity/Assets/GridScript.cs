using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class GridScript : MonoBehaviour

{
    public Sprite tractorEffectSprite; 
    public Sprite crusherEffectSprite;
    public GameObject tractorEffectPrefab;
    public Transform[,] grid;

    // Grid dimensions
    public int width = 10, height = 20; // Set default grid size

    public GameObject blockDestroyEffectPrefab; // Drag prefab in Inspector

    public GameManager gameManager;

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
                if (grid[x, y] != null && grid[x, y].parent == tetromino)
                {
                    grid[x, y] = null;
                }
            }
        }

        foreach (Transform mino in tetromino)
        {
            Vector2 pos = Round(mino.position);
            if (pos.y < height && pos.x >= 0 && pos.x < width)
            {
                // Snap the mino to the grid
                mino.position = new Vector3(Mathf.Round(pos.x), Mathf.Round(pos.y), 0);

                // Assign to the grid
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
    public void UseBombastic()
    {
        // Original bombastic functionality
        Debug.Log($"UseBombastic called");

        // Call the GameManager to change the upcoming tetromino to a bomb
        if (GameManager.Instance != null)
        {
            // In UseBombastic or any other method
            GameManager.Instance.SetNextPieceToBomb();
        }
        else
        {
            Debug.LogError("GameManager not found!");
        }

        // Your existing bombing functionality here...
    }


    // === Crusher: compact each column downward ===
    public void UseCrusher()
{
    StartCoroutine(PlayCrusherEffectAndCompress());
}

private IEnumerator PlayCrusherEffectAndCompress()
{
    float delay = 0.05f;

    // Step 1: Identify blocks to be crushed (i.e., removed)
    List<Transform> blocksToCrush = new List<Transform>();

    for (int x = 0; x < width; x++)
    {
        for (int y = 0; y < height; y++)
        {
            if (grid[x, y] != null)
            {
                blocksToCrush.Add(grid[x, y]);
            }
        }
    }

    // Step 2: Show crusher sprite on them for 0.2s
    foreach (Transform block in blocksToCrush)
    {
        SpriteRenderer sr = block.GetComponent<SpriteRenderer>();
        if (sr != null)
        {
            sr.sprite = crusherEffectSprite;
        }
    }

    yield return new WaitForSeconds(0.5f);

    // Step 3: Remove all blocks
    for (int x = 0; x < width; x++)
    {
        for (int y = 0; y < height; y++)
        {
            if (grid[x, y] != null)
            {
                Destroy(grid[x, y].gameObject);
                grid[x, y] = null;
            }
        }
    }

    yield return new WaitForSeconds(delay);

    // Step 4: Compress columns down
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

   for (int x = width - 1; x >= 0; x--)
{
    if (grid[x, 0] != null)
    {
        Transform block = grid[x, 0];
        SpriteRenderer sr = block.GetComponent<SpriteRenderer>();

        if (sr != null)
        {
            sr.sprite = tractorEffectSprite; 

        yield return new WaitForSeconds(0.2f);

        Destroy(block.gameObject);
        grid[x, 0] = null;
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
        {
            UseBombastic();
        }


        if (Input.GetKeyDown(KeyCode.Alpha2))
            UseCrusher();

        if (Input.GetKeyDown(KeyCode.Alpha3))
            UseTractor();

        if (Input.GetKeyDown(KeyCode.Alpha4))
            UseColorPopper();
    }

    // Method to trigger the bomb effect at a specific position
    public void TriggerBombAt(Vector2Int position)
    {
        Debug.Log($"Bomb triggered at position {position}");

        int blocksDestroyed = 0; // Keep track of blocks destroyed

        // Define the 3x3 area centered on the bomb
        for (int offsetX = -1; offsetX <= 1; offsetX++)
        {
            for (int offsetY = -1; offsetY <= 1; offsetY++)
            {
                int x = position.x + offsetX;
                int y = position.y + offsetY;

                // Check if this position is within grid bounds
                if (x >= 0 && x < width && y >= 0 && y < height)
                {
                    // If there's a block here, destroy it
                    if (grid[x, y] != null)
                    {
                        Transform block = grid[x, y];
                        blocksDestroyed++;

                        // Spawn explosion effect
                        if (blockDestroyEffectPrefab != null)
                        {
                            GameObject effect = Instantiate(blockDestroyEffectPrefab, block.position, Quaternion.identity);
                            effect.transform.localScale = Vector3.one * 1.5f;

                            ParticleSystem ps = effect.GetComponent<ParticleSystem>();
                            if (ps != null) ps.Play();

                            Destroy(effect, 1f);
                        }

                        // Destroy the block and clear grid reference
                        Destroy(block.gameObject);
                        grid[x, y] = null;
                    }
                }
            }
        }

        // After bombing, make remaining blocks fall while preserving tetromino shapes
        if (blocksDestroyed > 0)
        {
            // Find all unique tetromino parents in the grid
            HashSet<Transform> tetrominoParents = new HashSet<Transform>();
            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    if (grid[x, y] != null && grid[x, y].parent != null)
                    {
                        tetrominoParents.Add(grid[x, y].parent);
                    }
                }
            }
            
            // Process gravity in multiple passes until stable
            bool piecesMoved;
            do
            {
                piecesMoved = false;
                
                // Check each tetromino parent
                foreach (Transform parent in tetrominoParents)
                {
                    // Skip if parent has been destroyed
                    if (parent == null) continue;
                    
                    // Check if this tetromino can move down
                    bool canMoveDown = true;
                    bool hasValidChildren = false;
                    
                    // First, remove this tetromino from the grid temporarily
                    foreach (Transform child in parent)
                    {
                        Vector2 pos = Round(child.position);
                        if (IsInsideBorder(pos))
                        {
                            hasValidChildren = true;
                            grid[(int)pos.x, (int)pos.y] = null;
                        }
                    }
                    
                    // If no valid children remain (all were destroyed), skip this tetromino
                    if (!hasValidChildren) continue;
                    
                    // Check if we can move this tetromino down
                    foreach (Transform child in parent)
                    {
                        Vector2 pos = Round(child.position);
                        Vector2 posBelow = new Vector2(pos.x, pos.y - 1);
                        
                        // If any part would go out of bounds or hit another tetromino, we can't move down
                        if (!IsInsideBorder(posBelow) || 
                            (grid[(int)posBelow.x, (int)posBelow.y] != null && 
                             grid[(int)posBelow.x, (int)posBelow.y].parent != parent))
                        {
                            canMoveDown = false;
                            break;
                        }
                    }
                    
                    // Move the tetromino down if possible
                    if (canMoveDown)
                    {
                        parent.position += Vector3.down;
                        piecesMoved = true;
                    }
                    
                    // Put the tetromino back in the grid at its new position
                    foreach (Transform child in parent)
                    {
                        Vector2 pos = Round(child.position);
                        if (IsInsideBorder(pos))
                        {
                            grid[(int)pos.x, (int)pos.y] = child;
                        }
                    }
                }
                
            } while (piecesMoved);
            
            // Handle any orphaned blocks (blocks without parents)
            bool blocksMovedThisPass;
            do
            {
                blocksMovedThisPass = false;
                
                for (int y = 1; y < height; y++)
                {
                    for (int x = 0; x < width; x++)
                    {
                        // Only move blocks that don't have a parent (orphaned blocks)
                        if (grid[x, y] != null && (grid[x, y].parent == null || grid[x, y].parent.childCount == 1) && grid[x, y - 1] == null)
                        {
                            grid[x, y - 1] = grid[x, y];
                            grid[x, y] = null;
                            grid[x, y - 1].position += Vector3.down;
                            blocksMovedThisPass = true;
                        }
                    }
                }
            } while (blocksMovedThisPass);
            
            Debug.Log("Grid has been reorganized after bombing, preserving tetromino shapes");
        }
    }
}
