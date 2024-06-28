using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AI;
using Unity.AI.Navigation;

public class BuildingPlacement : MonoBehaviour
{
    public List<GameObject> buildingPrefabs;
    public List<int> buildingCosts;
    public GridSystem gridSystem;
    public GameObject selectionPanel;
    public int initialSelectedIndex = 0; // Initial index of the selected building prefab
    public NavMeshSurface navMeshSurface;
    public Button[] buildingButtons; // Array of UI buttons representing each building
    public Button removeBuildingButton; // UI button for removing buildings

    private GameObject buildingPreview;
    private bool buildMode = false;
    private GameObject selectedBuildingPrefab;
    private int selectedBuildingCost;
    private int selectedIndex = -1; // Currently selected building index
    private Dictionary<Vector2Int, GameObject> placedBuildings = new Dictionary<Vector2Int, GameObject>();

    [SerializeField]
    private float navMeshBakeDelay = 1.0f; // Delay before baking NavMesh

    void Start()
    {
        selectionPanel.SetActive(false);

        // Select initial building prefab
        if (buildingPrefabs.Count > 0 && initialSelectedIndex >= 0 && initialSelectedIndex < buildingPrefabs.Count)
        {
            SetSelectedBuilding(initialSelectedIndex);
        }
        else
        {
            Debug.LogWarning("Initial selected index is out of range or buildingPrefabs list is empty.");
        }

        // Add listeners to building buttons
        for (int i = 0; i < buildingButtons.Length; i++)
        {
            int index = i; // Capture index in the lambda function
            buildingButtons[i].onClick.AddListener(() => SelectBuilding(index));
        }

        // Add listener to remove building button
        if (removeBuildingButton != null)
        {
            removeBuildingButton.onClick.AddListener(RemoveBuildingButtonClicked);
        }
        else
        {
            Debug.LogWarning("Remove building button reference not set in the Inspector.");
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.B))
        {
            ToggleBuildMode();
        }

        if (buildMode && selectedBuildingPrefab != null)
        {
            if (buildingPreview != null)
            {
                UpdateBuildingPlacement();
            }
            else
            {
                UpdateBuildingRemoval();
            }
        }
    }

    void ToggleBuildMode()
    {
        buildMode = !buildMode;
        selectionPanel.SetActive(buildMode);

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
            buildingPreview.transform.rotation = Quaternion.Euler(0f, -90f, 0f);
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
        Vector2Int gridPos = new Vector2Int(x, y);

        if (gridSystem.IsCellAvailable(x, y) && Inventory.Instance.HasMaterials(selectedBuildingCost))
        {
            Quaternion rotation = Quaternion.Euler(0, -90, 0);

            GameObject newBuilding = Instantiate(selectedBuildingPrefab, gridSystem.GetCellCenter(x, y), rotation);

            gridSystem.OccupyCell(x, y);
            placedBuildings[gridPos] = newBuilding;
            Inventory.Instance.RemoveMaterials(selectedBuildingCost); // Deduct materials
            Destroy(buildingPreview);

            // Trigger NavMesh baking after a delay
            StartCoroutine(DelayedNavMeshBake());
        }
        else
        {
            Debug.Log("Not enough materials to place the building or the cell is not available.");
        }
    }

    void UpdateBuildingRemoval()
    {
        if (Input.GetMouseButtonDown(0))
        {
            RemoveBuilding();
        }
    }

    void RemoveBuilding()
    {
        Vector3 mousePosition = GetMouseWorldPosition();
        int x, y;
        GetGridPosition(mousePosition, out x, out y);
        Vector2Int gridPos = new Vector2Int(x, y);

        if (placedBuildings.ContainsKey(gridPos))
        {
            GameObject buildingToRemove = placedBuildings[gridPos];
            Destroy(buildingToRemove);
            placedBuildings.Remove(gridPos);
            gridSystem.ClearCell(x, y);

            // Trigger NavMesh baking after a delay
            StartCoroutine(DelayedNavMeshBake());
        }
    }

    IEnumerator DelayedNavMeshBake()
    {
        yield return new WaitForSeconds(navMeshBakeDelay);

        if (navMeshSurface != null)
        {
            navMeshSurface.BuildNavMesh();
        }
        else
        {
            Debug.LogError("NavMeshSurface reference is missing.");
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

    // Method to select a building based on index
    void SelectBuilding(int index)
    {
        if (index >= 0 && index < buildingPrefabs.Count)
        {
            SetSelectedBuilding(index);
        }
    }

    // Method to set the selected building prefab and cost
    void SetSelectedBuilding(int index)
    {
        selectedBuildingPrefab = buildingPrefabs[index];
        selectedBuildingCost = buildingCosts[index];
        selectedIndex = index;

        // Destroy any existing building preview
        if (buildingPreview != null)
        {
            Destroy(buildingPreview);
        }

        // Start placing the selected building
        StartPlacingBuilding();
    }

    // Method called when remove building button is clicked
    void RemoveBuildingButtonClicked()
    {
        Vector3 mousePosition = GetMouseWorldPosition();
        int x, y;
        GetGridPosition(mousePosition, out x, out y);
        Vector2Int gridPos = new Vector2Int(x, y);

        RemoveBuilding(gridPos);
    }

    // Method to remove building at a specific grid position
    void RemoveBuilding(Vector2Int gridPos)
    {
        if (placedBuildings.ContainsKey(gridPos))
        {
            GameObject buildingToRemove = placedBuildings[gridPos];
            Destroy(buildingToRemove);
            placedBuildings.Remove(gridPos);
            gridSystem.ClearCell(gridPos.x, gridPos.y);

            // Trigger NavMesh baking after a delay
            StartCoroutine(DelayedNavMeshBake());
        }
    }
}
