using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Inventory : MonoBehaviour
{
    // Static instance property
    public static Inventory Instance { get; private set; }

    public int foodCount;
    public int waterCount;
    public int materialCount; // New resource for materials

    public Text foodText;
    public Text waterText;
    public Text materialText; // Text component for materials

    private void Awake()
    {
        // Set the static instance property
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        UpdateUI();
    }

    // Method to add food
    public void AddFood(int amount)
    {
        foodCount += amount;
        UpdateUI();
    }

    // Method to remove food
    public void RemoveFood(int amount)
    {
        if (foodCount >= amount)
        {
            foodCount -= amount;
            UpdateUI();
        }
    }

    // Method to add water
    public void AddWater(int amount)
    {
        waterCount += amount;
        UpdateUI();
    }

    // Method to remove water
    public void RemoveWater(int amount)
    {
        if (waterCount >= amount)
        {
            waterCount -= amount;
            UpdateUI();
        }
    }

    // Method to add materials
    public void AddMaterials(int amount)
    {
        materialCount += amount;
        UpdateUI();
    }

    // Method to remove materials
    public void RemoveMaterials(int amount)
    {
        if (materialCount >= amount)
        {
            materialCount -= amount;
            UpdateUI();
        }
    }

    // Method to check food availability
    public bool HasFood(int amount)
    {
        return foodCount >= amount;
    }

    // Method to check water availability
    public bool HasWater(int amount)
    {
        return waterCount >= amount;
    }

    // Method to check materials availability
    public bool HasMaterials(int amount)
    {
        return materialCount >= amount;
    }

    // Method to update UI
    private void UpdateUI()
    {
        foodText.text = "Food: " + foodCount;
        waterText.text = "Water: " + waterCount;
        materialText.text = "Materials: " + materialCount; // Update materials UI text
    }
}