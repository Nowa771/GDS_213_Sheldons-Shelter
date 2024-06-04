using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterInfoDisplay : MonoBehaviour
{
    public Text nameText; // Reference to the name Text element
    public Text hungerText;
    public Text thirstText;
    public GameObject infoPanel;
    public Button eatButton;
    public Button drinkButton;
    public Button closeButton; // New close button

    private Character selectedCharacter;

    void Start()
    {
        if (infoPanel != null)
        {
            infoPanel.SetActive(false); // Hide the panel initially
        }

        // Add button listeners
        if (eatButton != null)
        {
            eatButton.onClick.AddListener(TryToEat);
        }
        if (drinkButton != null)
        {
            drinkButton.onClick.AddListener(TryToDrink);
        }

        if (closeButton != null) // Add listener for close button
        {
            closeButton.onClick.AddListener(ClosePanel);
        }
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0)) // Left mouse button clicked
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

        if (selectedCharacter != null)
        {
            UpdateCharacterInfo();
        }
    }

    void SelectCharacter(Character character)
    {
        if (selectedCharacter != null)
        {
            selectedCharacter.GetComponent<CharacterMovement>().Deselect();
        }

        selectedCharacter = character;
        selectedCharacter.GetComponent<CharacterMovement>().Select();

        if (infoPanel != null)
        {
            infoPanel.SetActive(true); // Show the panel when a character is selected
        }
    }

    void UpdateCharacterInfo()
    {
        if (nameText != null && hungerText != null && thirstText != null && selectedCharacter != null)
        {
            nameText.text = "Name: " + selectedCharacter.characterName;
            hungerText.text = "Hunger: " + selectedCharacter.hunger.ToString("F2");
            thirstText.text = "Thirst: " + selectedCharacter.thirst.ToString("F2");
        }
    }

    void TryToEat()
    {
        if (selectedCharacter != null)
        {
            selectedCharacter.TryToEat();
            UpdateCharacterInfo();
        }
    }

    void TryToDrink()
    {
        if (selectedCharacter != null)
        {
            selectedCharacter.TryToDrink();
            UpdateCharacterInfo();
        }
    }

    void ClosePanel() // Method to close the panel
    {
        if (infoPanel != null)
        {
            infoPanel.SetActive(false);
            selectedCharacter.GetComponent<CharacterMovement>().Deselect(); // Deselect the character
            selectedCharacter = null; // Reset selected character
        }
    }
}