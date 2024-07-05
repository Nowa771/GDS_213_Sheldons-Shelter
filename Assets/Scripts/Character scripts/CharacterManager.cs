using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterManager : MonoBehaviour
{
    public List<Character> characters;
    public CharacterInfoDisplay characterInfoDisplay; // Reference to CharacterInfoDisplay

    // Threshold values for automatic eating and drinking
    public float hungerThreshold = 20.0f;
    public float thirstThreshold = 20.0f;

    // Cooldown times for eating and drinking (in seconds)
    public float eatCooldown = 5.0f;
    public float drinkCooldown = 5.0f;

    private Dictionary<Character, float> lastEatTimes = new Dictionary<Character, float>();
    private Dictionary<Character, float> lastDrinkTimes = new Dictionary<Character, float>();

    private int currentIndex = 0;

    void Start()
    {
        foreach (Character character in characters)
        {
            lastEatTimes[character] = Time.time;
            lastDrinkTimes[character] = Time.time;
        }

        if (characters.Count > 0)
        {
            SelectCharacter(currentIndex);
        }
    }

    void Update()
    {
        foreach (Character character in characters)
        {
            CheckCharacterNeeds(character);
        }

        if (Input.GetKeyDown(KeyCode.Q))
        {
            SelectPreviousCharacter();
        }
        else if (Input.GetKeyDown(KeyCode.E))
        {
            SelectNextCharacter();
        }
    }

    public void AddCharacter(Character character)
    {
        if (!characters.Contains(character))
        {
            characters.Add(character);
            lastEatTimes[character] = Time.time;
            lastDrinkTimes[character] = Time.time;
        }
    }

    void CheckCharacterNeeds(Character character)
    {
        if (character.hunger < hungerThreshold && Time.time > lastEatTimes[character] + eatCooldown)
        {
            character.TryToEat();
            lastEatTimes[character] = Time.time;
        }

        if (character.thirst < thirstThreshold && Time.time > lastDrinkTimes[character] + drinkCooldown)
        {
            character.TryToDrink();
            lastDrinkTimes[character] = Time.time;
        }
    }

    void SelectCharacter(int index)
    {
        if (characters.Count > 0 && index >= 0 && index < characters.Count)
        {
            if (characters[currentIndex] != null)
            {
                characters[currentIndex].GetComponent<CharacterMovement>().Deselect();
            }

            currentIndex = index;
            characters[currentIndex].GetComponent<CharacterMovement>().Select();
            characterInfoDisplay.SelectCharacter(characters[currentIndex]); // Notify CharacterInfoDisplay
        }
    }

    void SelectPreviousCharacter()
    {
        int previousIndex = (currentIndex - 1 + characters.Count) % characters.Count;
        SelectCharacter(previousIndex);
    }

    void SelectNextCharacter()
    {
        int nextIndex = (currentIndex + 1) % characters.Count;
        SelectCharacter(nextIndex);
    }
}
