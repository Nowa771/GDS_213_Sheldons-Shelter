using System.Collections;
using UnityEngine;

public class Room : MonoBehaviour
{
    public float interval = 5f;
    public int foodAmount = 1;
    public int waterAmount = 1;
    public int materialAmount = 1;

    private bool playerInRoom = false;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRoom = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRoom = false;
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
            if (playerInRoom)
            {
                Inventory.Instance.AddFood(foodAmount); // food amount
                Inventory.Instance.AddWater(waterAmount); // water amount
                Inventory.Instance.AddMaterials(materialAmount); // material amount

                Debug.Log("Resources added to inventory.");
            }
        }
    }
}