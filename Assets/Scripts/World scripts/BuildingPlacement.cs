using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BuildingPlacement : MonoBehaviour
{
    public List<GameObject> buildingPrefabs;
    public List<int> buildingCosts; // List of material costs for each building
    public GridSystem gridSystem;
    public GameObject selectionPanel;

    private GameObject buildingPreview;
    private bool buildMode = false;
    private GameObject selectedBuildingPrefab;
    private int selectedBuildingCost;

    void Start()
    {
        selectionPanel.SetActive(false);
        if (buildingPrefabs.Count > 0)
        {
            selectedBuildingPrefab = buildingPrefabs[0];
            selectedBuildingCost = buildingCosts[0];
        }
    }

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
        selectionPanel.SetActive(buildMode);
        Time.timeScale = buildMode ? 0 : 1;

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
        if (selectedBuildingPrefab != null)
        {
            buildingPreview = Instantiate(selectedBuildingPrefab);
            buildingPreview.SetActive(false);
        }
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
            buildingPreview.SetActive(true);
            buildingPreview.transform.position = gridSystem.GetCellCenter(x, y);
        }
        else
        {
            buildingPreview.SetActive(false);
        }
    }

    void PlaceBuilding()
    {
        Vector3 mousePosition = GetMouseWorldPosition();
        int x, y;
        GetGridPosition(mousePosition, out x, out y);

        if (gridSystem.IsCellAvailable(x, y) && Inventory.Instance.HasMaterials(selectedBuildingCost))
        {
            Quaternion rotation = Quaternion.Euler(0, -90, 0);

            GameObject newBuilding = Instantiate(selectedBuildingPrefab, gridSystem.GetCellCenter(x, y), rotation);

            gridSystem.OccupyCell(x, y);
            Inventory.Instance.RemoveMaterials(selectedBuildingCost); // Deduct materials
            Destroy(buildingPreview);
        }
        else
        {
            // Optionally, provide feedback that the player does not have enough materials
            Debug.Log("Not enough materials to place the building.");
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
        x = Mathf.FloorToInt(localPosition.x / gridSystem.cellWidth);
        y = Mathf.FloorToInt(localPosition.z / gridSystem.cellHeight);
    }

    public void SelectBuilding(int index)
    {
        if (index >= 0 && index < buildingPrefabs.Count)
        {
            selectedBuildingPrefab = buildingPrefabs[index];
            selectedBuildingCost = buildingCosts[index]; // Update the cost for the selected building
            if (buildingPreview != null)
            {
                Destroy(buildingPreview);
                StartPlacingBuilding();
            }
            selectionPanel.SetActive(false);
            Time.timeScale = 1;
        }
    }
}