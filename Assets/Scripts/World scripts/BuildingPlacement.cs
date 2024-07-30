using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AI;
using UnityEngine.EventSystems;
using Unity.AI.Navigation;

public class BuildingPlacement : MonoBehaviour
{
    public List<GameObject> buildingPrefabs;
    public List<int> buildingCosts;
    public GridSystem gridSystem1;
    public GridSystem gridSystem2; // Second grid
    public GameObject selectionPanel;
    public RectTransform selectionPanelRectTransform; // RectTransform for sliding animation
    public GameObject buildingButtonPrefab; // Prefab for building buttons
    public Transform buttonContainer; // Parent object for the building buttons
    public NavMeshSurface navMeshSurface;
    public Button toggleBuildMenuButton; // Button to open and close the build menu

    private GameObject buildingPreview;
    private bool buildMode = false;
    private bool removeMode = false; // New variable to track remove mode
    private GameObject selectedBuildingPrefab;
    private int selectedBuildingCost;
    private Dictionary<Vector2Int, GameObject> placedBuildings1 = new Dictionary<Vector2Int, GameObject>();
    private Dictionary<Vector2Int, GameObject> placedBuildings2 = new Dictionary<Vector2Int, GameObject>();

    [SerializeField]
    private float navMeshBakeDelay = 1.0f; // Delay before baking NavMesh
    [SerializeField]
    private float slideDuration = 0.5f; // Duration of the sliding animation
    [SerializeField]
    private Vector2 slideInOffset = new Vector2(0, 0); // Offset for sliding in
    [SerializeField]
    private Vector2 slideOutOffset = new Vector2(-500, 0); // Offset for sliding out

    private Vector2 slideInPosition;
    private Vector2 slideOutPosition;

    void Start()
    {
        if (selectionPanelRectTransform == null)
        {
            selectionPanelRectTransform = selectionPanel.GetComponent<RectTransform>();
        }

        // Initialize positions based on offsets
        slideInPosition = selectionPanelRectTransform.anchoredPosition + slideInOffset;
        slideOutPosition = selectionPanelRectTransform.anchoredPosition + slideOutOffset;

        // Initially set the panel as active
        selectionPanel.SetActive(true);

        PopulateBuildingButtons();

        toggleBuildMenuButton.onClick.AddListener(ToggleBuildMode); // Link the button to ToggleBuildMode
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.B))
        {
            ToggleBuildMode();
        }

        if (buildMode && selectedBuildingPrefab != null)
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

        if (buildMode)
        {
            StartCoroutine(SlidePanel(selectionPanelRectTransform, slideInPosition, slideDuration));
        }
        else
        {
            StartCoroutine(SlidePanel(selectionPanelRectTransform, slideOutPosition, slideDuration, () =>
            {
                // Don't deactivate the panel, just move it out of view
                if (buildingPreview != null)
                {
                    Destroy(buildingPreview);
                }
                selectedBuildingPrefab = null; // Reset selected building
            }));
        }
    }

    void StartBuildingPreview()
    {
        if (selectedBuildingPrefab != null)
        {
            if (buildingPreview != null)
            {
                Destroy(buildingPreview);
            }
            buildingPreview = Instantiate(selectedBuildingPrefab);
            buildingPreview.SetActive(false); // Initially hidden
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

        if (GetGridPosition(mousePosition, gridSystem1, out x, out y) && gridSystem1.IsCellAvailable(x, y))
        {
            buildingPreview.SetActive(true);
            buildingPreview.transform.position = gridSystem1.GetCellCenter(x, y);
            buildingPreview.transform.rotation = Quaternion.Euler(0f, -90f, 0f);
        }
        else if (GetGridPosition(mousePosition, gridSystem2, out x, out y) && gridSystem2.IsCellAvailable(x, y))
        {
            buildingPreview.SetActive(true);
            buildingPreview.transform.position = gridSystem2.GetCellCenter(x, y);
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
        Vector2Int gridPos;

        if (GetGridPosition(mousePosition, gridSystem1, out x, out y) && gridSystem1.IsCellAvailable(x, y))
        {
            gridPos = new Vector2Int(x, y);
            if (Inventory.Instance.HasMaterials(selectedBuildingCost))
            {
                Quaternion rotation = Quaternion.Euler(0, -90, 0);
                GameObject newBuilding = Instantiate(selectedBuildingPrefab, gridSystem1.GetCellCenter(x, y), rotation);

                gridSystem1.OccupyCell(x, y);
                placedBuildings1[gridPos] = newBuilding;
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
        else if (GetGridPosition(mousePosition, gridSystem2, out x, out y) && gridSystem2.IsCellAvailable(x, y))
        {
            gridPos = new Vector2Int(x, y);
            if (Inventory.Instance.HasMaterials(selectedBuildingCost))
            {
                Quaternion rotation = Quaternion.Euler(0, -90, 0);
                GameObject newBuilding = Instantiate(selectedBuildingPrefab, gridSystem2.GetCellCenter(x, y), rotation);

                gridSystem2.OccupyCell(x, y);
                placedBuildings2[gridPos] = newBuilding;
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
        Vector2Int gridPos;

        if (GetGridPosition(mousePosition, gridSystem1, out x, out y))
        {
            gridPos = new Vector2Int(x, y);
            if (placedBuildings1.ContainsKey(gridPos))
            {
                GameObject buildingToRemove = placedBuildings1[gridPos];
                Destroy(buildingToRemove);
                placedBuildings1.Remove(gridPos);
                gridSystem1.ClearCell(x, y);

                // Trigger NavMesh baking after a delay
                StartCoroutine(DelayedNavMeshBake());
            }
        }
        else if (GetGridPosition(mousePosition, gridSystem2, out x, out y))
        {
            gridPos = new Vector2Int(x, y);
            if (placedBuildings2.ContainsKey(gridPos))
            {
                GameObject buildingToRemove = placedBuildings2[gridPos];
                Destroy(buildingToRemove);
                placedBuildings2.Remove(gridPos);
                gridSystem2.ClearCell(x, y);

                // Trigger NavMesh baking after a delay
                StartCoroutine(DelayedNavMeshBake());
            }
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

    bool GetGridPosition(Vector3 worldPosition, GridSystem gridSystem, out int x, out int y)
    {
        Vector3 localPosition = gridSystem.transform.InverseTransformPoint(worldPosition);
        x = Mathf.FloorToInt(localPosition.x / gridSystem.cellWidth);
        y = Mathf.FloorToInt(localPosition.z / gridSystem.cellHeight);
        return x >= 0 && y >= 0 && x < gridSystem.width && y < gridSystem.height;
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

        StartBuildingPreview(); // Start building preview
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
        return EventSystem.current.IsPointerOverGameObject();
    }

    IEnumerator SlidePanel(RectTransform panel, Vector2 targetPosition, float duration, System.Action onComplete = null)
    {
        Vector2 startPosition = panel.anchoredPosition;
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            panel.anchoredPosition = Vector2.Lerp(startPosition, targetPosition, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        panel.anchoredPosition = targetPosition;

        onComplete?.Invoke();
    }
}
