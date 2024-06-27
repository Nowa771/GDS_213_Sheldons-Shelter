using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    public LayerMask characterLayer; // Define the layer mask for characters
    private Character selectedCharacter;

    void Update()
    {
        if (Input.GetMouseButtonDown(0)) // Left mouse button clicked
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, Mathf.Infinity, characterLayer)) // Check for characters
            {
                Character character = hit.collider.GetComponent<Character>();
                if (character != null)
                {
                    SelectCharacter(character);
                }
            }
        }
    }

    void SelectCharacter(Character character)
    {
        selectedCharacter = character;
        Debug.Log("Selected character: " + selectedCharacter.name);

        // Additional logic for selecting a character, such as displaying info panel, etc.
    }
}
