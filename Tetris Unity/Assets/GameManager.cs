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
    
    // Piece pool system
    private List<GameObject> piecePool = new List<GameObject>();
    private const int POOL_SIZE = 5;

    // Initialize the game by spawning the first tetromino
    void Start()
    {
        remainingMoves = maxMoves;
        InitializePiecePool();
        SpawnTetromino();
        UpdateMoveText();
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
        if (passedTime >= movementFrequency)
        {
            passedTime -= movementFrequency;
            MoveTetromino(Vector3.down);
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
        if (piecePool.Count == 0)
        {
            InitializePiecePool();
        }

        // Get the first piece from the pool
        currentTetromino = Instantiate(piecePool[0], new Vector3(3, 18, 0), Quaternion.identity);
        
        // Remove the used piece and add a new one
        piecePool.RemoveAt(0);
        piecePool.Add(Tetrominos[Random.Range(0, Tetrominos.Length)]);
        
        // Update the preview
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

                // Play brick landing sound
            SoundManager.Instance.PlayBrickSound();

                CheckForLines();
                SpawnTetromino();
            }
            return false;
        }
        return true;
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

    // Indicate that the game has ended when there are no moves remaining
    void EndGame()
    {
        Debug.Log("Out of moves! Game Over...");
        // Disable input or show game over UI
        enabled = false; // Disables this script
    }
}