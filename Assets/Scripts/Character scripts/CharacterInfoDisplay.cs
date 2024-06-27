using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterInfoDisplay : MonoBehaviour
{
    public Text nameText;
    public Text hungerText;
    public Text thirstText;
    public Text healthText;
    public GameObject infoPanel;
    public Button eatButton;
    public Button drinkButton;
    public Button closeButton;

    private Character selectedCharacter;

    void Start()
    {
        if (infoPanel != null)
        {
            infoPanel.SetActive(false);
        }

        if (eatButton != null)
        {
            eatButton.onClick.AddListener(TryToEat);
        }
        if (drinkButton != null)
        {
            drinkButton.onClick.AddListener(TryToDrink);
        }

        if (closeButton != null)
        {
            closeButton.onClick.AddListener(ClosePanel);
        }
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(1)) // mouse button clicked
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                Character character = hit.collider.GetComponent<Character>();
                if (character != null)
                {
                    if (selectedCharacter == character && infoPanel.activeSelf)
                    {
                        if (!RectTransformUtility.RectangleContainsScreenPoint(eatButton.GetComponent<RectTransform>(), Input.mousePosition))
                        {
                            ClosePanel();
                        }
                    }
                    else
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
        if (selectedCharacter != null)
        {
            selectedCharacter.GetComponent<CharacterMovement>().Deselect();
        }

        selectedCharacter = character;
        selectedCharacter.GetComponent<CharacterMovement>().Select();

        if (infoPanel != null)
        {
            infoPanel.SetActive(true);
        }

        UpdateCharacterInfo();
    }

    void UpdateCharacterInfo()
    {
        if (nameText != null && hungerText != null && thirstText != null && healthText != null && selectedCharacter != null)
        {
            nameText.text = "Name: " + selectedCharacter.characterName;
            hungerText.text = "Hunger: " + selectedCharacter.hunger.ToString("F2"); // hunger value
            thirstText.text = "Thirst: " + selectedCharacter.thirst.ToString("F2"); // thirst value
            healthText.text = "Health: " + selectedCharacter.health.ToString("F2"); // health value
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

    void ClosePanel()
    {
        if (infoPanel != null)
        {
            infoPanel.SetActive(false);
        }
        if (selectedCharacter != null)
        {
            selectedCharacter.GetComponent<CharacterMovement>().Deselect();
            selectedCharacter = null;
        }
    }
}