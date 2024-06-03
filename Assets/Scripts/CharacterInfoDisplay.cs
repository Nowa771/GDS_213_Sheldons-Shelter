using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterInfoDisplay : MonoBehaviour
{
    public Text hungerText;
    public Text thirstText;
    public GameObject infoPanel;

    private Character selectedCharacter;

    void Start()
    {
        if (infoPanel != null)
        {
            infoPanel.SetActive(false); // Hide the panel initially
        }
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0)) // Left mouse button clicked
        {
            if (infoPanel != null && infoPanel.activeSelf) // If the panel is already active, close it
            {
                ClosePanel();
            }
            else
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                if (Physics.Raycast(ray, out RaycastHit hit))
                {
                    Character character = hit.collider.GetComponent<Character>();
                    if (character != null)
                    {
                        SelectCharacter(character);
                    }
                }
            }
        }

        if (selectedCharacter != null)
        {
            UpdateCharacterInfo();
        }
    }

    void SelectCharacter(Character character)
    {
        selectedCharacter = character;
        if (infoPanel != null)
        {
            infoPanel.SetActive(true); // Show the panel when a character is selected
        }
    }

    void UpdateCharacterInfo()
    {
        if (hungerText != null && thirstText != null && selectedCharacter != null)
        {
            hungerText.text = "Hunger: " + selectedCharacter.hunger.ToString("F2");
            thirstText.text = "Thirst: " + selectedCharacter.thirst.ToString("F2");
        }
    }

    void ClosePanel()
    {
        if (infoPanel != null)
        {
            infoPanel.SetActive(false);
        }
        selectedCharacter = null;
    }
}
