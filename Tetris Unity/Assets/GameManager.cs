using UnityEngine;

/// Main controller for the Tetris game, handling tetromino spawning, movement, game logic, and interacting with the global run manager
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

    // Initialize the game by spawning the first tetromino
    void Start()
    {
        SpawnTetromino();
    }

    // Called every frame - handles automatic downward movement and user input
    void Update()
    {
        // Start the game when the player clicks on the grid
        if (!GlobalGameManager.Instance.IsGameRunning() && Input.GetMouseButtonDown(0))
        {
            // Start the game when the player clicks the grid
            GlobalGameManager.Instance.StartRun();
            SpawnTetromino();
        }
        UserInput();
    }

    // Handle keyboard input for tetromino movement and rotation
    void UserInput()
    {
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            MoveTetromino(Vector3.left);
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            MoveTetromino(Vector3.right);
        }
        else if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            // Rotate the tetromino and revert if the rotation creates an invalid position
            currentTetromino.transform.Rotate(0, 0, 90);
            if (!IsValidPosition())
            {
                currentTetromino.transform.Rotate(0, 0, -90);
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
            while (MoveTetromino(Vector3.down))
            {
                continue;
            }
        }
    }

    // Create a new random tetromino at the top of the grid
    void SpawnTetromino()
    {
        int index = Random.Range(0, Tetrominos.Length);
        currentTetromino = Instantiate(Tetrominos[index], new Vector3(3, 18, 0), Quaternion.identity);
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
                int linesCleared = GetComponent<GridScript>().CheckForLines();  // Now returns an int
                int pointsGained = CalculatePoints(linesCleared);
                runManager.TrackProgress(pointsGained);  // Update score in RunManager
                playerScore += pointsGained;  // Update local score
                SpawnTetromino();  // Spawn the next tetromino
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

    // Calculate points based on lines cleared
    int CalculatePoints(int linesCleared)
    {
        int points = 0;
        switch (linesCleared)
        {
            case 1:
                points = 100;
                break;
            case 2:
                points = 300;
                break;
            case 3:
                points = 500;
                break;
            case 4:
                points = 800;
                break;
            default:
                points = 0;
                break;
        }
        Debug.Log($"Lines cleared: {linesCleared}, Points: {points}");
        return points;
    }

    // Check for completed lines and clear them
    void CheckForLines()
    {
        GetComponent<GridScript>().CheckForLines();
    }
}
