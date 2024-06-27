using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
<<<<<<< HEAD
    public LayerMask characterLayer; // Define the layer mask for characters
    private Character selectedCharacter;
=======
    public Camera mainCamera;
    private CharacterMovement selectedCharacter;
    public LayerMask characterLayer;
    public LayerMask roomLayer;
>>>>>>> new-character-movement

    void Update()
    {
        if (Input.GetMouseButtonDown(0)) // Left mouse button clicked
        {
<<<<<<< HEAD
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, Mathf.Infinity, characterLayer)) // Check for characters
            {
                Character character = hit.collider.GetComponent<Character>();
                if (character != null)
                {
                    SelectCharacter(character);
=======
            Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, characterLayer))
            {
                if (selectedCharacter != null)
                {
                    selectedCharacter.Deselect();
                }

                selectedCharacter = hit.collider.GetComponent<CharacterMovement>();
                if (selectedCharacter != null)
                {
                    selectedCharacter.Select();
                }
            }
            else if (selectedCharacter != null)
            {
                if (Physics.Raycast(ray, out hit, Mathf.Infinity, roomLayer))
                {
                    Room room = hit.collider.GetComponent<Room>();
                    if (room != null)
                    {
                        selectedCharacter.MoveToRoom(room);
                        selectedCharacter.Deselect();
                        selectedCharacter = null;
                    }
>>>>>>> new-character-movement
                }
            }
        }
    }
<<<<<<< HEAD

    void SelectCharacter(Character character)
    {
        selectedCharacter = character;
        Debug.Log("Selected character: " + selectedCharacter.name);

        // Additional logic for selecting a character, such as displaying info panel, etc.
    }
=======
>>>>>>> new-character-movement
}
