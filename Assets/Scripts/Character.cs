using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    public string characterName;
    public float hunger;
    public float thirst;
    public float hungerDecayRate = 1f;
    public float thirstDecayRate = 1f;
    public float healthDecayRate = 1f;

    public float maxHunger = 100f;
    public float maxThirst = 100f;

    public Inventory inventory;

    void Update()
    {
        ManageNeeds();
    }

    void ManageNeeds()
    {
        hunger -= Time.deltaTime * hungerDecayRate;
        thirst -= Time.deltaTime * thirstDecayRate;

        if (hunger <= 0 || thirst <= 0)
        {
            // health -= Time.deltaTime * healthDecayRate;
        }

        if (Input.GetKeyDown(KeyCode.E)) // Example key for eating
        {
            TryToEat();
        }

        if (Input.GetKeyDown(KeyCode.R)) // Example key for drinking
        {
            TryToDrink();
        }
    }

    public void TryToEat()
    {
        if (inventory.HasFood(1))
        {
            Eat(10); // Adjust the food value as needed
            inventory.RemoveFood(1); // Remove one unit of food from inventory
        }
        else
        {
            Debug.Log("No food in inventory!");
        }
    }

    public void TryToDrink()
    {
        if (inventory.HasWater(1))
        {
            Drink(10); // Adjust the water value as needed
            inventory.RemoveWater(1); // Remove one unit of water from inventory
        }
        else
        {
            Debug.Log("No water in inventory!");
        }
    }

    public void Eat(float foodValue)
    {
        hunger = Mathf.Min(hunger + foodValue, maxHunger);
    }

    public void Drink(float waterValue)
    {
        thirst = Mathf.Min(thirst + waterValue, maxThirst);
    }
}