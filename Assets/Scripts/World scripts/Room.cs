using System.Collections;
using UnityEngine;

public class Room : MonoBehaviour
{
    public float interval = 5f;
    public int baseFoodAmount = 1;
    public int baseWaterAmount = 1;
    public int baseMaterialAmount = 1;
    public string roomName = "Default Room";

    private int peopleInRoom = 0;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            peopleInRoom++;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            peopleInRoom--;
        }
    }

    private void Start()
    {
        StartCoroutine(AddResourcesOverTime());
    }

    private IEnumerator AddResourcesOverTime()
    {
        while (true)
        {
            yield return new WaitForSeconds(interval);
            if (peopleInRoom > 0)
            {
                int foodAmount = baseFoodAmount * peopleInRoom;
                int waterAmount = baseWaterAmount * peopleInRoom;
                int materialAmount = baseMaterialAmount * peopleInRoom;

                Inventory.Instance.AddFood(foodAmount); // food amount
                Inventory.Instance.AddWater(waterAmount); // water amount
                Inventory.Instance.AddMaterials(materialAmount); // material amount

                Debug.Log($"Resources added to inventory: Food {foodAmount}, Water {waterAmount}, Materials {materialAmount}");
            }
        }
    }

    private void OnMouseDown()
    {
        RoomSelectionManager.Instance.ShowRoomStats(this);
    }

    public string GetRoomStats()
    {
        return $"{roomName}\n" +
               $"Produces:\n" +
               $"- Food: {baseFoodAmount} per person per {interval} seconds\n" +
               $"- Water: {baseWaterAmount} per person per {interval} seconds\n" +
               $"- Materials: {baseMaterialAmount} per person per {interval} seconds";
    }
}