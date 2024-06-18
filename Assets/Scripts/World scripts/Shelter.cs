using System.Collections.Generic;
using UnityEngine;
using System;

public class Shelter : MonoBehaviour
{
    public int currentCapacity = 0;
    public int maxCapacity = 0;

    private List<GameObject> residents;

    public event Action OnCapacityChanged;

    void Start()
    {
        residents = new List<GameObject>();
    }

    public bool AddResident(GameObject resident)
    {
        if (currentCapacity < maxCapacity)
        {
            residents.Add(resident);
            currentCapacity++;
            OnCapacityChanged?.Invoke();
            Debug.Log("Resident added. Current capacity: " + currentCapacity);
            return true;
        }
        else
        {
            Debug.Log("Shelter is full. Cannot add resident.");
            return false;
        }
    }

    public bool RemoveResident(GameObject resident)
    {
        if (residents.Contains(resident))
        {
            residents.Remove(resident);
            currentCapacity--;
            OnCapacityChanged?.Invoke();
            Debug.Log("Resident removed. Current capacity: " + currentCapacity);
            return true;
        }
        else
        {
            Debug.Log("Resident not found in the shelter.");
            return false;
        }
    }

    public void IncreaseCapacity(int amount)
    {
        maxCapacity += amount;
        OnCapacityChanged?.Invoke();
        Debug.Log("Shelter capacity increased by " + amount + ". New max capacity: " + maxCapacity);
    }

    public void DecreaseCapacity(int amount)
    {
        maxCapacity -= amount;
        if (maxCapacity < 0) maxCapacity = 0;
        OnCapacityChanged?.Invoke();
        Debug.Log("Shelter capacity decreased by " + amount + ". New max capacity: " + maxCapacity);
    }

    public bool IsFull()
    {
        return currentCapacity >= maxCapacity;
    }

    public int GetCurrentCapacity()
    {
        return currentCapacity;
    }

    public int GetMaxCapacity()
    {
        return maxCapacity;
    }
}