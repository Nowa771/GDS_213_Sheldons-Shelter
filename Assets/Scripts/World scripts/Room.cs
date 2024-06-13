using System.Collections;
using UnityEngine;

public class Room : MonoBehaviour
{
    public float interval = 5f;
    public int foodAmount = 1;
    public int waterAmount = 1;
    public int materialAmount = 1;

    private void Start()
    {
        StartCoroutine(AddResourcesOverTime());
    }

    private IEnumerator AddResourcesOverTime()
    {
        while (true)
        {
            yield return new WaitForSeconds(interval);
            Inventory.Instance.AddFood(foodAmount); // food amount
            Inventory.Instance.AddWater(waterAmount); // water amount
            Inventory.Instance.AddMaterials(materialAmount); // material amount

            Debug.Log("Resources added to inventory.");
        }
    }
}