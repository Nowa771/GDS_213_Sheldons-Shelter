using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LivingQuarters : MonoBehaviour
{
    public int capacityIncrease = 2; // The amount by which the room increases the shelter's capacity
    private bool isRoomActive = false; // To track if the room is active

    private Shelter shelter; // Reference to the shelter

    void Start()
    {
        // Assuming there's only one shelter in the scene and it has a "Shelter" component
        shelter = FindObjectOfType<Shelter>();

        if (shelter == null)
        {
            Debug.LogError("Shelter not found in the scene.");
            return;
        }

        ActivateRoom();
    }

    void ActivateRoom()
    {
        if (!isRoomActive)
        {
            shelter.IncreaseCapacity(capacityIncrease);
            isRoomActive = true;
        }
    }

    void OnDestroy()
    {
        if (isRoomActive)
        {
            shelter.DecreaseCapacity(capacityIncrease);
            isRoomActive = false;
        }
    }
}