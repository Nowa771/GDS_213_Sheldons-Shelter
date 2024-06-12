using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room : MonoBehaviour
{
    public float interval = 5f;
    public int foodAmount = 1; 
    public int waterAmount = 1; 
    public int materialAmount = 1;

    private bool isAddingResources = false;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (!isAddingResources)
            {
                StartCoroutine(AddResourcesOverTime());
            }
        }
    }

    private IEnumerator AddResourcesOverTime()
    {
        isAddingResources = true;
        while (true)
        {
            yield return new WaitForSeconds(interval);
            Inventory.Instance.AddFood(foodAmount);
            Inventory.Instance.AddWater(waterAmount);
            Inventory.Instance.AddMaterials(materialAmount); 

            Debug.Log("Resources added to inventory.");
        }
    }
}