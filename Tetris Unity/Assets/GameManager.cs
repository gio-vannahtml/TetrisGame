using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

// Main controller for the Tetris game, handling tetromino spawning, movement and game logic
public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    // Array of different tetromino prefabs
    public GameObject[] Tetrominos;
    
    // Time delay between automatic downward movements
    public float movementFrequency = 0.8f;
    
    // Timer for tracking when to move the tetromino down
    private float passedTime = 0;
    
    // Reference to the currently active tetromino
    private GameObject currentTetromino;
    //Reference to special blocks
    public GameObject luckyBlockPrefab;
    public GameObject unluckyBlockPrefab;
    public GameObject BombBlockPrefab;

    public GridScript gridScript;

    // Number indicator for the players score
    public int score = 0;

    // Text for the score
    public TextMeshProUGUI scoreText;

    public int totalLinesCleared = 0;
    public TextMeshProUGUI linesClearedText; // Assign in Inspector

    public int maxMoves = 100; // Based on level

    private int remainingMoves;

    public TextMeshProUGUI moveText; // UI display for moves left

    public Transform nextPiecePreviewLocation; // Set in Inspector
    private GameObject nextTetrominoPreview;   // The preview instance
    
    public static BossManager bossManager;
    public Boss CurrentBoss { get; private set; }

    // This is a property that returns true if there's currently an active boss in the game
    // It uses expression-bodied syntax to concisely check if CurrentBoss is not null
    // Allows other scripts to easily check boss status without accessing CurrentBoss directly
    //public bool IsBossActive => CurrentBoss != null;

    //public BossPool bossPool;

    private GameObject ghostTetromino;
    public Material ghostMaterial; // Assign in Inspector

    public GameObject winOverlay;
    public GameObject gameOverOverlay;

    void Awake()
    {
        // Singleton pattern implementation
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Optional: keeps GameManager across scene loads
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
        }
        
            //bossManager = FindFirstObjectByType<BossManager>();
            //bossPool = FindFirstObjectByType<BossPool>();
        
    }

    // Piece pool system
    private List<GameObject> piecePool = new List<GameObject>();
    private const int POOL_SIZE = 5;

    private Dictionary<string, int> sceneWinScores = new Dictionary<string, int>()
{
    { "Level - Tutorial", 500 }, 
    { "Level - Neweasy", 2000 },
    { "Level - Bosstry", 3000 }
};

    private Dictionary<string, int> sceneMoveCounts = new Dictionary<string, int>()
    {
    { "Level - Tutorial", 100 },
    { "Level - Neweasy", 50 },
    { "Level - Bosstry", 80 }
    };
    private int winScore;
    private bool hasWon = false; // To prevent triggering win multiple times

    // Initialize the game by spawning the first tetromino
    void Start()
    {
        bossManager = BossManager.Instance;

        string currentScene = SceneManager.GetActiveScene().name;
        if (sceneWinScores.ContainsKey(currentScene))
        {
            winScore = sceneWinScores[currentScene];
        }
        else
        {
            winScore = 2000; // Fallback default
        }
        if (sceneMoveCounts.ContainsKey(currentScene))
        {
            maxMoves = sceneMoveCounts[currentScene];
        }
        else
        {
            maxMoves = 100; // Fallback default
        }
        if (bossManager != null)
        {
            Debug.Log("BossManager.Instance exists");
            if (!bossManager.IsBossActive)
            {
                Debug.Log("No boss is active, applying boss...");
                bossManager.ApplyBoss();
            }
        }

        remainingMoves = maxMoves;
        InitializePiecePool();
        SpawnTetromino();
        UpdateMoveText();
        UpdateLineCounter();
        
        if (winOverlay != null) winOverlay.SetActive(false);
        if (gameOverOverlay != null) gameOverOverlay.SetActive(false);
    }

    void InitializePiecePool()
    {
        // Clear existing pool
        piecePool.Clear();
        
        // Fill the pool with random pieces
        for (int i = 0; i < POOL_SIZE; i++)
        {
            int index = Random.Range(0, Tetrominos.Length);
            piecePool.Add(Tetrominos[index]);
        }
        
        // Show initial preview
        ShowNextTetrominoPreview();
    }

    void ShowNextTetrominoPreview()
    {
        // Remove existing preview, if any
        if (nextTetrominoPreview != null)
        {
            Destroy(nextTetrominoPreview);
        }

        // Show the first piece in the pool as preview
        if (piecePool.Count > 0)
        {
            nextTetrominoPreview = Instantiate(piecePool[0], nextPiecePreviewLocation.position, Quaternion.identity);
            nextTetrominoPreview.transform.SetParent(nextPiecePreviewLocation, true);
            nextTetrominoPreview.transform.localScale = Vector3.one * 0.5f;

            // Disable physics on preview piece
            foreach (var rb in nextTetrominoPreview.GetComponentsInChildren<Rigidbody2D>())
            {
                rb.simulated = false;
            }
        }
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
        if (maxMoves != -1 && remainingMoves <= 0)
            return; // Only block input if not infinite
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

        if (moved && maxMoves != -1)
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
            while (MoveTetromino(Vector3.down))
            {
                continue;
            }
            remainingMoves--;
            UpdateMoveText();

            if (remainingMoves <= 0)
            {
                EndGame();
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

        if (piecePool.Count == 0)
        {
            InitializePiecePool();
        }

        // Get the first piece from the pool
        currentTetromino = Instantiate(piecePool[0], new Vector3(3, 18, 0), Quaternion.identity);
        
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

        float luckyChance = 0.05f;   // 5%
        float unluckyChance = 0.01f; // 1%
        float roll = Random.value;

        // Remove the used piece and add a new one
        piecePool.RemoveAt(0);
        if (roll < luckyChance)
        {
            piecePool.Add(luckyBlockPrefab);
        }
        else if (roll < luckyChance + unluckyChance)
        {
            piecePool.Add(unluckyBlockPrefab);
        }
        else
        {
            piecePool.Add(Tetrominos[Random.Range(0, Tetrominos.Length)]);
        }

        // Generate the next preview piece
        ShowNextTetrominoPreview();
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
                
                // Check if this is a bomb block using tag instead of component
                if (currentTetromino.CompareTag("Bomb"))
                {
                    // Get the position of the bomb
                    Vector2Int bombPosition = new Vector2Int(
                        Mathf.RoundToInt(currentTetromino.transform.position.x),
                        Mathf.RoundToInt(currentTetromino.transform.position.y)
                    );
                    
                    // Trigger bomb explosion directly
                    GetComponent<GridScript>().TriggerBombAt(bombPosition);
                }
                else
                {
                    // Only handle special blocks if this is not a bomb
                    HandleSpecialBlock(currentTetromino);
                }

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
    
    void HandleSpecialBlock(GameObject block)
    {
        if (block.CompareTag("Lucky"))
        {
            ActivateRandomItem(); // Immediately activate a random item
            score += 200; // Bonus Points
            remainingMoves = Mathf.Max(0, remainingMoves + 5); // Gain moves
            Debug.Log("Lucky block landed! Payday!");
        }
        else if (block.CompareTag("Unlucky"))
        {
            score -= 200; // Penalty
            remainingMoves = Mathf.Max(0, remainingMoves - 5); // Lose moves
            Debug.Log("Unlucky block landed! Budget Cuts...");
        }

        UpdateMoveText();
    }

    public void ActivateRandomItem()
    {
        int roll = Random.Range(0, 4); // 4 item types

        switch (roll)
        {
            case 0:
                gridScript.UseBombastic();
                Debug.Log("Activated Bombastic from Lucky Block!");
                break;
            case 1:
                gridScript.UseCrusher();
                Debug.Log("Activated Crusher from Lucky Block!");
                break;
            case 2:
                gridScript.UseTractor();
                Debug.Log("Activated Tractor from Lucky Block!");
                break;
            case 3:
                gridScript.UseColorPopper();
                Debug.Log("Activated ColorPopper from Lucky Block!");
                break;
        }
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
        totalLinesCleared += lines; // Count total lines
        
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

        UpdateLineCounter();
        Debug.Log(score);

        // ✅ Check win condition here
        if (!hasWon && score >= winScore)
        {
            WinGame();
        }
    }

    void UpdateLineCounter()
    {
        if (linesClearedText != null)
        {
            linesClearedText.text = "" + totalLinesCleared;
        }
        else
        {
            Debug.LogWarning("linesClearedText not assigned in the Inspector.");
        }
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

        enabled = false;

        if (SoundManager.Instance != null)
        {
            // SoundManager.Instance.PlayGameOverSound();
        }

        if (score > PlayerPrefs.GetInt("HighScore", 0))
        {
            PlayerPrefs.SetInt("HighScore", score);
            PlayerPrefs.Save();
            Debug.Log("New high score: " + score);
        }

        if (gameOverOverlay != null)
        {
            gameOverOverlay.SetActive(true);
        }
    }

    void WinGame()
    {
        hasWon = true;
        Debug.Log("You Win!");

        // Stop the game
        enabled = false; // Disable GameManager script
        if (winOverlay != null)
        {
        winOverlay.SetActive(true);
        }
    }
    public void SetNextPieceToBomb()
    {
        if (piecePool.Count > 0)
        {
            // Replace the upcoming piece (first in pool) with bomb block
            piecePool[0] = BombBlockPrefab;
            
            // Update the preview to show the bomb block
            ShowNextTetrominoPreview();
            
            Debug.Log("Next tetromino changed to Bomb Block!");
        }
        else
        {
            Debug.LogWarning("Piece pool is empty, cannot set next piece to bomb!");
        }
    }
}