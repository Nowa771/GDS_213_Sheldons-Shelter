using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryUI : MonoBehaviour
{
    public static InventoryUI instance;

    public Text foodText;
    public Text waterText;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void UpdateInventoryUI(int foodCount, int waterCount)
    {
        foodText.text = "Food: " + foodCount.ToString();
        waterText.text = "Water: " + waterCount.ToString();
    }
}