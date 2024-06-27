using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    public Camera mainCamera;
    private CharacterMovement selectedCharacter;
    public LayerMask characterLayer;
    public LayerMask roomLayer;

    void Update()
    {
        if (Input.GetMouseButtonDown(0)) // Left mouse button clicked
        {
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
                }
            }
        }
    }
}
