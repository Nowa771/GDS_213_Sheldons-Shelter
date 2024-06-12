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

    private World world;

    void Start()
    {
        world = FindObjectOfType<World>(); // Assuming there's only one World instance
    }

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
        if (world.HasFood(1))
        {
            Eat(10); 
            world.RemoveFood(1);
        }
        else
        {
            Debug.Log("No food in inventory!");
        }
    }

    public void TryToDrink()
    {
        if (world.HasWater(1))
        {
            Drink(10);
            world.RemoveWater(1);
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