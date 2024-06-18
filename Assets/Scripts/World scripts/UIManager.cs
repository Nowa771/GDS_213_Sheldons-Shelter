using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public Text capacityText;
    private Shelter shelter;

    void Start()
    {
        shelter = FindObjectOfType<Shelter>();

        if (shelter == null)
        {
            Debug.LogError("Shelter not found in the scene.");
            return;
        }

        UpdateCapacityText();
    }

    void Update()
    {
        UpdateCapacityText();
    }

    void UpdateCapacityText()
    {
        capacityText.text = "Capacity: " + shelter.GetCurrentCapacity() + "/" + shelter.GetMaxCapacity();
    }
}