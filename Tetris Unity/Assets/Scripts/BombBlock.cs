// New script to create
// filepath: c:\Users\Lali\Downloads\dev\gamedev\tetrisgame\TetrisGame\Tetris Unity\Assets\Scripts\BombBlock.cs
using UnityEngine;

public class BombBlock : MonoBehaviour
{
    private bool hasExploded = false;
    
    // Called when the bomb is locked to the grid
    public void OnLock()
    {
        if (hasExploded) return; // Prevent multiple explosions
        
        Explode();
    }
    
    private void Explode()
    {
        hasExploded = true;
        Debug.Log("BombBlock exploding!");
        
        // Find the center of the bomb (usually the parent transform's position)
        Vector2 position = transform.position;
        Vector2Int gridPos = new Vector2Int(Mathf.RoundToInt(position.x), Mathf.RoundToInt(position.y));
        
        // Get reference to grid script
        GridScript grid = GameManager.Instance.GetComponent<GridScript>();
        if (grid != null)
        {
            grid.TriggerBombAt(gridPos);
        }
        else
        {
            Debug.LogError("GridScript not found!");
        }
    }
}