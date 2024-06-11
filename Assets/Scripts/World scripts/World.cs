using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class World : MonoBehaviour
{
    public Inventory inventory;

    void Start()
    {
        inventory = new Inventory();
    }

    public bool HasFood(int amount)
    {
        return inventory.HasFood(amount);
    }

    public bool HasWater(int amount)
    {
        return inventory.HasWater(amount);
    }

    public void RemoveFood(int amount)
    {
        inventory.RemoveFood(amount);
    }

    public void RemoveWater(int amount)
    {
        inventory.RemoveWater(amount);
    }
}