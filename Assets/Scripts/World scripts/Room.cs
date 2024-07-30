using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room : MonoBehaviour
{
    public float baseInterval = 5f; // Base interval time in seconds
    public int baseFoodAmount = 1;
    public int baseWaterAmount = 1;
    public int baseMaterialAmount = 1;
    public int baseMedpackAmount = 1;
    public string roomName = "Default Room";

    public Transform[] spots; // spots in the room
    private List<Transform> availableSpots;
    private List<Transform> occupiedSpots = new List<Transform>();
    private List<Character> charactersInRoom = new List<Character>();

    private void Start()
    {
        availableSpots = new List<Transform>(spots);
        StartCoroutine(AddResourcesOverTime());
        Debug.Log("Room started and resource coroutine initiated.");
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Character character = other.GetComponent<Character>();
            if (character != null)
            {
                charactersInRoom.Add(character);
                Debug.Log($"Character {character.characterName} entered the room. Productivity: {character.productivity}%");
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Character character = other.GetComponent<Character>();
            if (character != null)
            {
                charactersInRoom.Remove(character);
                Debug.Log($"Character {character.characterName} left the room.");
            }
            CharacterMovement characterMovement = other.GetComponent<CharacterMovement>();
            if (characterMovement != null)
            {
                characterMovement.ClearAssignedSpot();
            }
        }
    }

    private IEnumerator AddResourcesOverTime()
    {
        while (true)
        {
            if (charactersInRoom.Count > 0)
            {
                // Calculate average productivity
                float totalProductivity = 0f;
                foreach (Character character in charactersInRoom)
                {
                    totalProductivity += character.productivity;
                }
                float averageProductivity = totalProductivity / charactersInRoom.Count;

                // Adjust interval based on productivity
                float productivityMultiplier = averageProductivity / 100f;
                float adjustedInterval = baseInterval / (1 + productivityMultiplier); // Shorter interval for higher productivity

                // Wait for the adjusted interval
                yield return new WaitForSeconds(adjustedInterval);

                // Add base amount of resources to inventory
                Inventory.Instance.AddFood(baseFoodAmount);
                Inventory.Instance.AddWater(baseWaterAmount);
                Inventory.Instance.AddMaterials(baseMaterialAmount);
                Inventory.Instance.AddMedpacks(baseMedpackAmount);

                Debug.Log($"Resources produced with average productivity {averageProductivity}% in {adjustedInterval} seconds: Food {baseFoodAmount}, Water {baseWaterAmount}, Materials {baseMaterialAmount}, Medpacks {baseMedpackAmount}");
            }
            else
            {
                // Wait for base interval if no characters are in the room
                yield return new WaitForSeconds(baseInterval);
            }
        }
    }

    public Transform GetAvailableSpot()
    {
        if (availableSpots.Count > 0)
        {
            Transform spot = availableSpots[0];
            availableSpots.RemoveAt(0);
            occupiedSpots.Add(spot);
            return spot;
        }
        return null; // No available spots
    }

    public void ReleaseSpot(Transform spot)
    {
        if (occupiedSpots.Contains(spot))
        {
            occupiedSpots.Remove(spot);
            availableSpots.Add(spot);
        }
    }
}
