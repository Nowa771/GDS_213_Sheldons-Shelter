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
    public Text productivityText; // New text field for productivity
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
        if (Input.GetMouseButtonDown(0)) // mouse button clicked
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                Character character = hit.collider.GetComponent<Character>();
                if (character != null)
                {
                    Debug.Log("Character clicked: " + character.characterName);
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

    public void SelectCharacter(Character character)
    {
        Debug.Log("SelectCharacter called in CharacterInfoDisplay");

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
        if (nameText != null && hungerText != null && thirstText != null && healthText != null && productivityText != null && selectedCharacter != null)
        {
            nameText.text = "Name: " + selectedCharacter.characterName;
            hungerText.text = "Hunger: " + Mathf.Min(Mathf.RoundToInt((selectedCharacter.hunger / selectedCharacter.maxHunger) * 100), 100).ToString() + "%"; // hunger percentage
            thirstText.text = "Thirst: " + Mathf.Min(Mathf.RoundToInt((selectedCharacter.thirst / selectedCharacter.maxThirst) * 100), 100).ToString() + "%"; // thirst percentage
            healthText.text = "Health: " + Mathf.RoundToInt((selectedCharacter.health / selectedCharacter.maxHealth) * 100).ToString() + "%"; // health percentage
            productivityText.text = "Productivity: " + Mathf.RoundToInt(selectedCharacter.productivity).ToString() + "%"; // productivity percentage
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
