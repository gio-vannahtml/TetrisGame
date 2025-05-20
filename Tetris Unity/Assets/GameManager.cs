using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

// Main controller for the Tetris game, handling tetromino spawning, movement and game logic
public class GameManager : MonoBehaviour
{
    // Array of different tetromino prefabs
    public GameObject[] Tetrominos;
    
    // Time delay between automatic downward movements
    public float movementFrequency = 0.8f;
    
    // Timer for tracking when to move the tetromino down
    private float passedTime = 0;
    
    // Reference to the currently active tetromino
    private GameObject currentTetromino;

    // Number indicator for the players score
    public int score = 0;

    // Text for the score
    public TextMeshProUGUI scoreText;

    public int maxMoves = 100; // Based on level

    private int remainingMoves;

    public TextMeshProUGUI moveText; // UI display for moves left

    public Transform nextPiecePreviewLocation; // Set in Inspector
    private GameObject nextTetrominoPreview;   // The preview instance
    private GameObject nextTetrominoPrefab;    // The prefab we'll spawn next

    
    public static BossManager bossManager;
    public Boss CurrentBoss { get; private set; }

    // This is a property that returns true if there's currently an active boss in the game
    // It uses expression-bodied syntax to concisely check if CurrentBoss is not null
    // Allows other scripts to easily check boss status without accessing CurrentBoss directly
    //public bool IsBossActive => CurrentBoss != null;

    //public BossPool bossPool;

    private GameObject ghostTetromino;
    public Material ghostMaterial; // Assign in Inspector


    void Awake()
    {
        
            //bossManager = FindFirstObjectByType<BossManager>();
            //bossPool = FindFirstObjectByType<BossPool>();
        
    }

    public void ApplyBoss()
    {
        // Just an example
        CurrentBoss = new Boss(Boss.BossType.SpeedUp);  // set based on your logic
    }


    // Initialize the game by spawning the first tetromino
    void Start()
    {
        bossManager = BossManager.Instance;

        if (bossManager != null)
        {
            Debug.Log("BossManager.Instance exists");
            if (!bossManager.IsBossActive)
            {
                Debug.Log("No boss is active, applying boss...");
                bossManager.ApplyBoss();
            }

            /* if (bossManager.CurrentBoss != null && bossManager.CurrentBoss.Type == Boss.BossType.SpeedUp)
            {
                Debug.LogWarning("No available bosses in pool!");
                movementFrequency *= 0.5f;
            } */
        }
        else
        {
            Debug.LogWarning("BossManager.Instance is null!");
        }

        // if there is a boss, don't spawn a tetromino until the boss is unlocked
        
        remainingMoves = maxMoves;
        SpawnTetromino();
        UpdateMoveText();
    }

    // Called each frame - handles automatic downward movement and user input
    void Update()
    {
        // Track time for automatic downward movement
        passedTime += Time.deltaTime;
        
        // 🧠 Modify speed if boss is active
        if (BossManager.Instance != null && BossManager.Instance.IsBossActive)
        {
            if (BossManager.Instance.CurrentBoss.Type == Boss.BossType.SpeedUp)
            {
                movementFrequency = 0.4f; // Faster drop speed during boss
            }
            else
            {
                movementFrequency = 0.8f; // Reset default speed
            }
        }

        if (passedTime >= movementFrequency)
        {
            passedTime -= movementFrequency;

            // Check if boss is active AND not locked
            if (bossManager != null && bossManager.IsBossActive && !bossManager.IsBossLocked)
            {
                MoveBoss(Vector3.down);
            }
            else if (currentTetromino != null) // Make sure we have a tetromino to move
            {
                MoveTetromino(Vector3.down);
            }
        }
        UserInput();
        scoreText.text = "" + score.ToString();
    }

    void UpdateMoveText()
    {
        if (moveText != null)
        {
            moveText.text = "" + remainingMoves;
            Debug.Log("Updated moves text: " + remainingMoves);
        }
        else
        {
            Debug.LogWarning("movesText is NOT assigned!");
        }
    }

    // Handle keyboard input for tetromino movement and rotation
    void UserInput()
    {
        if (remainingMoves <= 0)
            return; // Prevent input if no moves left

        bool moved = false;

        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            MoveTetromino(Vector3.left);
            moved = true;
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            MoveTetromino(Vector3.right);
            moved = true;
        }
        else if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            // Rotate the tetromino and revert if the rotation creates an invalid position
            currentTetromino.transform.Rotate(0, 0, 90);
            if (!IsValidPosition())
            {
                currentTetromino.transform.Rotate(0, 0, -90);
            }
            moved = true;
        }

        if (moved)
        {
            remainingMoves--;
            UpdateMoveText();
            if (remainingMoves <= 0)
            {
                EndGame(); // Custom game-over logic
            }
            
            UpdateGhostPosition(); // 💡 update here

        }

        // Speed up movement when down arrow is pressed
        if (Input.GetKey(KeyCode.DownArrow))
        {
            movementFrequency = 0.2f;
        }
        else
        {
            movementFrequency = 0.8f;
        }

        // Hard drop tetromino
        if (Input.GetKeyDown(KeyCode.Space))
        {
            while(MoveTetromino(Vector3.down))
            {
                continue;
            }
        }
        
    }

    // Create a new random tetromino at the top of the grid
    void SpawnTetromino()
    {
        // Check if a boss is active and not locked yet
        if (bossManager != null && bossManager.IsBossActive && !bossManager.IsBossLocked)
        {
            Debug.Log("Boss is active but not locked yet. Waiting to spawn tetromino...");
            return; // Don't spawn tetromino until boss is locked
        }
        
        int index = Random.Range(0, Tetrominos.Length);
        currentTetromino = Instantiate(Tetrominos[index], new Vector3(3, 18, 0), Quaternion.identity);
        
        // Check if the spawned tetromino is in a valid position
        if (!IsValidPosition())
        {
            // Game over - the spawn area is blocked
            Debug.Log("Game Over - No space to spawn new tetromino!");
            Destroy(currentTetromino);
            currentTetromino = null;
            EndGame();
            return;
        }
        
        CreateGhost();

        // If there's no preview yet, create the first one
        if (nextTetrominoPrefab == null)
        {
            nextTetrominoPrefab = Tetrominos[Random.Range(0, Tetrominos.Length)];
            ShowNextTetrominoPreview();
        }

        // Generate the next preview piece
        nextTetrominoPrefab = Tetrominos[Random.Range(0, Tetrominos.Length)];
        ShowNextTetrominoPreview();
    }

    void ShowNextTetrominoPreview()
    {
        // Remove existing preview, if any
        if (nextTetrominoPreview != null)
        {
            Destroy(nextTetrominoPreview);
        }

        // Instantiate preview piece
        nextTetrominoPreview = Instantiate(nextTetrominoPrefab, nextPiecePreviewLocation.position, Quaternion.identity);
        nextTetrominoPreview.transform.SetParent(nextPiecePreviewLocation, true);

        // Optional: scale it down to fit in preview box
        nextTetrominoPreview.transform.localScale = Vector3.one * 0.5f;

        // Optional: disable script components or physics on preview
        foreach (var rb in nextTetrominoPreview.GetComponentsInChildren<Rigidbody2D>())
        {
            rb.simulated = false;
        }
    }

    // Move the tetromino in the specified direction if possible
    bool MoveTetromino(Vector3 direction)
    {
        currentTetromino.transform.position += direction;
        if (!IsValidPosition())
        {
            currentTetromino.transform.position -= direction;
            if (direction == Vector3.down)
            {
                // When a tetromino can't move down anymore, lock it in place and spawn a new one
                GetComponent<GridScript>().UpdateGrid(currentTetromino.transform);

                // Play brick landing sound
                SoundManager.Instance.PlayBrickSound();

                CheckForLines();
                SpawnTetromino();
            }
            return false;
        }
        UpdateGhostPosition();
        return true;
        
    }
    public void MoveBoss(Vector3 direction)
    {
        // Check if BossManager exists and has a CurrentBoss
        if (BossManager.Instance == null || BossManager.Instance.CurrentBoss == null)
        {
            Debug.LogWarning("Attempted to move boss, but no active boss found!");
            return;
        }

        // Get the current boss transform
        Transform bossTransform = BossManager.Instance.CurrentBoss.transform;
        
        // Try moving the boss
        bossTransform.position += direction;
        
        // Check if the new position is valid (using your grid validation)
        if (!GetComponent<GridScript>().IsValidPosition(bossTransform))
        {
            bossTransform.position -= direction; // Move back if invalid
            
            // If the boss tried to move down but couldn't, it's at the bottom
            if (direction == Vector3.down)
            {
                // Update the grid with the final boss position
                GetComponent<GridScript>().UpdateGridWithBoss(bossTransform);
                
                // Lock the boss
                bossManager.LockBoss();
                
                // Now that boss is locked, spawn the first tetromino
                SpawnTetromino();
            }
            
            return;
        }
        
        // Update the grid with the boss's new position
        GetComponent<GridScript>().UpdateGridWithBoss(bossTransform);
    }

    // Check if the current tetromino position is valid
    bool IsValidPosition()
    {
        return GetComponent<GridScript>().IsValidPosition(currentTetromino.transform);
    }

    // Check for completed lines and clear them
    void CheckForLines()
    {
        int lines = GetComponent<GridScript>().CheckForLines();
        
        switch (lines)
        {
            case 1:
                score += 100;
                    break;
            case 2:
                score += 300;
                    break;
            case 3:
                score += 500;
                    break;
            case 4:
                score += 800;
                if (CurrencyManager.Instance != null)
                {
                    CurrencyManager.Instance.AddCurrency(1);
                }
                else
                {
                    Debug.LogWarning("CurrencyManager instance not found! Make sure there's a GameObject with CurrencyManager component in the scene.");
                }
                break;
        }

        Debug.Log(score);
    }

    void CreateGhost()
    {
        if (ghostTetromino != null)
        {
            Destroy(ghostTetromino);
        }

        ghostTetromino = Instantiate(currentTetromino, currentTetromino.transform.position, currentTetromino.transform.rotation);
        foreach (Transform mino in ghostTetromino.transform)
        {
            var sr = mino.GetComponent<SpriteRenderer>();
            if (sr != null && ghostMaterial != null)
            {
                sr.material = ghostMaterial;
            }
        }

        // Disable collision and scripts
        foreach (Collider2D col in ghostTetromino.GetComponentsInChildren<Collider2D>())
        {
            col.enabled = false;
        }
        foreach (MonoBehaviour comp in ghostTetromino.GetComponentsInChildren<MonoBehaviour>())
        {
            comp.enabled = false;
        }

        UpdateGhostPosition();
    }

    void UpdateGhostPosition()
    {
        ghostTetromino.transform.position = currentTetromino.transform.position;
        ghostTetromino.transform.rotation = currentTetromino.transform.rotation;

        while (true)
        {
            ghostTetromino.transform.position += Vector3.down;
            if (!GetComponent<GridScript>().IsValidPosition(ghostTetromino.transform))
            {
                ghostTetromino.transform.position -= Vector3.down;
                break;
            }
        }
    }



    // Indicate that the game has ended when there are no moves remaining
    void EndGame()
    {
        Debug.Log("Game Over!");
        
        // Disable input controls
        enabled = false;
        
        // You might want to show a game over UI panel here
        // gameOverPanel.SetActive(true);
        
        // Optional: Play game over sound
        if (SoundManager.Instance != null)
        {
            // SoundManager.Instance.PlayGameOverSound();
        }
        
        // Save high score if applicable
        if (score > PlayerPrefs.GetInt("HighScore", 0))
        {
            PlayerPrefs.SetInt("HighScore", score);
            PlayerPrefs.Save();
            Debug.Log("New high score: " + score);
        }
    }
}