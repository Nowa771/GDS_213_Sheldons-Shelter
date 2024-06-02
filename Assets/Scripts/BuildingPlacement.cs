using UnityEngine;

public class BuildingPlacement : MonoBehaviour
{
    public GameObject buildingPrefab;
    public GridSystem gridSystem;

    private GameObject buildingPreview;
    private bool buildMode = false;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.B))
        {
            ToggleBuildMode();
        }

        if (buildMode)
        {
            UpdateBuildingPlacement();
        }
    }

    void ToggleBuildMode()
    {
        buildMode = !buildMode;
        if (buildMode)
        {
            StartPlacingBuilding();
        }
        else
        {
            if (buildingPreview != null)
            {
                Destroy(buildingPreview);
            }
        }
    }

    void StartPlacingBuilding()
    {
        buildingPreview = Instantiate(buildingPrefab);
        buildingPreview.SetActive(false); // Disable initially to prevent it from blocking raycasts
    }

    void UpdateBuildingPlacement()
    {
        if (buildingPreview != null)
        {
            UpdateBuildingPreviewPosition();
            if (Input.GetMouseButtonDown(0))
            {
                PlaceBuilding();
            }
        }
    }

    void UpdateBuildingPreviewPosition()
    {
        Vector3 mousePosition = GetMouseWorldPosition();
        int x, y;
        GetGridPosition(mousePosition, out x, out y);

        if (gridSystem.IsCellAvailable(x, y))
        {
            buildingPreview.SetActive(true); // Enable building preview if it's in a valid position
            buildingPreview.transform.position = gridSystem.GetCellCenter(x, y);
        }
        else
        {
            buildingPreview.SetActive(false); // Disable building preview if it's in an invalid position
        }
    }

    void PlaceBuilding()
    {
        Vector3 mousePosition = GetMouseWorldPosition();
        int x, y;
        GetGridPosition(mousePosition, out x, out y);

        if (gridSystem.IsCellAvailable(x, y))
        {
            // Define the rotation you want for the prefab
            Quaternion rotation = Quaternion.Euler(0, -90, 0); // Example rotation (90 degrees around Y axis)

            // Instantiate the prefab with the specified rotation
            GameObject newBuilding = Instantiate(buildingPrefab, gridSystem.GetCellCenter(x, y), rotation);

            // Occupy the grid cell and destroy the building preview
            gridSystem.OccupyCell(x, y);
            Destroy(buildingPreview);
        }
    }

    Vector3 GetMouseWorldPosition()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            return hit.point;
        }
        return Vector3.zero;
    }

    void GetGridPosition(Vector3 worldPosition, out int x, out int y)
    {
        Vector3 localPosition = gridSystem.transform.InverseTransformPoint(worldPosition);
        x = Mathf.FloorToInt(localPosition.x / gridSystem.cellSize);
        y = Mathf.FloorToInt(localPosition.z / gridSystem.cellSize);
    }
}