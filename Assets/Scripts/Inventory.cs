using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Inventory : MonoBehaviour
{
    public int foodCount;
    public int waterCount;

    public Text foodText;
    public Text waterText;

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

    // Method to update UI
    private void UpdateUI()
    {
        foodText.text = "Food: " + foodCount;
        waterText.text = "Water: " + waterCount;
    }
}