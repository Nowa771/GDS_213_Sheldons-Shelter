using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance; // Singleton instance

    public CharacterMovement selectedCharacter; // Reference to the currently selected character

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject); // Ensure only one instance of GameManager exists
        }
    }

    void Update()
    {
        HandleInput(); // Call method to handle player input
    }

    void HandleInput()
    {
        if (Input.GetMouseButtonDown(0)) // Left mouse button clicked
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                Room room = hit.collider.GetComponent<Room>();
                if (room != null && selectedCharacter != null)
                {
                    selectedCharacter.MoveToRoom(room); // Move selected character to the room
                }
            }
        }
    }
}
