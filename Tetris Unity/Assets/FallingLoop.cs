using UnityEngine;

public class FallingLoop : MonoBehaviour
{
    public GameObject[] tetrominos;      // Assign prefabs in Inspector
    public float fallSpeed = 2f;         // Falling speed
    public float spawnInterval = 2f;     // Time between spawns

    private float spawnTimer;

    void Update()
    {
        spawnTimer += Time.deltaTime;

        if (spawnTimer >= spawnInterval)
        {
            spawnTimer = 0f;
            SpawnTetromino();
        }

        MoveTetrominos();
    }

    void SpawnTetromino()
    {
        if (tetrominos.Length == 0)
        {
            Debug.LogWarning("No Tetrominos assigned!");
            return;
        }

        int index = Random.Range(0, tetrominos.Length);

        // Null check in case prefab was deleted or unassigned
        GameObject prefab = tetrominos[index];
        if (prefab == null)
        {
            Debug.LogWarning($"Tetromino at index {index} is null. Check your array in the Inspector.");
            return;
        }

        float screenLeft = Camera.main.ViewportToWorldPoint(new Vector3(0, 0, 10f)).x;
        float screenRight = Camera.main.ViewportToWorldPoint(new Vector3(1, 0, 10f)).x;
        float spawnX = Random.Range(screenLeft + 1f, screenRight - 1f);
        float spawnY = Camera.main.ViewportToWorldPoint(new Vector3(0, 1.1f, 10f)).y;

        Vector3 spawnPos = new Vector3(spawnX, spawnY, 0f);
        GameObject newTetromino = Instantiate(prefab, spawnPos, Quaternion.identity);
        newTetromino.tag = "Tetromino";

        if (newTetromino.TryGetComponent<Rigidbody2D>(out var rb))
        {
            rb.gravityScale = 0f;
            rb.linearVelocity = Vector2.zero;
        }
    }

    void MoveTetrominos()
    {
        GameObject[] fallingPieces = GameObject.FindGameObjectsWithTag("Tetromino");

        foreach (GameObject piece in fallingPieces)
        {
            piece.transform.position += Vector3.down * fallSpeed * Time.deltaTime;

            // Check if it's below the screen
            float bottomY = Camera.main.ViewportToWorldPoint(new Vector3(0, -0.1f, 10f)).y;
            if (piece.transform.position.y < bottomY)
            {
                Destroy(piece);
            }
        }
    }
}