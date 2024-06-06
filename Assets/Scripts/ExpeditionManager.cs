using System.Collections;
using UnityEngine;

public class ExpeditionManager : MonoBehaviour
{
    public static ExpeditionManager Instance;
    public float expeditionTime = 10f; // Time in seconds for the expedition
    public int foodGained = 5;
    public int waterGained = 3;
    public int materialsGained = 2;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void StartExpedition(GameObject character)
    {
        StartCoroutine(ExpeditionCoroutine(character));
    }

    private IEnumerator ExpeditionCoroutine(GameObject character)
    {
        // Temporarily store the original inventory counts
        int originalFoodCount = Inventory.Instance.foodCount;
        int originalWaterCount = Inventory.Instance.waterCount;
        int originalMaterialCount = Inventory.Instance.materialCount;

        // Make the character not visible
        Renderer renderer = character.GetComponent<Renderer>();
        if (renderer != null)
        {
            renderer.enabled = false;
        }

        yield return new WaitForSeconds(expeditionTime);

        // Enable the renderer component to make the character visible again
        if (renderer != null)
        {
            renderer.enabled = true;
        }

        Debug.Log("Expedition completed!");

        // Add the predefined gained resources to the inventory
        Inventory.Instance.AddFood(foodGained);
        Inventory.Instance.AddWater(waterGained);
        Inventory.Instance.AddMaterials(materialsGained);

        Debug.Log("Food Gained: " + foodGained);
        Debug.Log("Water Gained: " + waterGained);
        Debug.Log("Materials Gained: " + materialsGained);
    }
}