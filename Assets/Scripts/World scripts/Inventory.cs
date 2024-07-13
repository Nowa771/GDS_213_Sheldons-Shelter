using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Inventory : MonoBehaviour
{
    public static Inventory Instance { get; private set; }

    public int foodCount;
    public int waterCount;
    public int materialCount;
    public int medpackCount; // count

    public int maxFoodCount = 100;
    public int maxWaterCount = 100;
    public int maxMaterialCount = 100;
    public int maxMedpackCount = 50; // Maximum limits

    public RawImage foodImage;
    public RawImage waterImage;
    public RawImage materialImage;
    public RawImage medpackImage; //image

    public Text foodCountText;
    public Text waterCountText;
    public Text materialCountText;
    public Text medpackCountText; // count text

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
        foodCount = Mathf.Min(foodCount + amount, maxFoodCount);
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
        waterCount = Mathf.Min(waterCount + amount, maxWaterCount);
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
        materialCount = Mathf.Min(materialCount + amount, maxMaterialCount);
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

    public void AddMedpacks(int amount)
    {
        medpackCount = Mathf.Min(medpackCount + amount, maxMedpackCount);
        UpdateUI();
    }

    public void RemoveMedpacks(int amount)
    {
        if (medpackCount >= amount)
        {
            medpackCount -= amount;
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

    public bool HasMedpacks(int amount)
    {
        return medpackCount >= amount;
    }

    private void UpdateUI()
    {
        foodCountText.text = $"{foodCount} / {maxFoodCount}";
        waterCountText.text = $"{waterCount} / {maxWaterCount}";
        materialCountText.text = $"{materialCount} / {maxMaterialCount}";
        medpackCountText.text = $"{medpackCount} / {maxMedpackCount}";
    }
}