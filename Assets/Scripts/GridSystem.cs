using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridSystem : MonoBehaviour
{
    public int width = 10;
    public int height = 10;
    public float cellSize = 1f;
    public Color gridColor = Color.white;

    private bool[,] gridArray;

    void Start()
    {
        gridArray = new bool[width, height];
    }

    void OnDrawGizmos()
    {
        DrawGrid();
    }

    void DrawGrid()
    {
        Gizmos.color = gridColor;

        Quaternion rotation = transform.rotation;

        // Draw vertical lines
        for (int x = 0; x <= width; x++)
        {
            Vector3 startPos = transform.position + rotation * new Vector3(x * cellSize, 0, 0);
            Vector3 endPos = startPos + rotation * new Vector3(0, 0, height * cellSize);
            Gizmos.DrawLine(startPos, endPos);
        }

        // Draw horizontal lines
        for (int y = 0; y <= height; y++)
        {
            Vector3 startPos = transform.position + rotation * new Vector3(0, 0, y * cellSize);
            Vector3 endPos = startPos + rotation * new Vector3(width * cellSize, 0, 0);
            Gizmos.DrawLine(startPos, endPos);
        }
    }

    public Vector3 GetCellCenter(int x, int y)
    {
        return transform.position + transform.rotation * (new Vector3(x, 0, y) * cellSize + new Vector3(cellSize, 0, cellSize) * 0.5f);
    }

    public bool IsCellAvailable(int x, int y)
    {
        return x >= 0 && y >= 0 && x < width && y < height && !gridArray[x, y];
    }

    public void OccupyCell(int x, int y)
    {
        gridArray[x, y] = true;
    }

    public void FreeCell(int x, int y)
    {
        gridArray[x, y] = false;
    }
}

