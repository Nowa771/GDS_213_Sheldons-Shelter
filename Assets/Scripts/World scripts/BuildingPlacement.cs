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
    public GameObject buildingButtonPrefab; // Prefab for building buttons
    public Transform buttonContainer; // Parent object for the building buttons
    public NavMeshSurface navMeshSurface;
    public Button closeButton; // Button to close the build menu

    private GameObject buildingPreview;
    private bool buildMode = false;
    private bool removeMode = false; // New variable to track remove mode
    private GameObject selectedBuildingPrefab;
    private int selectedBuildingCost;
    private Dictionary<Vector2Int, GameObject> placedBuildings = new Dictionary<Vector2Int, GameObject>();

    [SerializeField]
    private float navMeshBakeDelay = 1.0f; // Delay before baking NavMesh

    void Start()
    {
        selectionPanel.SetActive(false);
        PopulateBuildingButtons();

        if (buildingPrefabs.Count > 0)
        {
            selectedBuildingPrefab = buildingPrefabs[0];
            selectedBuildingCost = buildingCosts[0];
        }

        closeButton.onClick.AddListener(CloseBuildMenu);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.B))
        {
            ToggleBuildMode();
        }

        if (buildMode)
        {
            if (removeMode)
            {
                UpdateBuildingRemoval();
            }
            else
            {
                UpdateBuildingPlacement();
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

    void CloseBuildMenu()
    {
        buildMode = false;
        selectionPanel.SetActive(false);
        removeMode = false;

        if (buildingPreview != null)
        {
            Destroy(buildingPreview);
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
        if (buildingPreview != null && !IsPointerOverUI())
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
            Debug.Log("Not enough materials to place the building.");
        }
    }

    void UpdateBuildingRemoval()
    {
        if (Input.GetMouseButtonDown(0) && !IsPointerOverUI())
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

    void PopulateBuildingButtons()
    {
        foreach (Transform child in buttonContainer)
        {
            Destroy(child.gameObject); // Clear existing buttons
        }

        // Add a button for removing buildings first
        GameObject removeButtonObj = Instantiate(buildingButtonPrefab, buttonContainer);
        Button removeButton = removeButtonObj.GetComponent<Button>();
        removeButton.onClick.AddListener(OnRemoveButtonClicked);

        Text removeButtonText = removeButtonObj.GetComponentInChildren<Text>();
        removeButtonText.text = "Remove Building";

        for (int i = 0; i < buildingPrefabs.Count; i++)
        {
            GameObject buttonObj = Instantiate(buildingButtonPrefab, buttonContainer);
            Button button = buttonObj.GetComponent<Button>();
            int index = i; // Capture the current value of i

            button.onClick.AddListener(() => OnBuildingButtonClicked(index));

            Text buttonText = buttonObj.GetComponentInChildren<Text>();
            buttonText.text = buildingPrefabs[index].name;
        }
    }

    void OnBuildingButtonClicked(int index)
    {
        removeMode = false;
        selectedBuildingPrefab = buildingPrefabs[index];
        selectedBuildingCost = buildingCosts[index];

        if (buildingPreview != null)
        {
            Destroy(buildingPreview);
        }
        StartPlacingBuilding(); // Always start placing the building again
    }

    void OnRemoveButtonClicked()
    {
        removeMode = true;
        selectedBuildingPrefab = null;
        selectedBuildingCost = 0;

        if (buildingPreview != null)
        {
            Destroy(buildingPreview);
        }
    }

    bool IsPointerOverUI()
    {
        return UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject();
    }
}
