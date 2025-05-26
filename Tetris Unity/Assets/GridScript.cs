using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridScript : MonoBehaviour
{
    public Sprite tractorEffectSprite; 
    public Sprite crusherEffectSprite;
    public GameObject tractorEffectPrefab;
    public Sprite bombSprite;
    public Transform[,] grid;

    public int width = 10, height = 20;

    public GameObject blockDestroyEffectPrefab;
    public GameManager gameManager;

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
                mino.position = new Vector3(Mathf.Round(pos.x), Mathf.Round(pos.y), 0);
                grid[(int)pos.x, (int)pos.y] = mino;
            }
        }
    }

    public void UpdateGridWithBoss(Transform boss)
    {
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
            if (!IsInsideBorder(pos) || 
                (GetTransformAtGridPosition(pos) != null && GetTransformAtGridPosition(pos).parent != tetromino))
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
                linesCleared++;
                DecreaseRowsAbove(y + 1);
                y--;
            }
        }
        return linesCleared;
    }

    bool LineIsFull(int y)
    {
        for (int x = 0; x < width; x++)
        {
            if (grid[x, y] == null)
                return false;
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

                GameObject effect = Instantiate(blockDestroyEffectPrefab, spawnPos, Quaternion.identity);
                effect.transform.localScale = Vector3.one * 1.5f;

                ParticleSystem ps = effect.GetComponent<ParticleSystem>();
                if (ps != null) ps.Play();

                grid[x, y] = null;
                Destroy(effect, 1f);
                Destroy(block.gameObject, 0.01f);
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

    public void UseBombastic()
    {
        Debug.Log("UseBombastic called");
        if (GameManager.Instance != null)
        {
            GameManager.Instance.SetNextPieceToBomb();
        }
        else
        {
            Debug.LogError("GameManager not found!");
        }
    }

    public void UseCrusher()
    {
        StartCoroutine(PlayCrusherEffectAndCompress());
    }

    private IEnumerator PlayCrusherEffectAndCompress()
    {
        float delay = 0.05f;

        List<Transform> blocksToCrush = new List<Transform>();
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                if (grid[x, y] != null)
                    blocksToCrush.Add(grid[x, y]);
            }
        }

        /*TODO: Saved for later, use sprites to show the crusher effect
        foreach (Transform block in blocksToCrush)
        {
            SpriteRenderer sr = block.GetComponent<SpriteRenderer>();
            if (sr != null)
                sr.sprite = crusherEffectSprite;
        }

        yield return new WaitForSeconds(0.5f);

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
        } */

        yield return new WaitForSeconds(delay);

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
                    sr.sprite = tractorEffectSprite;

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

    public void UseColorPopper()
    {
        // Define possible color tags
        string[] colorTags = new string[] { "RedColored", "BlueColored", "GreenColored", "YellowColored", "PurpleColored", "PinkColored", "OrangeColored" };
        
        // Find colors that are actually present on the grid
        List<string> availableColors = new List<string>();
        foreach (string tag in colorTags)
        {
            // Check if at least one block of this color exists in our grid
            bool colorExists = false;
            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    if (grid[x, y] != null && grid[x, y].CompareTag(tag))
                    {
                        colorExists = true;
                        availableColors.Add(tag);
                        break;
                    }
                }
                if (colorExists) break;
            }
        }
        
        // If no colors are found, return early
        if (availableColors.Count == 0)
        {
            Debug.Log("Color Popper: No colored blocks found on the grid");
            return;
        }
        
        // Select a random tag from the available colors
        string selectedTag = availableColors[Random.Range(0, availableColors.Count)];
        
        // Find all game objects with the selected tag (for debugging/verification)
        GameObject[] objectsWithTag = GameObject.FindGameObjectsWithTag(selectedTag);
        
        Debug.Log($"Color Popper removing blocks with tag: {selectedTag} (Found {objectsWithTag.Length} blocks)");
        
        // Loop through the grid and destroy blocks with the selected color tag
        int blocksDestroyed = 0;
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                if (grid[x, y] != null && grid[x, y].CompareTag(selectedTag))
                {
                    // Spawn particle effect before destroying (if you want to show effects)
                    if (blockDestroyEffectPrefab != null)
                    {
                        GameObject effect = Instantiate(blockDestroyEffectPrefab, grid[x, y].position, Quaternion.identity);
                        Destroy(effect, 1f);
                    }
                    
                    Destroy(grid[x, y].gameObject);
                    grid[x, y] = null;
                    blocksDestroyed++;
                }
            }
        }
        
        Debug.Log($"Color Popper destroyed {blocksDestroyed} blocks with tag: {selectedTag}");

        // We need to start from the bottom and work our way up
        /* for (int y = 0; y < height; y++)
        {
            Debug.Log($"Checking row {y}");
            DecreaseRowsAbove(y);
        } */
            
        // Make the grid fall down after destroying blocks
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
            
            // Handle any orphaned blocks (blocks without parents) like in the original method
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
            
            Debug.Log("Grid has been reorganized after color popping, preserving tetromino shapes");
        }
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1)) UseBombastic();
        if (Input.GetKeyDown(KeyCode.Alpha2)) UseCrusher();
        if (Input.GetKeyDown(KeyCode.Alpha3)) UseTractor();
        if (Input.GetKeyDown(KeyCode.Alpha4)) UseColorPopper();
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
