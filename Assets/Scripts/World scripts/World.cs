using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class World : MonoBehaviour
{
    public Inventory inventory;

    // Ensure that the inventory reference is properly assigned in the Inspector
    public void SetInventory(Inventory inv)
    {
        inventory = inv;
    }

    public bool HasFood(int amount)
    {
        if (inventory != null)
        {
            return inventory.HasFood(amount);
        }
        return false;
    }

    public bool HasWater(int amount)
    {
        if (inventory != null)
        {
            return inventory.HasWater(amount);
        }
        return false;
    }

    public void RemoveFood(int amount)
    {
        if (inventory != null)
        {
            inventory.RemoveFood(amount);
        }
    }

    public void RemoveWater(int amount)
    {
        if (inventory != null)
        {
            inventory.RemoveWater(amount);
        }
    }
}