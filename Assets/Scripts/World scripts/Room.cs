using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room : MonoBehaviour
{
    public float interval = 5f;
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
            yield return new WaitForSeconds(interval);
            if (charactersInRoom.Count > 0)
            {
                int totalFoodAmount = 0;
                int totalWaterAmount = 0;
                int totalMaterialAmount = 0;
                int totalMedpackAmount = 0;

                foreach (Character character in charactersInRoom)
                {
                    float productivityMultiplier = character.productivity / 100f;
                    totalFoodAmount += Mathf.RoundToInt(baseFoodAmount * productivityMultiplier);
                    totalWaterAmount += Mathf.RoundToInt(baseWaterAmount * productivityMultiplier);
                    totalMaterialAmount += Mathf.RoundToInt(baseMaterialAmount * productivityMultiplier);
                    totalMedpackAmount += Mathf.RoundToInt(baseMedpackAmount * productivityMultiplier);

                    Debug.Log($"Character {character.characterName} with productivity {character.productivity}% contributes: Food {Mathf.RoundToInt(baseFoodAmount * productivityMultiplier)}, Water {Mathf.RoundToInt(baseWaterAmount * productivityMultiplier)}, Materials {Mathf.RoundToInt(baseMaterialAmount * productivityMultiplier)}, Medpacks {Mathf.RoundToInt(baseMedpackAmount * productivityMultiplier)}");
                }

                Inventory.Instance.AddFood(totalFoodAmount); // Add food amount
                Inventory.Instance.AddWater(totalWaterAmount); // Add water amount
                Inventory.Instance.AddMaterials(totalMaterialAmount); // Add material amount
                Inventory.Instance.AddMedpacks(totalMedpackAmount); // Add medpack amount

                Debug.Log($"Total resources added to inventory: Food {totalFoodAmount}, Water {totalWaterAmount}, Materials {totalMaterialAmount}, Medpacks {totalMedpackAmount}");
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
