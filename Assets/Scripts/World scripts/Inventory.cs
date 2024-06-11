using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Inventory : MonoBehaviour
{
    public static Inventory Instance { get; private set; }

    public int foodCount;
    public int waterCount;
    public int materialCount; // New resource for materials

    public Text foodText;
    public Text waterText;
    public Text materialText; // Text component for materials

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

    private void Start()
    {
        UpdateUI();
    }

    public void AddFood(int amount)
    {
        foodCount += amount;
        UpdateUI();
    }

    public void RemoveFood(int amount)
    {
        if (foodCount >= amount)
        {
            foodCount -= amount;
            UpdateUI();
        }
    }

    public void AddWater(int amount)
    {
        waterCount += amount;
        UpdateUI();
    }

    public void RemoveWater(int amount)
    {
        if (waterCount >= amount)
        {
            waterCount -= amount;
            UpdateUI();
        }
    }

    public void AddMaterials(int amount)
    {
        materialCount += amount;
        UpdateUI();
    }

    public void RemoveMaterials(int amount)
    {
        if (materialCount >= amount)
        {
            materialCount -= amount;
            UpdateUI();
        }
    }

    public bool HasFood(int amount)
    {
        return foodCount >= amount;
    }

    public bool HasWater(int amount)
    {
        return waterCount >= amount;
    }

    public bool HasMaterials(int amount)
    {
        return materialCount >= amount;
    }

    private void UpdateUI()
    {
        foodText.text = "Food: " + foodCount;
        waterText.text = "Water: " + waterCount;
        materialText.text = "Materials: " + materialCount; // Update materials UI text
    }
}