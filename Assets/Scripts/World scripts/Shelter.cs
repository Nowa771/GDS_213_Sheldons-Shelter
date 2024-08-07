using System.Collections.Generic;
using UnityEngine;
using System;

public class Shelter : MonoBehaviour
{
    public int currentCapacity = 0;
    public int maxCapacity = 10; // Example default max capacity, can be set in the inspector
    public string characterTag = "Player"; // Tag for characters in the scene

    private List<GameObject> addedResidents;

    public event Action OnCapacityChanged;

    void Start()
    {
        addedResidents = new List<GameObject>();
        UpdateCurrentCapacity(); // Initialize the current capacity
    }

    void UpdateCurrentCapacity()
    {
        // Count the number of characters in the scene by their tag
        GameObject[] characters = GameObject.FindGameObjectsWithTag(characterTag);
        currentCapacity = characters.Length;
        OnCapacityChanged?.Invoke();
        Debug.Log("Current capacity updated. Current capacity: " + currentCapacity);
    }

    public bool AddResident(GameObject resident)
    {
        // Add the resident and update capacity based on the total number of characters in the scene
        if (currentCapacity < maxCapacity)
        {
            addedResidents.Add(resident);
            UpdateCurrentCapacity(); // Update to reflect total number of characters
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
        if (addedResidents.Contains(resident))
        {
            addedResidents.Remove(resident);
            UpdateCurrentCapacity(); // Update to reflect total number of characters
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

    // Call this method to update the current capacity dynamically
    public void RefreshCurrentCapacity()
    {
        UpdateCurrentCapacity();
    }
}
