using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridSystem : MonoBehaviour
{
    public int width = 10;
    public int height = 10;
    public float cellWidth = 1f;
    public float cellHeight = 1f;
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
            Vector3 startPos = transform.position + rotation * new Vector3(x * cellWidth, 0, 0);
            Vector3 endPos = startPos + rotation * new Vector3(0, 0, height * cellHeight);
            Gizmos.DrawLine(startPos, endPos);
        }

        // Draw horizontal lines
        for (int y = 0; y <= height; y++)
        {
            Vector3 startPos = transform.position + rotation * new Vector3(0, 0, y * cellHeight);
            Vector3 endPos = startPos + rotation * new Vector3(width * cellWidth, 0, 0);
            Gizmos.DrawLine(startPos, endPos);
        }
    }

    public Vector3 GetCellCenter(int x, int y)
    {
        // Calculate the cell center with separate width and height
        Vector3 cellCenter = transform.position + transform.rotation * (new Vector3(x * cellWidth, 0, y * cellHeight) + new Vector3(cellWidth, 0, cellHeight) * 0.5f);
        Debug.Log($"Cell Center for ({x}, {y}): {cellCenter}");
        return cellCenter;
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

    public void PlacePrefab(GameObject prefab, int x, int y)
    {
        if (IsCellAvailable(x, y))
        {
            Vector3 cellCenter = GetCellCenter(x, y);
            Instantiate(prefab, cellCenter, Quaternion.identity);
            OccupyCell(x, y);
        }
        else
        {
            Debug.LogWarning("Cell is not available for placement.");
        }
    }
}